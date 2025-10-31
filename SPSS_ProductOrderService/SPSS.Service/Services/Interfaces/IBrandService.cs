using SPSS.BusinessObject.DTOs.Brand;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SPSS.Service.Services.Interfaces
{
    public interface IBrandService
    {
        Task<BrandResponse> GetBrandByIdAsync(Guid id);
        Task<IEnumerable<BrandResponse>> GetAllBrandsAsync();
        Task<BrandResponse> CreateBrandAsync(CreateBrandRequest createBrandRequest);
        Task UpdateBrandAsync(UpdateBrandRequest updateBrandRequest);
        Task DeleteBrandAsync(Guid id);
    }
}