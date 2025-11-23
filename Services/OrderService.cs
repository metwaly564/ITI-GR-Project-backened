using AutoMapper;
using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public OrderService(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment environment)
        {
            _context = context;
            _mapper = mapper;
            _environment = environment;
        }

        public async Task<OrderDTO> CreateOrderAsync(string userId, CreateOrderDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                decimal totalAmount = 0;
                var orderItems = new List<OrderItem>();

                foreach (var item in dto.OrderItems)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product == null || !product.IsActive)
                        throw new Exception($"Product with ID {item.ProductId} not found or inactive");

                    var stock = await _context.ProductStocks
                        .FirstOrDefaultAsync(ps => ps.ProductId == item.ProductId && ps.BranchId == dto.BranchId);

                    if (stock == null || stock.Quantity < item.Quantity)
                        throw new Exception($"Insufficient stock for product: {product.Name}");

                    var orderItem = new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product.DiscountPrice ?? product.Price,
                        TotalPrice = (product.DiscountPrice ?? product.Price) * item.Quantity
                    };

                    orderItems.Add(orderItem);
                    totalAmount += orderItem.TotalPrice;
                    stock.Quantity -= item.Quantity;
                }

                string prescriptionImageUrl = null;
                if (dto.PrescriptionImage != null)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "prescriptions");
                    prescriptionImageUrl = await FileHelper.SaveFileAsync(dto.PrescriptionImage, uploadsFolder, new[] { ".jpg", ".jpeg", ".png", ".pdf" });
                }

                var order = new Order
                {
                    UserId = userId,
                    BranchId = dto.BranchId.Value,
                    TotalAmount = totalAmount,
                    DeliveryFee = 10,
                    Status = "Pending",
                    PaymentStatus = dto.PaymentMethod == "Cash" ? "Pending" : "Paid",
                    PaymentMethod = dto.PaymentMethod,
                    ShippingAddress = dto.ShippingAddress,
                    CustomerNotes = dto.CustomerNotes,
                    PrescriptionImageUrl = prescriptionImageUrl,
                    OrderItems = orderItems
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetOrderByIdAsync(order.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<OrderDTO>> GetUserOrdersAsync(string userId)
        {
            var orders = await _context.Orders
                .Include(o => o.Branch)
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return orders.Select(o => MapToOrderDTO(o)).ToList();
        }

        public async Task<OrderDTO> GetOrderByIdAsync(int id, string userId = null)
        {
            var query = _context.Orders
                .Include(o => o.Branch)
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.Id == id);

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(o => o.UserId == userId);
            }

            var order = await query.FirstOrDefaultAsync();
            if (order == null) return null;

            return MapToOrderDTO(order);
        }

        public async Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDTO dto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            order.Status = dto.Status;
            order.AdminNotes = dto.AdminNotes;
            order.UpdatedAt = DateTime.UtcNow;

            if (dto.Status == "Delivered")
                order.DeliveredAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Branch)
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return orders.Select(o => MapToOrderDTO(o)).ToList();
        }

        public async Task<List<OrderDTO>> GetOrdersByBranchAsync(int branchId)
        {
            var orders = await _context.Orders
                .Include(o => o.Branch)
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.BranchId == branchId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return orders.Select(o => MapToOrderDTO(o)).ToList();
        }

        public async Task<bool> CancelOrderAsync(int id, string userId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null || order.Status != "Pending") return false;

            foreach (var item in order.OrderItems)
            {
                var stock = await _context.ProductStocks
                    .FirstOrDefaultAsync(ps => ps.ProductId == item.ProductId && ps.BranchId == order.BranchId);

                if (stock != null)
                    stock.Quantity += item.Quantity;
            }

            order.Status = "Cancelled";
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

   
        private OrderDTO MapToOrderDTO(Order o)
        {
            return new OrderDTO
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                TotalAmount = o.TotalAmount,
                DeliveryFee = o.DeliveryFee,
                Status = o.Status,
                PaymentStatus = o.PaymentStatus,
                PaymentMethod = o.PaymentMethod,
                ShippingAddress = o.ShippingAddress,
                CustomerNotes = o.CustomerNotes,
                PrescriptionImageUrl = o.PrescriptionImageUrl,
                CreatedAt = o.CreatedAt,
                UserName = o.User != null ? $"{o.User.FirstName} {o.User.LastName}" : "",
                BranchName = o.Branch?.Name ?? "",
                OrderItems = o.OrderItems.Select(oi => new OrderItemDTO
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name ?? "",
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.TotalPrice
                }).ToList()
            };
        }
    }
}
