using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SPSS.Service.Interfaces;
using SPSS.BusinessObject.Dto.Reply;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Shared.Constants;
using System;
using System.Threading.Tasks;

namespace SPSS.Service.Implementations
{
	public class ReplyService : IReplyService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IReviewRepository _reviewRepository;
		private readonly IReplyRepository _replyRepository;

		public ReplyService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_reviewRepository = _unitOfWork.GetRepository<IReviewRepository>();
			_replyRepository = _unitOfWork.GetRepository<IReplyRepository>();
		}

		public async Task<ReplyDto> CreateAsync(Guid userId, ReplyForCreationDto replyDto)
		{
			if (replyDto == null)
				throw new ArgumentNullException(nameof(replyDto), ExceptionMessageConstants.Reply.ReplyDataNull);

			var reviewExists = await _reviewRepository.Entities.AnyAsync(r => r.Id == replyDto.ReviewId);
			if (!reviewExists)
				throw new ArgumentException(ExceptionMessageConstants.Reply.ReviewNotFound, nameof(replyDto.ReviewId));

			var reply = new Reply
			{
				Id = Guid.NewGuid(),
				ReviewId = replyDto.ReviewId,
				ReplyContent = replyDto.ReplyContent,
				CreatedTime = DateTimeOffset.UtcNow,
				UserId = userId,
				CreatedBy = userId.ToString(),
				LastUpdatedTime = DateTimeOffset.UtcNow,
				LastUpdatedBy = userId.ToString(),
				IsDeleted = false
			};

			try
			{
				_replyRepository.Add(reply);
				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format(ExceptionMessageConstants.Reply.FailedToSave, ex.Message), ex);
			}

			var replyDtoResult = new ReplyDto
			{
				Id = reply.Id,
				ReplyContent = reply.ReplyContent,
			};

			return replyDtoResult;
		}

		public async Task<ReplyDto> UpdateAsync(Guid userId, ReplyForUpdateDto replyDto, Guid id)
		{
			if (replyDto == null)
				throw new ArgumentNullException(nameof(replyDto), ExceptionMessageConstants.Reply.ReplyDataNull);

			var reply = await _replyRepository.GetByIdAsync(id);
			if (reply == null)
				throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Reply.ReplyNotFound, id));

			_mapper.Map(replyDto, reply);
			reply.LastUpdatedTime = DateTimeOffset.UtcNow;
			reply.LastUpdatedBy = userId.ToString();
			_replyRepository.Update(reply);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<ReplyDto>(reply);
		}

		public async Task DeleteAsync(Guid userId, Guid id)
		{
			var reply = await _replyRepository.GetByIdAsync(id);
			if (reply == null)
				throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Reply.ReplyNotFound, id));

			_replyRepository.Delete(reply);
			await _unitOfWork.SaveChangesAsync();
		}
	}
}
