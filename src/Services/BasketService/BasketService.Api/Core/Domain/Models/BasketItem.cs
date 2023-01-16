using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BasketService.Api.Core.Domain.Models
{
    public class BasketItem : IValidatableObject
    {
        public string Id{ get; set; }
        public int ProductId{ get; set; }
        public string ProductName{ get; set; }
        public decimal UnitPrice{ get; set; }
        public decimal OldUnitPrice{ get; set; }
        public int Quantity{ get; set; }
        public string PictureUrl{ get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Quantity <= 0)
            {
                yield return new ValidationResult("Invalid quantity.", new[] { nameof(Quantity) });
            }
        }
    }
}
