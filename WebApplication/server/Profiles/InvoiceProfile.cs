using AutoMapper;
using BeautySaloon.Api.Dto.Requests.Invoice;
using BeautySaloon.Api.Dto.Responses.Common;
using BeautySaloon.Api.Dto.Responses.Invoice;
using WebApplication.Pages;

namespace WebApplication.Profiles;

public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<GetInvoiceResponseDto, EditInvoiceComponent.InvoiceRequest>();

        CreateMap<MaterialResponseDto, EditInvoiceComponent.InvoiceRequest.MaterialRequest>();

        CreateMap<AddOrEditInvoiceMaterialComponent.InvoiceMaterialRequest, EditInvoiceComponent.InvoiceRequest.MaterialRequest>();

        CreateMap<AddOrEditInvoiceMaterialComponent.InvoiceMaterialRequest, AddInvoiceComponent.InvoiceRequest.MaterialRequest>();

        CreateMap<EditInvoiceComponent.InvoiceRequest, UpdateInvoiceRequestDto>();

        CreateMap<EditInvoiceComponent.InvoiceRequest.MaterialRequest, MaterialRequestDto>();

        CreateMap<EditInvoiceComponent.InvoiceRequest.MaterialRequest, AddOrEditInvoiceMaterialComponent.InvoiceMaterialRequest>();

        CreateMap<AddInvoiceComponent.InvoiceRequest, CreateInvoiceRequestDto>();

        CreateMap<AddInvoiceComponent.InvoiceRequest.MaterialRequest, MaterialRequestDto>();

        CreateMap<AddInvoiceComponent.InvoiceRequest.MaterialRequest, AddOrEditInvoiceMaterialComponent.InvoiceMaterialRequest>();
    }
}
