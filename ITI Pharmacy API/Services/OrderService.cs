using AutoMapper;
using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrderService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderDTO> CreateOrderAsync(string userId, CreateOrderDTO dto)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID is required");

            if (dto.OrderItems == null || dto.OrderItems.Count == 0)
                throw new ArgumentException("Order must contain at least one item");

            // Verify branch exists
            var branch = await _context.PharmacyBranches.FindAsync(dto.BranchId);
            if (branch == null)
                throw new ArgumentException("Invalid branch ID");

            // Verify city exists and is active
            var city = await _context.Cities.FindAsync(dto.CityId);
            if (city == null || !city.IsActive)
                throw new ArgumentException("Invalid city ID or city is not active");

            // Validate shipping address
            if (string.IsNullOrWhiteSpace(dto.ShippingAddress) || dto.ShippingAddress.Trim().Length < 10)
                throw new ArgumentException("Shipping address must be at least 10 characters long");

            // Validate phone number
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber) || dto.PhoneNumber.Trim().Length < 8)
                throw new ArgumentException("Phone number must be at least 8 characters long");

            // Get all products for the order
            var productIds = dto.OrderItems.Select(oi => oi.ProductId).ToList();
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id) && p.IsActive)
                .ToListAsync();

            if (products.Count != productIds.Count)
                throw new ArgumentException("One or more products are invalid or inactive");

            // Create order
            var order = new Order
            {
                UserId = userId,
                BranchId = dto.BranchId,
                OrderNumber = GenerateOrderNumber(),
                ShippingAddress = dto.ShippingAddress,
                PhoneNumber = dto.PhoneNumber,
                CityId = dto.CityId,
                CustomerNotes = dto.CustomerNotes ?? string.Empty,
                AdminNotes = string.Empty, // Initialize AdminNotes to empty string
                PrescriptionImageUrl = dto.PrescriptionImageUrl ?? string.Empty,
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = "Pending",
                Status = "Pending",
                DeliveryFee = 5.00m, // Default delivery fee
                CreatedAt = DateTime.UtcNow
            };

            // Create order items and calculate total
            decimal totalAmount = 0;
            foreach (var itemDto in dto.OrderItems)
            {
                var product = products.First(p => p.Id == itemDto.ProductId);
                var unitPrice = product.DiscountPrice ?? product.Price;
                var totalPrice = unitPrice * itemDto.Quantity;

                var orderItem = new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = unitPrice,
                    TotalPrice = totalPrice,
                    Order = order
                };

                order.OrderItems.Add(orderItem);
                totalAmount += totalPrice;
            }

            order.TotalAmount = totalAmount + order.DeliveryFee;

            // Save order
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Load order with related data for mapping
            var savedOrder = await _context.Orders
                .Include(o => o.Branch)
                .Include(o => o.User)
                .Include(o => o.City)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            return _mapper.Map<OrderDTO>(savedOrder);
        }

        private string GenerateOrderNumber()
        {
            // Generate unique order number: ORD-YYYYMMDD-HHMMSS-XXXX
            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss");
            var random = new Random().Next(1000, 9999);
            return $"ORD-{timestamp}-{random}";
        }

        public async Task<List<OrderDTO>> GetUserOrdersAsync(string userId)
        {
            var orders = await _context.Orders
                .Include(o => o.Branch)
                .Include(o => o.User)
                .Include(o => o.City)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return _mapper.Map<List<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetOrderByIdAsync(int id, string? userId = null)
        {
            var query = _context.Orders
                .Include(o => o.Branch)
                .Include(o => o.User)
                .Include(o => o.City)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .AsQueryable();

            if (userId != null)
            {
                query = query.Where(o => o.UserId == userId);
            }

            var order = await query.FirstOrDefaultAsync(o => o.Id == id);
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<List<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Branch)
                .Include(o => o.User)
                .Include(o => o.City)
                .ToListAsync();

            return _mapper.Map<List<OrderDTO>>(orders);
        }

        public async Task<List<OrderDTO>> GetOrdersByBranchAsync(int branchId)
        {
            var orders = await _context.Orders
                .Include(o => o.Branch)
                .Include(o => o.User)
                .Include(o => o.City)
                .Where(o => o.BranchId == branchId)
                .ToListAsync();

            return _mapper.Map<List<OrderDTO>>(orders);
        }

        public async Task<bool> CancelOrderAsync(int id, string userId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
                return false;

            if (order.Status == "Delivered" || order.Status == "Cancelled")
                return false;

            order.Status = "Cancelled";
            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDTO dto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return false;

            order.Status = dto.Status;
            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
