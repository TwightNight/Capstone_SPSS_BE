using Microsoft.EntityFrameworkCore;
using SPSS.BusinessObject.DTOs.Brand;
using SPSS.BusinessObject.Models;
using SPSS.Repository.Repositories.Interfaces;
using SPSS.Repository.UnitOfWork.Interfaces;
using SPSS.Service.Services.Interfaces;
using SPSS.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPSS.Service.Services.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBrandRepository _brandRepository;

        public BrandService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _brandRepository = _unitOfWork.GetRepository<IBrandRepository>();
        }

        public async Task<BrandResponse> GetBrandByIdAsync(Guid id)
        {
            var brand = await _brandRepository.GetSingleAsync(
                predicate: b => b.Id == id,
                include: b => b.Include(c => c.Country)
            );

            if (brand == null)
            {
                throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Brand.NotFound, id));
            }

            return new BrandResponse
            {
                Id = brand.Id,
                Name = brand.Name,
                Title = brand.Title,
                Description = brand.Description,
                ImageUrl = brand.ImageUrl,
                CountryId = brand.CountryId,
                CountryName = brand.Country?.CountryName 
            };
        }

        public async Task<IEnumerable<BrandResponse>> GetAllBrandsAsync()
        {
            var brands = await _brandRepository.GetAsync(
                include: b => b.Include(c => c.Country),
                orderBy: q => q.OrderBy(b => b.Name)
            );

            return brands.Select(brand => new BrandResponse
            {
                Id = brand.Id,
                Name = brand.Name,
                Title = brand.Title,
                Description = brand.Description,
                ImageUrl = brand.ImageUrl,
                CountryId = brand.CountryId,
                CountryName = brand.Country?.CountryName
            });
        }

        public async Task<BrandResponse> CreateBrandAsync(CreateBrandRequest createBrandRequest)
        {
            if (createBrandRequest == null)
            {
                throw new ArgumentNullException(ExceptionMessageConstants.Brand.BrandDataNull);
            }

            // Kiểm tra xem tên thương hiệu đã tồn tại chưa
            var existingBrand = await _brandRepository.ExistsAsync(b => b.Name == createBrandRequest.Name);
            if (existingBrand)
            {
                throw new InvalidOperationException(string.Format(ExceptionMessageConstants.Brand.NameAlreadyExists, createBrandRequest.Name));
            }

            var brand = new Brand
            {
                Id = Guid.NewGuid(),
                Name = createBrandRequest.Name,
                Title = createBrandRequest.Title,
                Description = createBrandRequest.Description,
                ImageUrl = createBrandRequest.ImageUrl,
                CountryId = createBrandRequest.CountryId
            };

            try
            {
                _brandRepository.Add(brand);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Ghi log lỗi (tùy chọn) và throw exception
                throw new Exception(string.Format(ExceptionMessageConstants.Brand.FailedToCreate, ex.Message));
            }

            // Lấy lại thông tin country để trả về response đầy đủ
            var createdBrand = await GetBrandByIdAsync(brand.Id);
            return createdBrand;
        }

        public async Task UpdateBrandAsync(UpdateBrandRequest updateBrandRequest)
        {
            if (updateBrandRequest == null)
            {
                throw new ArgumentNullException(ExceptionMessageConstants.Brand.BrandDataNull);
            }

            var brandToUpdate = await _brandRepository.GetByIdAsync(updateBrandRequest.Id);
            if (brandToUpdate == null)
            {
                throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Brand.NotFound, updateBrandRequest.Id));
            }

            // Kiểm tra nếu tên mới đã tồn tại ở một brand khác
            var existingBrand = await _brandRepository.ExistsAsync(b => b.Name == updateBrandRequest.Name && b.Id != updateBrandRequest.Id);
            if (existingBrand)
            {
                throw new InvalidOperationException(string.Format(ExceptionMessageConstants.Brand.NameAlreadyExists, updateBrandRequest.Name));
            }

            // Cập nhật các thuộc tính
            brandToUpdate.Name = updateBrandRequest.Name;
            brandToUpdate.Title = updateBrandRequest.Title;
            brandToUpdate.Description = updateBrandRequest.Description;
            brandToUpdate.ImageUrl = updateBrandRequest.ImageUrl;
            brandToUpdate.CountryId = updateBrandRequest.CountryId;

            try
            {
                _brandRepository.Update(brandToUpdate);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ExceptionMessageConstants.Brand.FailedToUpdate, ex.Message));
            }
        }

        public async Task DeleteBrandAsync(Guid id)
        {
            var brandToDelete = await _brandRepository.GetSingleAsync(
                b => b.Id == id,
                include: b => b.Include(p => p.Products)
            );

            if (brandToDelete == null)
            {
                throw new KeyNotFoundException(string.Format(ExceptionMessageConstants.Brand.NotFound, id));
            }

            // Kiểm tra xem thương hiệu có đang được sử dụng bởi sản phẩm nào không
            if (brandToDelete.Products.Any())
            {
                throw new InvalidOperationException(ExceptionMessageConstants.Brand.InUseByProducts);
            }

            try
            {
                _brandRepository.Delete(brandToDelete);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(ExceptionMessageConstants.Brand.FailedToDelete, ex.Message));
            }
        }
    }
}