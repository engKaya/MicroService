using AutoMapper;
using OrderService.Application.DTOs.Order;
using OrderService.Application.Features.Commands.CreateOrder;
using OrderService.Application.Features.Queries.ViewModels;
using OrderService.Domain.AggregateModels.OrderAggregate;
using System.Linq;

namespace OrderService.Application.Mapping.OrderMapping
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, CreateOrderCommand>()
                .ReverseMap();

            CreateMap<OrderItem, OrderItemDTO>()
                .ReverseMap();

            CreateMap<Order, OrderDetailViewModel>()
                .ForMember(x => x.City, y => y.MapFrom(y => y.Address.City))
                .ForMember(x => x.Country, y => y.MapFrom(y => y.Address.Country))
                .ForMember(x => x.Street, y => y.MapFrom(y => y.Address.Street))
                .ForMember(x => x.Zipcode, y => y.MapFrom(y => y.Address.ZipCode))
                .ForMember(x => x.OrderDate, y => y.MapFrom(y => y.OrderDate))
                .ForMember(x => x.OrderNumber, y => y.MapFrom(y => y.Id.ToString()))
                .ForMember(x => x.Status, y => y.MapFrom(y => y.OrderStatus))
                .ForMember(x => x.Total, y => y.MapFrom(y => y.OrderItems.Sum(x => x.Units * x.UnitPrice)))
                .ReverseMap();

            CreateMap<OrderItem, Features.Queries.ViewModels.OrderDetailViewModel.OrderItem>();
        }
    }
}
