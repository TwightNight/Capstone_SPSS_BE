using AutoMapper;
using SPSS.BusinessObject.Dto.Address;
using SPSS.BusinessObject.Models;

namespace SPSS.Service.Mappings;

public class AddressMappingProfile : Profile
{
    public AddressMappingProfile()
    {
        // Map từ Entity (Address) sang DTO (AddressDto)
        CreateMap<Address, AddressDto>()
            .ForMember(
                dest => dest.CountryName,
                opt => opt.MapFrom(src => src.Country != null ? src.Country.CountryName : "Unknown")
            )
            .ForMember(
                dest => dest.PostCode, // Map Postcode (entity) sang PostCode (DTO)
                opt => opt.MapFrom(src => src.Postcode)
            );

        // Map từ DTO tạo mới (AddressForCreationDto) sang Entity (Address)
        CreateMap<AddressForCreationDto, Address>()
            .ForMember(
                dest => dest.Postcode, // Map Postcode (DTO) sang Postcode (entity)
                opt => opt.MapFrom(src => src.Postcode)
            );

        // Map từ DTO cập nhật (AddressForUpdateDto) sang Entity (Address)
        CreateMap<AddressForUpdateDto, Address>()
            .ForMember(
                dest => dest.Postcode, // Map Postcode (DTO) sang Postcode (entity)
                opt => opt.MapFrom(src => src.Postcode)
            );
    }
}
