using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(ILogger<DiscountService> logger, DiscountContext dbContext)
    : DiscountProtoService.DiscountProtoServiceBase
{

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        logger.LogInformation("{Class}.{Method} called", nameof(DiscountService), nameof(GetDiscount));

        var coupon = await dbContext.Coupons
            .FirstOrDefaultAsync(x => x.ProductName.Equals(request.ProductName));

        coupon ??= new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };

        logger.LogInformation("Discount {Coupon} is retrieved for productName : {productName}",
            coupon,
            request.ProductName);

        var copounModel = coupon.Adapt<CouponModel>();

        return copounModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Adapt<Coupon>()
            ?? throw new RpcException(new(StatusCode.InvalidArgument, "Invalid request object."));

        await dbContext.Coupons.AddAsync(coupon);

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount {Coupon} is successfully created. productName {productName}",
            request,
            request.ProductName);

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>()
           ?? throw new RpcException(new(StatusCode.InvalidArgument, "Invalid request object."));

        dbContext.Coupons.Update(coupon);

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount {Coupon} is successfully updated. productName {productName}",
            request.Coupon,
            request.Coupon.ProductName);

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons
            .FirstOrDefaultAsync(x => x.ProductName.Equals(request.ProductName))
            ?? throw new RpcException(new(StatusCode.NotFound, $"Discount with productName={request.ProductName} is not found"));

        dbContext.Remove(coupon);

        var isDeleted = await dbContext.SaveChangesAsync() > 0;

        logger.LogInformation("Discount {Coupon} is successfully deleted. productName {productName}",
           coupon,
           request.ProductName);

        return new() { Sucess = isDeleted };
    }
}