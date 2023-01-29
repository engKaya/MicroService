using MediatR;
using OrderService.Application.Features.Queries.ViewModels;
using System;

namespace OrderService.Application.Features.Queries.GetOrderDetailById
{
    public class GetOrderDetailsQuery : IRequest<OrderDetailViewModel>
    {
        public Guid Id { get; set; }
        public GetOrderDetailsQuery(Guid id)
        {
            Id = id;
        }
    }
}
