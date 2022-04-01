using FSH.WebApi.Application.Exchange.Groups;
using FSH.WebApi.Application.Exchange.Inquiries.DTOs;
using FSH.WebApi.Application.Exchange.Offers.DTOs;
using FSH.WebApi.Application.Exchange.Orders;
using FSH.WebApi.Application.Exchange.Traders;
using FSH.WebApi.Domain.Exchange;
using Mapster;

namespace FSH.WebApi.Infrastructure.Mapping;

public class MapsterSettings
{
    public static void Configure()
    {
        // here we will define the type conversion / Custom-mapping
        // More details at https://github.com/MapsterMapper/Mapster/wiki/Custom-mapping

        // This one is actually not necessary as it's mapped by convention
        // TypeAdapterConfig<Product, ProductDto>.NewConfig().Map(dest => dest.BrandName, src => src.Brand.Name);

        TypeAdapterConfig<Group, GroupDetailsDto>.NewConfig()
            .Map(dest => dest.Traders, src => src.TraderGroups.Select(tg => tg.Trader));

        TypeAdapterConfig<Trader, TraderDetailsDto>.NewConfig()
            .Map(dest => dest.Groups, src => src.TraderGroups.Select(tg => tg.Group));

        TypeAdapterConfig<Inquiry, InquiryDetailsDto>.NewConfig()
            .Map(dest => dest.Recipients, src => src.InquiryRecipients.Select(ir => ir.Trader))
            .Map(dest => dest.RecipientCount, src => src.InquiryRecipients.Count)
            .Map(dest => dest.OfferCount, src => src.Offers.Count);

        TypeAdapterConfig<Inquiry, InquiryWithCountsDto>.NewConfig()
            .Map(dest => dest.RecipientCount, src => src.InquiryRecipients.Count)
            .Map(dest => dest.OfferCount, src => src.Offers.Count);

        TypeAdapterConfig<Offer, OfferDetailsDto>.NewConfig()
            .Map(dest => dest.Products, src => src.OfferProducts);

        TypeAdapterConfig<Order, OrderDetailsDto>.NewConfig()
            .Map(dest => dest.Products, src => src.Products.Select(p => p.OfferProduct));
    }
}