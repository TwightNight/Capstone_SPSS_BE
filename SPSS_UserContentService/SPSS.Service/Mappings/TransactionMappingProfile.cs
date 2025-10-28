using AutoMapper;
using BusinessObjects.Dto.Transaction;
using SPSS.BusinessObject.Dto.Transaction;
using SPSS.BusinessObject.Models;
using SPSS.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SPSS.Service.Mappings;

public class TransactionMappingProfile : Profile
{
    public TransactionMappingProfile()
    {
        CreateMap<Transaction, TransactionDto>()
            .ForMember(dest => dest.UserName,
                       opt => opt.MapFrom(src => src.User != null ? src.User.UserName : "Unknown"));

        CreateMap<CreateTransactionDto, Transaction>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => TransactionStatus.Pending))
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.LastUpdatedTime, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.QrImageUrl, opt => opt.Ignore())
            .ForMember(dest => dest.BankInformation, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.LastUpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedTime, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());
    }
}
