using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Extensions;

internal class InitialData
{
    public static IEnumerable<Customer> Customers =>
    new List<Customer>
    {
        Customer.Create(CustomerId.Of(new Guid("58c49479-ec65-4de2-86e7-033c546291aa")).Value, "mehmet", "mehmet@gmail.com"),
        Customer.Create(CustomerId.Of(new Guid("189dc8dc-990f-48e0-a37b-e6f2b60b9d7d")).Value, "john", "john@gmail.com")
    };

    public static IEnumerable<Product> Products =>
        new List<Product>
        {
            Product.Create(ProductId.Of(new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61")).Value, "IPhone X", 500).Value,
            Product.Create(ProductId.Of(new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914")).Value, "Samsung 10", 400).Value,
            Product.Create(ProductId.Of(new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8")).Value, "Huawei Plus", 650).Value,
            Product.Create(ProductId.Of(new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27")).Value, "Xiaomi Mi", 450).Value
        };

    public static IEnumerable<Order> OrdersWithItems
    {
        get
        {
            var address1 = Address.Of("mehmet", "ozkaya", "mehmet@gmail.com", "Bahcelievler No:4", "Turkey", "Istanbul", "38050");
            var address2 = Address.Of("john", "doe", "john@gmail.com", "Broadway No:1", "England", "Nottingham", "08050");

            var payment1 = Payment.Of("mehmet", "5555555555554444", "12/28", "355", 1);
            var payment2 = Payment.Of("john", "8885555555554444", "06/30", "222", 2);

            var order1 = Order.Create(
                            OrderId.Of(Guid.NewGuid()).Value,
                            CustomerId.Of(new Guid("58c49479-ec65-4de2-86e7-033c546291aa")).Value,
                            OrderName.Of("ORD_1").Value,
                            shippingAddress: address1.Value,
                            billingAddress: address1.Value,
                            payment1.Value);
            order1.Value.Add(ProductId.Of(new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61")).Value, 2, 500);
            order1.Value.Add(ProductId.Of(new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914")).Value, 1, 400);

            var order2 = Order.Create(
                            OrderId.Of(Guid.NewGuid()).Value,
                            CustomerId.Of(new Guid("189dc8dc-990f-48e0-a37b-e6f2b60b9d7d")).Value,
                            OrderName.Of("ORD_2").Value,
                            shippingAddress: address2.Value,
                            billingAddress: address2.Value,
                            payment2.Value);
            order2.Value.Add(ProductId.Of(new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8")).Value, 1, 650);
            order2.Value.Add(ProductId.Of(new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27")).Value, 2, 450);

            return new List<Order> { order1.Value, order2.Value };
        }
    }
}