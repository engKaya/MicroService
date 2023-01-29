using EventBus.Base.Abstraction;
using MediatR;
using Microsoft.Azure.Amqp.Framing;
using OrderService.Application.IntegrationEvents;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.AggregateModels.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Address = OrderService.Domain.AggregateModels.OrderAggregate.Address;

namespace OrderService.Application.Features.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        IOrderRepository orderRepository;
        IEventBus eventBus;
        public CreateOrderCommandHandler(IOrderRepository _repo, IEventBus _eventBus)
        {
            orderRepository = _repo;
            eventBus = _eventBus;
        }

        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var addr = new Address(request.Street, request.City, request.State, request.Country, request.ZipCode);

            Order dbOrder = new(request.UserName, addr, request.CardTypeId, request.CardNumber, request.CardSecurityNumber, request.CardHolderName, request.CardExpiration, null);

            request.OrderItems.ToList().ForEach(x => dbOrder.AddOrderItem(x.Id, x.ProductName, x.UnitPrice, x.PictureUrl, x.Units));

            await orderRepository.AddAsync(dbOrder);
            await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(request.UserName);

            eventBus.Publish(orderStartedIntegrationEvent);
            return true;
        }
    }
}
