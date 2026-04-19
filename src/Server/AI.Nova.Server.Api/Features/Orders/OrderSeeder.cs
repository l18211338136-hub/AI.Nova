using AI.Nova.Server.Api.Features.Payments;
using AI.Nova.Server.Api.Features.Products;
using AI.Nova.Server.Api.Infrastructure.Data.Seed;

namespace AI.Nova.Server.Api.Features.Orders;

#pragma warning disable NonAsyncEFCoreMethodsUsageAnalyzer

public class OrderSeeder : IDataSeeder
{
    private static readonly string[] ReviewComments =
    [
        "非常好的商品，物流也很快！", "质量出乎意料的好，推荐！", "帮朋友买的，他说挺好用的。", "包装非常专业，没有破损。", "实物比图片还要好看。",
        "性价比极高，这个价位无敌了。", "客服态度非常好，耐心地解答了所有问题。", "发货速度非常快，第二天就收到了。", "做工精细，手感非常棒。", "已经是第二次购买了，一如既往的好。",
        "朋友推荐买的，果然没有让我失望。", "颜色很正，跟我想要的完全一样。", "安装很简单，按照说明书几分钟就搞定了。", "尺寸大小非常合适，做工也很精美。", "虽然中间出了点小插曲，但客服很快就解决了。",
        "非常满意，完全超出了我的预期。", "整体感觉不错，物有所值。", "送给老公的礼物，他很喜欢。", "包装很细心，一看就是用心做的。", "物流给力，下单没多久就到了。",
        "用了几天才来评价，真的很好用。", "质感很好，以后还会回购的。", "正品保障，用着放心。", "面料很舒服，款式也时尚。", "细节处理得很好，没有多余的线头。",
        "很有档次感，送礼也拿得出手。", "操作简单，功能很强大。", "实物和描述的一模一样，赞一个。", "卖家的服务态度一流，必须好评。", "省时省力，生活好帮手。",
        "设计非常人性化，考虑得很周到。", "虽然有点小贵，但真的值得拥有。", "收到了，很满意，五星好评。", "东西挺好的，就是物流稍微慢了点。", "很精致，是我喜欢的风格。",
        "帮公司采购的，领导同事都说好。", "效果非常明显，超赞！", "期待了很久，终于收到了，很棒。", "在这个价位能买到这么好的东西，知足了。", "很有耐心，讲解得很详细。",
        "服务周到，物流迅速，产品优良。", "一分钱一分货，品质确实不一样。", "买了绝对不后悔系列。", "简单大方，非常实用。", "同事看了都问我要链接。",
        "非常专业，包装得很稳固。", "细节决定成败，这家店做到了。", "老客户了，每次都很满意。", "挺不错的，下次还会来买。", "给老爸买的，他很高兴。",
        "款式新颖，走在时尚前端。", "非常有质感，一看就很高档。", "用了很久了，质量还是那么稳。", "非常方便，节省了很多空间。", "颜值控闭眼入，真的好看。",
        "客服小妹声音真好听，讲解也专业。", "物流包装很严实，怕磕碰的朋友放心购买。", "整体很满意，推荐给身边的朋友了。", "发货快，物流快，各方面都很快。", "价格挺实在的，没有套路。",
        "很有创意的一款产品，值得点赞。", "手感细腻，非常舒服。", "对比了几家，最后选了这家，没选错。", "包装简洁大方，很喜欢。", "收到货后第一时间就拆开了，非常惊喜。",
        "东西很好，比实体店便宜多了。", "非常有分量感，货真价实。", "售后服务非常有保障，点个赞。", "买的时候还担心，收到货完全放心了。", "非常实用的一次购物经历。",
        "大品牌，值得信赖。", "每一个细节都处理得非常到位。", "不仅外观好看，功能也很好。", "客服回复很及时，服务态度很好。", "真心觉得不错，推荐购买。",
        "挺实用的，已经推荐给几个朋友了。", "做工很扎实，应该能用很久。", "颜色款式都很好，很喜欢。", "产品包装很好，没有受潮。", "送的小礼物也很实用，谢谢卖家。",
        "挺好的，和图片上看到的没有差别。", "发货挺及时的，物流也很顺畅。", "服务质量没得说，真的是太棒了。", "很满意的一次网购。", "性价比非常高，推荐！",
        "宝贝收到了，质量非常棒，超出预期！", "发货特别快，包装也严实，赞！", "客服态度超级好，耐心解答每一个问题。", "东西用着很顺手，以后还会光顾。", "实物比照片还漂亮，颜色超正！",
        "性价比之王，买到就是赚到。", "很有设计感，拿在手里很有分量。", "非常愉快的一次购物，满意推荐。", "包装很用心，物流也给力，好评！", "老顾客了，一直信赖这家的品质。",
        "真的是一分钱一分货，质量没法说。", "实物跟图片一样漂亮，没有色差。", "客服很贴心，还送了小礼物，真好。", "发货速度惊人，还没反应过来就到了。", "强烈推荐，买它买它买它！"
    ];

    public async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        // 1. 检查是否已有数据
        if (await dbContext.Orders.AsNoTracking().AnyAsync(cancellationToken))
        {
            return;
        }

        // 2. 预加载基础数据到内存
        var users = await dbContext.Users.AsNoTracking().Select(u => u.Id).ToListAsync(cancellationToken);
        var addresses = await dbContext.Addresses.AsNoTracking().Select(a => new { a.Id, a.UserId }).ToListAsync(cancellationToken);
        var products = await dbContext.Products.AsNoTracking().Select(p => new { p.Id, p.Name, p.Price }).ToListAsync(cancellationToken);

