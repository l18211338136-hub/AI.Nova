using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI.Nova.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddEcommerceEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()", comment: "地址记录唯一标识"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true, comment: "关联的用户 ID"),
                    RecipientName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "收货联系人姓名"),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "收货联系人电话号码"),
                    Province = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "一级行政区划分 (省/自治区/直辖市)"),
                    City = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "二级行政区划分 (城市)"),
                    District = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "三级行政区划分 (区/县)"),
                    StreetAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "详细街道/门牌地址描述"),
                    PostalCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true, comment: "邮政编码"),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: true, comment: "是否设为该用户的默认首选收货地址"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "用户收货地址表：存储用户的收货联系人、电话及多级行政区划详细地址。");

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()", comment: "购物车记录唯一标识"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true, comment: "关联的用户 ID"),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true, comment: "关联的商品 ID"),
                    Quantity = table.Column<int>(type: "integer", nullable: true, comment: "用户预备购买的商品件数"),
                    Selected = table.Column<bool>(type: "boolean", nullable: true, comment: "标记该商品当前是否在结算清单中处于勾选/激活状态"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "购物车明细表：记录用户添加到结算清单的商品、数量及勾选状态。");

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()", comment: "库存记录唯一标识"),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true, comment: "关联的商品 ID"),
                    StockQuantity = table.Column<int>(type: "integer", nullable: true, comment: "当前真实的可用库存余量"),
                    ReservedQuantity = table.Column<int>(type: "integer", nullable: true, comment: "已下单但未支付的冻结/预占库存数量"),
                    LowStockThreshold = table.Column<int>(type: "integer", nullable: true, comment: "触发低库存自动提醒的阈值高度"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "商品库存表：维护商品实时库存、占用库存及库存报警阈值。");

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()", comment: "图片记录唯一标识"),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true, comment: "关联的商品 ID"),
                    ImageUrl = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "图片素材的存储 URL 路径"),
                    AltText = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "图片替代文本 (Alt Text)，用于 SEO 和无障碍显示"),
                    SortOrder = table.Column<int>(type: "integer", nullable: true, comment: "图片在展示列表中的排序权重 (升序排列)"),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: true, comment: "是否将此图片设为商品封面主图"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "商品图片关联表：存储商品的多媒体展示资源，支持多图展示、主图标记及排序展示。");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()", comment: "订单记录唯一标识"),
                    OrderNo = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "全局唯一业务订单编号"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true, comment: "关联的下单用户 ID"),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: true, comment: "商品原始销售总价"),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: true, comment: "优惠抵扣的总金额"),
                    ShippingFee = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: true, comment: "订单物流配送费用"),
                    PayableAmount = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: true, comment: "实付应结的总金额 (含运费并扣除优惠)"),
                    Status = table.Column<short>(type: "smallint", nullable: true, comment: "当前订单所处的业务状态枚举值"),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: true, comment: "关联的配送地址 ID"),
                    Remark = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "用户提供的订单留言或特殊备注说明"),
                    PaidOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "该订单由于支付成功而被确认的时间戳"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "订单主表：电商交易的核心记录，维护订单生命周期状态及金额明细。");

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()", comment: "订单项记录唯一标识"),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: true, comment: "关联的主订单 ID"),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true, comment: "关联的商品 ID"),
                    ProductName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "下单时刻存储的商品名称属性快照"),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: true, comment: "下单时刻存储的成交单价快照"),
                    Quantity = table.Column<int>(type: "integer", nullable: true, comment: "用户购买商品的选择数量"),
                    SubTotal = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: true, comment: "该订单项对应的合计金额小计"),
                    PrimaryImageAltText = table.Column<string>(type: "text", nullable: true, comment: "下单时刻存储的图片替代文本快照"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "订单明细表：记录订单中每一项商品的详细快照及购买数量。");

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()", comment: "支付记录唯一标识"),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: true, comment: "关联的所属订单 ID"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true, comment: "执行该支付流水记录的用户 ID"),
                    Amount = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: true, comment: "本次支付流水的实付金额"),
                    PaymentMethod = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "支付方式/渠道名称 (如：Alipay, WeChatPay)"),
                    TransactionId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "三方支付机构返回的全局唯一交易参考单号"),
                    Status = table.Column<short>(type: "smallint", nullable: true, comment: "该笔支付流水当前的处理状态枚举"),
                    PaidOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "支付状态被标记为成功的时间点"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "支付流水表：记录用户对订单进行的每一笔支付尝试及其最终状态。");

            migrationBuilder.CreateTable(
                name: "ProductReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()", comment: "评价记录唯一标识"),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: true, comment: "关联的订单 ID"),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true, comment: "被评价的商品 ID"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true, comment: "发表评论的用户 ID"),
                    Rating = table.Column<short>(type: "smallint", nullable: true, comment: "商品评分 (取值范围 1-5)"),
                    Comment = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "用户发表的评价正文内容"),
                    IsAnonymous = table.Column<bool>(type: "boolean", nullable: true, comment: "是否启用匿名方式显示评价"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录创建时间"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录创建者ID"),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录最后修改时间"),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录最后修改者ID"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: true, comment: "软删除标记：true表示已删除"),
                    DeletedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "记录删除时间"),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true, comment: "记录删除者ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductReviews_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ProductReviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductReviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "商品评价表");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId_IsDefault",
                table: "Addresses",
                columns: new[] { "UserId", "IsDefault" });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_UserId_ProductId",
                table: "CartItems",
                columns: new[] { "UserId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ProductId",
                table: "Inventories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AddressId",
                table: "Orders",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNo",
                table: "Orders",
                column: "OrderNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_OrderId",
                table: "ProductReviews",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_ProductId",
                table: "ProductReviews",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_UserId",
                table: "ProductReviews",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductReviews");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
