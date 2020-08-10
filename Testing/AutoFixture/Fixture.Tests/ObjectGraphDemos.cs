using System;
using System.Collections.Generic;
using AutoFixture;
using Xunit;

namespace Fixture.Tests
{
    public class ObjectGraphDemos
    {
        [Fact]
        public void ManualCreation()
        {
            Customer customer = new Customer
            {
                CustomerName = "Amrit"
            };

            Order order = new Order(customer)
            {
                Id = 412,
                OrderDate = DateTime.Now,
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductName = "Rubber ducks",
                        Quantity = 2
                    }
                }
            };
        }

        [Fact]
        public void AutoCreation()
        {
            var fixture = new AutoFixture.Fixture();
            Order order = fixture.Create<Order>();

        }
    }
}