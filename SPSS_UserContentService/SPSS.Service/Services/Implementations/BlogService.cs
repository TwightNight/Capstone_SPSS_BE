using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SPSS.BusinessObject.Dto.Blog;
using SPSS.BusinessObject.Dto.BlogSection;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Constants;
using SPSS.Shared.Responses;

namespace SPSS.Service.Services.Implementations;

public class BlogService : IBlogService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IBlogRepository _blogRepo;
	private readonly IBlogSectionRepository _sectionRepo;

	public BlogService(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		// Sử dụng đúng UoW pattern
		_blogRepo = _unitOfWork.GetRepository<IBlogRepository>();
		_sectionRepo = _unitOfWork.GetRepository<IBlogSectionRepository>();
	}

	public async Task<BlogWithDetailDto> GetByIdAsync(Guid id)
	{
		// Dùng GetSingleAsync của Repository
		var blog = await _blogRepo.GetSingleAsync(
			predicate: b => b.Id == id && !b.IsDeleted,
			include: q => q.Include(b => b.User)
						   .Include(b => b.BlogSections)
		);

		if (blog == null)
			throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Blog.NotFound, id));

		// Dùng AutoMapper
		return _mapper.Map<BlogWithDetailDto>(blog);
	}

	public async Task<PagedResponse<BlogDto>> GetPagedAsync(int pageNumber, int pageSize)
	{
		// Dùng GetPagedAsync có sẵn trong RepositoryBase
		var (blogs, totalCount) = await _blogRepo.GetPagedAsync(
			pageNumber: pageNumber,
			pageSize: pageSize,
			filter: b => !b.IsDeleted,
			orderBy: q => q.OrderByDescending(b => b.LastUpdatedTime),
			include: q => q.Include(b => b.User)
		);

		// Map kết quả
		var blogDtos = _mapper.Map<List<BlogDto>>(blogs);

		// Dùng đúng constructor của PagedResponse
		return new PagedResponse<BlogDto>(blogDtos, totalCount, pageNumber, pageSize);
	}

	public async Task<BlogDto> CreateBlogAsync(BlogForCreationDto blogDto, Guid userId)
	{
		if (blogDto == null)
			throw new ArgumentNullException(nameof(blogDto), ExceptionMessageConstants.Blog.BlogDataNull);

		// Map bằng AutoMapper
		var blog = _mapper.Map<Blog>(blogDto);

		// Set các trường hệ thống
		blog.Id = Guid.NewGuid();
		blog.UserId = userId;
		blog.CreatedTime = DateTimeOffset.UtcNow;
		blog.LastUpdatedTime = DateTimeOffset.UtcNow;
		blog.CreatedBy = userId.ToString();
		blog.LastUpdatedBy = userId.ToString();
		blog.IsDeleted = false;

		// Set Id và BlogId cho các child section
		foreach (var section in blog.BlogSections)
		{
			section.Id = Guid.NewGuid();
			section.BlogId = blog.Id; // EF Core cần điều này
		}

		// Dùng Transaction
		await _unitOfWork.BeginTransactionAsync();
		try
		{
			_blogRepo.Add(blog); // EF Core sẽ tự động add các child sections
			await _unitOfWork.CommitTransactionAsync();
		}
		catch (Exception ex)
		{
			await _unitOfWork.RollbackTransactionAsync();
			throw new Exception(string.Format(ExceptionMessageConstants.Blog.FailedToCreate, ex.Message), ex);
		}

		// Map và trả về
		return _mapper.Map<BlogDto>(blog);
	}

	public async Task<BlogDto> UpdateBlogAsync(Guid blogId, BlogForUpdateDto blogDto, Guid userId)
	{
		if (blogDto == null)
			throw new ArgumentNullException(nameof(blogDto), ExceptionMessageConstants.Blog.BlogDataNull);

		// Dùng transaction
		await _unitOfWork.BeginTransactionAsync();
		try
		{
			var blog = await _blogRepo.GetSingleAsync(
				predicate: b => b.Id == blogId && !b.IsDeleted,
				include: q => q.Include(b => b.BlogSections)
			);

			if (blog == null)
				throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Blog.NotFound, blogId));

			// Map các thuộc tính của Blog (BlogSections được ignore)
			_mapper.Map(blogDto, blog);
			blog.LastUpdatedTime = DateTimeOffset.UtcNow;
			blog.LastUpdatedBy = userId.ToString();

			// Xử lý sync (đồng bộ) BlogSections
			SyncBlogSectionsAsync(blog, blogDto.Sections);
			_blogRepo.Update(blog);
			await _unitOfWork.CommitTransactionAsync();

			return _mapper.Map<BlogDto>(blog);
		}
		catch (Exception ex)
		{
			await _unitOfWork.RollbackTransactionAsync();
			throw new Exception(string.Format(ExceptionMessageConstants.Blog.FailedToUpdate, ex.Message), ex);
		}
	}

	public async Task<bool> DeleteAsync(Guid id, Guid userId)
	{
		var blog = await _blogRepo.GetByIdAsync(id);

		if (blog == null || blog.IsDeleted)
			throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Blog.NotFound, id));

		blog.IsDeleted = true;
		blog.DeletedTime = DateTimeOffset.UtcNow;
		blog.DeletedBy = userId.ToString();

		_blogRepo.Update(blog);
		await _unitOfWork.SaveChangesAsync();
		return true;
	}

	/// <summary>
	/// (Hàm private) Đồng bộ collection BlogSections.
	/// </summary>
	private void SyncBlogSectionsAsync(Blog blog, List<BlogSectionForUpdateDto> sectionDtos)
	{
		var existingSections = blog.BlogSections.ToDictionary(s => s.Id);
		var sectionsInDto = sectionDtos.ToHashSet();

		// 1. Xóa các section không còn tồn tại trong DTO
		var sectionsToDelete = existingSections.Values
			.Where(s => !sectionsInDto.Any(dto => dto.Id == s.Id))
			.ToList();

		_sectionRepo.RemoveRange(sectionsToDelete);

		// 2. Cập nhật hoặc Thêm mới
		foreach (var dto in sectionDtos)
		{
			if (existingSections.TryGetValue(dto.Id, out var existingSection))
			{
				// Cập nhật section đã tồn tại
				_mapper.Map(dto, existingSection);
				_sectionRepo.Update(existingSection);
			}
			else
			{
				// Thêm section mới
				var newSection = _mapper.Map<BlogSection>(dto);
				newSection.Id = Guid.NewGuid(); // Gán Id mới
				newSection.BlogId = blog.Id;
				_sectionRepo.Add(newSection);
			}
		}
	}
}