        if (users.Count == 0 || products.Count == 0) return;

        var totalOrders = 1000000;
        var batchSize = 100000; // 每100000条提交一次
        var adminUserId = Guid.Parse("8ff71671-a1d6-4f97-abb9-d87d7b47d6e7");
        var random = new Random();

        // 3. 定义用于暂存当前批次数据的列表
        var ordersBatch = new List<Order>(batchSize);
        var orderItemsBatch = new List<OrderItem>(batchSize);
        var paymentsBatch = new List<Payment>(batchSize);
        var reviewsBatch = new List<ProductReview>(batchSize);

        for (int k = 0; k < totalOrders; k++)
        {
            var orderId = Guid.NewGuid();
            var userId = users[k % users.Count];
            // 查找对应用户的地址，如果没有则随机取一个
            var addressId = addresses.FirstOrDefault(x => x.UserId == userId)?.Id
                            ?? addresses[k % addresses.Count].Id;

            var product = products[k % products.Count];

            var orderNo = $"ORD{DateTime.UtcNow:yyyyMMdd}{k:D7}";
            var status = (short)(k % 6);
            var createdOn = DateTimeOffset.UtcNow.AddMinutes(-(totalOrders - k));

            var quantity = (k % 3) + 1;
            var totalAmount = (product.Price ?? 0) * quantity;
            var shippingFee = 10.0m;
            var payableAmount = totalAmount + shippingFee;

            // --- 构建订单实体 ---
            var order = new Order
            {
                Id = orderId,
                OrderNo = orderNo,
                UserId = userId,
                TotalAmount = totalAmount,
                DiscountAmount = 0,
                ShippingFee = shippingFee,
                PayableAmount = payableAmount,
                Status = status,
                AddressId = addressId,
                CreatedOn = createdOn,
                PaidOn = createdOn,
                CreatedBy = userId,
            };
            ordersBatch.Add(order);

            // --- 构建订单明细实体 ---
            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                ProductId = product.Id,
                ProductName = product.Name,
                UnitPrice = product.Price ?? 0,
                Quantity = quantity,
                SubTotal = totalAmount,
                PrimaryImageAltText = product.Name,
                CreatedOn = createdOn,
                CreatedBy = userId,
            };
            orderItemsBatch.Add(orderItem);

            // --- 构建支付实体 ---
            if (status >= 1 && status <= 3)
            {
                var paymentMethods = new[] { "Alipay", "WeChat Pay", "UnionPay", "Bank Card", "Digital RMB" };
                var paymentMethod = paymentMethods[k % paymentMethods.Length];

                var payment = new Payment
                {
                    Id = Guid.NewGuid(),
                    OrderId = orderId,
                    UserId = userId,
                    Amount = payableAmount,
                    PaymentMethod = paymentMethod,
                    TransactionId = $"TRX{Guid.NewGuid():N}".ToUpper().Substring(0, 20),
                    Status = 1,
                    PaidOn = createdOn,
                    CreatedOn = createdOn,
                    CreatedBy = userId,
                };
                paymentsBatch.Add(payment);
            }

            // --- 构建评论实体 ---
            if (status == 3 && k % 10 == 0)
            {
                var randomDelay = TimeSpan.FromDays(random.NextDouble() * 7);
                var randomRating = random.Next(1, 6);
                var comment = ReviewComments[random.Next(ReviewComments.Length)];
                var review = new ProductReview
                {
                    Id = Guid.NewGuid(),
                    OrderId = orderId,
                    ProductId = product.Id,
                    UserId = userId,
                    Rating = (short?)randomRating,
                    Comment = comment,
                    IsAnonymous = false,
                    CreatedOn = createdOn + randomDelay,
                    CreatedBy = userId,
                };
                reviewsBatch.Add(review);
            }

            // 4. 当达到批次大小时，统一写入数据库
            if ((k + 1) % batchSize == 0)
            {
                await SaveBatchAsync(dbContext, ordersBatch, orderItemsBatch, paymentsBatch, reviewsBatch, cancellationToken);

                // 清空列表释放内存
                ordersBatch.Clear();
                orderItemsBatch.Clear();
                paymentsBatch.Clear();
                reviewsBatch.Clear();

                // 清理 EF Core 的变更追踪器，防止内存溢出
                dbContext.ChangeTracker.Clear();
            }
        }

        // 5. 处理最后剩余不足一个批次的数据
        if (ordersBatch.Any())
        {
            await SaveBatchAsync(dbContext, ordersBatch, orderItemsBatch, paymentsBatch, reviewsBatch, cancellationToken);
        }
    }

    // 辅助方法：执行批量保存
    private async Task SaveBatchAsync(
        AppDbContext context,
        List<Order> orders,
        List<OrderItem> orderItems,
        List<Payment> payments,
        List<ProductReview> reviews,
        CancellationToken cancellationToken)
    {
        // 使用 AddRange 批量添加，EF Core 会生成更高效的 SQL
        if (orders.Any()) await context.Orders.AddRangeAsync(orders, cancellationToken);
        if (orderItems.Any()) await context.OrderItems.AddRangeAsync(orderItems, cancellationToken);
        if (payments.Any()) await context.Payments.AddRangeAsync(payments, cancellationToken);
        if (reviews.Any()) await context.ProductReviews.AddRangeAsync(reviews, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }
}
