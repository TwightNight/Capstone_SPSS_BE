using System;

namespace SPSS.Shared.DTOs.Brand
{
    public class BrandResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
    }
}