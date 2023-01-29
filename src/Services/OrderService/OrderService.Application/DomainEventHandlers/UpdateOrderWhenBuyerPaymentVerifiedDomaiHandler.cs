using MediatR;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.DomainEventHandlers
{
    internal class UpdateOrderWhenBuyerPaymentVerifiedDomaiHandler : INotificationHandler<BuyerAndPaymentMethodVerifiedDomainEvent>
    {
        IOrderRepository orderRepository;
        
        public UpdateOrderWhenBuyerPaymentVerifiedDomaiHandler(IOrderRepository _orderRepo)
        {
            orderRepository = _orderRepo ?? throw new   ArgumentNullException(nameof(orderRepository));
        }
        public async Task Handle(BuyerAndPaymentMethodVerifiedDomainEvent notification, CancellationToken cancellationToken)
        {
            var orderToUpdate = await orderRepository.GetByIdAsync(notification.OrderId);
            orderToUpdate.SetBuyerId(notification.Buyer.Id);
            orderToUpdate.SetPaymentMethodId(notification.PaymentMethod.Id);
        }
    }
}
