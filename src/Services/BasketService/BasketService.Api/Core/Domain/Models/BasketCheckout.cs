using System;
using System.ComponentModel.DataAnnotations;

namespace BasketService.Api.Core.Domain.Models
{
    public class BasketCheckout
    {
        public string City { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        [CreditCard]
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string CardHolderName { get; set; }
        public DateTime CardExpiration { get; set; }
        [Range(100, 999)]
        [Required]
        public string CardSecurityNumber { get; set; }
        public int CardTypeId { get; set; }
        public string BuyerId { get; set; } 
    }
}
