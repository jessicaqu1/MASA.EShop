﻿using MASA.Contrib.Dispatcher.IntegrationEvents.Dapr;
using MASA.EShop.Contracts.Basket.Model;

namespace MASA.EShop.Contracts.Basket
{
    public class UserCheckoutAcceptedIntegrationEvent:IntegrationEvent
    {
        public override string Topic { get; set; } = nameof(UserCheckoutAcceptedIntegrationEvent);

        public string UserId { get; set; }

        public string UserName { get; set; }

        public int OrderNumber { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public string CardNumber { get; set; }

        public string CardHolderName { get; set; }

        public DateTime CardExpiration { get; set; }

        public string CardSecurityNumber { get; set; }

        public int CardTypeId { get; set; }

        public string Buyer { get; set; }

        public Guid RequestId { get; set; }

        public CustomerBasket Basket { get; }

        public UserCheckoutAcceptedIntegrationEvent(
            string userId,
            string userName,
            string city,
            string street,
            string state,
            string country,
            string zipCode,
            string cardNumber,
            string cardHolderName,
            DateTime cardExpiration,
            string cardSecurityNumber,
            int cardTypeId,
            string buyer,
            Guid requestId,
            CustomerBasket basket)
        {
            UserId = userId;
            UserName = userName;
            City = city;
            Street = street;
            State = state;
            Country = country;
            ZipCode = zipCode;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            CardExpiration = cardExpiration;
            CardSecurityNumber = cardSecurityNumber;
            CardTypeId = cardTypeId;
            Buyer = buyer;
            Basket = basket;
            RequestId = requestId;
        }
    }
}
