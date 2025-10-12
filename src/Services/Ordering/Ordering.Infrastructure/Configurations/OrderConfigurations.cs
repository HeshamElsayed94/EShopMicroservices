using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Configurations;

internal class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
            orderId => orderId.Value,
            dbId => OrderId.Of(dbId).Value);

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .IsRequired();

        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId);

        builder.ComplexProperty(o => o.OrderName, nameBuilder =>
        {
            nameBuilder.Property(n => n.Value)
                .HasColumnName(nameof(Order.OrderName))
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.ComplexProperty(o => o.ShippingAddress, nameBuilder =>
        {
            nameBuilder.Property(a => a.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            nameBuilder.Property(a => a.LastName)
                .HasMaxLength(50)
                .IsRequired();

            nameBuilder.Property(a => a.EmailAddress)
                .HasMaxLength(50)
                .IsRequired();

            nameBuilder.Property(a => a.AddressLine)
                .HasMaxLength(180)
                .IsRequired();

            nameBuilder.Property(a => a.Country)
                .HasMaxLength(50)
                .IsRequired();

            nameBuilder.Property(a => a.State)
                .HasMaxLength(50)
                .IsRequired();

            nameBuilder.Property(a => a.ZipCode)
                .HasMaxLength(50)
                .IsRequired();
        });

        builder.ComplexProperty(o => o.BillingAddress, nameBuilder =>
        {
            nameBuilder.Property(a => a.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            nameBuilder.Property(a => a.LastName)
                .HasMaxLength(50)
                .IsRequired();

            nameBuilder.Property(a => a.EmailAddress)
                .HasMaxLength(50)
                .IsRequired();

            nameBuilder.Property(a => a.AddressLine)
                .HasMaxLength(180)
                .IsRequired();

            nameBuilder.Property(a => a.Country)
                .HasMaxLength(50)
                .IsRequired();

            nameBuilder.Property(a => a.State)
                .HasMaxLength(50)
                .IsRequired();

            nameBuilder.Property(a => a.ZipCode)
                .HasMaxLength(50)
                .IsRequired();
        });

        builder.ComplexProperty(o => o.Payment, paymentBuilder =>
        {
            paymentBuilder.Property(p => p.CardName)
            .HasMaxLength(50);

            paymentBuilder.Property(p => p.CardNumber)
            .HasMaxLength(24)
            .IsRequired();

            paymentBuilder.Property(p => p.Expiration)
            .HasMaxLength(10)
            .IsRequired();

            paymentBuilder.Property(propertyExpression: p => p.CVV)
            .HasMaxLength(3)
            .IsRequired();
        });

        builder.Property(o => o.Status)
            .HasDefaultValue(OrderStatus.Draft)
            .HasConversion(
            s => s.ToString(),
            dbStatus => Enum.Parse<OrderStatus>(dbStatus)
            ).HasMaxLength(20);

        builder.Property(o => o.TotalPrice);
    }
}