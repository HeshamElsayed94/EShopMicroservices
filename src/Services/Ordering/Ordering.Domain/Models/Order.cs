namespace Ordering.Domain.Models;

public sealed class Order : Aggregate<OrderId>
{
	private readonly List<OrderItem> _orderItems = [];

	private Order()
	{

	}

	//[JsonConstructor]
	private Order(OrderId id, List<OrderItem> orderItems, CustomerId customerId, OrderName orderName, Address shippingAddress,
		Address billingAddress, Payment payment, OrderStatus status, decimal totalPrice)
	{
		Id = id;
		_orderItems = orderItems;
		CustomerId = customerId;
		OrderName = orderName;
		ShippingAddress = shippingAddress;
		BillingAddress = billingAddress;
		Payment = payment;
		Status = status;
		TotalPrice = totalPrice;
	}

	private Order(OrderId id, CustomerId customerId, OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment)
	{
		Id = id;
		CustomerId = customerId;
		OrderName = orderName;
		ShippingAddress = shippingAddress;
		BillingAddress = billingAddress;
		Payment = payment;
		Status = OrderStatus.Pending;
	}

	public IReadOnlyList<OrderItem> OrderItems => _orderItems;

	public CustomerId CustomerId { get; private set; } = null!;

	public OrderName OrderName { get; private set; } = null!;

	public Address ShippingAddress { get; private set; } = null!;

	public Address BillingAddress { get; private set; } = null!;

	public Payment Payment { get; private set; } = null!;

	public OrderStatus Status { get; private set; } = OrderStatus.Pending;

	public decimal TotalPrice
	{
		get => OrderItems.Sum(x => x.Price * x.Quantity);
		private set { }
	}

	public static Result<Order> Create(OrderId id, CustomerId customerId, OrderName orderName, Address shippingAddress,
		Address billingAddress, Payment payment)
	{
		ArgumentNullException.ThrowIfNull(id);
		ArgumentNullException.ThrowIfNull(customerId);
		ArgumentNullException.ThrowIfNull(orderName);
		ArgumentNullException.ThrowIfNull(shippingAddress);
		ArgumentNullException.ThrowIfNull(billingAddress);
		ArgumentNullException.ThrowIfNull(payment);

		var order = new Order(id, customerId, orderName, shippingAddress, billingAddress, payment);

		order.AddDomainEvent(new OrderCreatedEvent(order));

		return order;

	}

	public Result<Success> Update(OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment, OrderStatus status)
	{
		ArgumentNullException.ThrowIfNull(orderName);
		ArgumentNullException.ThrowIfNull(shippingAddress);
		ArgumentNullException.ThrowIfNull(billingAddress);
		ArgumentNullException.ThrowIfNull(payment);

		if (!Enum.IsDefined(status))
			return Error.Validation("Order.Status", "Invalid status");

		OrderName = orderName;
		ShippingAddress = shippingAddress;
		BillingAddress = billingAddress;
		Payment = payment;
		Status = status;

		AddDomainEvent(new OrderUpdatedEvent(this));

		return Result.Success;
	}

	public void Add(ProductId productId, int quantity, decimal price)
	{
		ArgumentNullException.ThrowIfNull(productId);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

		var orderItem = new OrderItem(Id, productId, quantity, price);

		_orderItems.Add(orderItem);
	}

	public void Remove(ProductId productId)
	{

		ArgumentNullException.ThrowIfNull(productId);

		var orderItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);

		if (orderItem is not null)
			_orderItems.Remove(orderItem);
	}
}