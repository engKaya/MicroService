using OrderService.Domain.SeedWork;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Domain.AggregateModels.OrderAggregate
{
    public class OrderItem : BaseEntity, IValidatableObject
    {
        private int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public int Units { get; set; } 
        protected OrderItem()
        {
            
        }

        public OrderItem(int productId, string productName, decimal unitPrice, string pictureUrl, int units = 1)
        {
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            PictureUrl = pictureUrl;
            Units = units;
        }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (Units <= 0)
                results.Add(new ValidationResult("Invalid Number Of Units", new[] {"Units"}));

            return results;
            
        }
    }
}
