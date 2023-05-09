using AutoMapper;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using BeautySaloon.DAL.Entities;
using BeautySaloon.Api.Dto.Responses.Invoice;
using BeautySaloon.Api.Dto.Responses.Common;

namespace BeautySaloon.Core.Profiles;

public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<Invoice, GetInvoiceResponseDto>()
            .ForMember(dest => dest.Materials, cfg => cfg.MapFrom(src => src.InvoiceMaterials))
            .ForMember(dest => dest.Employee, cfg => cfg.MapFrom(src => src.Employee));

        CreateMap<InvoiceMaterial, MaterialResponseDto>()
            .ForMember(dest => dest.Id, cfg => cfg.MapFrom(src => src.Material.Id))
            .ForMember(dest => dest.Name, cfg => cfg.MapFrom(src => src.Material.Name))
            .ForMember(dest => dest.Description, cfg => cfg.MapFrom(src => src.Material.Description))
            .ForMember(dest => dest.Cost, cfg => cfg.MapFrom(src => src.Cost))
            .ForMember(dest => dest.Count, cfg => cfg.MapFrom(src => src.Count));

        CreateMap<User, UserResponseDto>();
    }
}
