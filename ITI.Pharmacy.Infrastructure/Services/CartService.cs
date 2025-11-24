using AutoMapper;
using ITI.Pharmacy.Core.DTOs;
using ITI.Pharmacy.Core.Entities;
using ITI.Pharmacy.Core.Interfaces.Services;
using ITI.Pharmacy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.Infrastructure.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CartService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CartDTO> GetUserCartAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                // Create cart if it doesn't exist
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return new CartDTO
            {
                Id = cart.Id,
                UserId = cart.UserId,
                TotalAmount = cart.CartItems.Sum(ci => ci.TotalPrice),
                CartItems = cart.CartItems.Select(ci => new CartItemDTO
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.Name,
                    ProductPrice = ci.UnitPrice,
                    ProductImage = ci.Product.ImageUrl,
                    Quantity = ci.Quantity,
                    TotalPrice = ci.TotalPrice
                }).ToList()
            };
        }

        public async Task<CartDTO> AddToCartAsync(string userId, AddToCartDTO dto)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
            }

            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);
            var product = await _context.Products.FindAsync(dto.ProductId);

            if (product == null || !product.IsActive)
                throw new Exception("Product not found or inactive");

            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
                existingItem.TotalPrice = existingItem.Quantity * existingItem.UnitPrice;
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = product.DiscountPrice ?? product.Price,
                    TotalPrice = (product.DiscountPrice ?? product.Price) * dto.Quantity
                };
                cart.CartItems.Add(cartItem);
            }

            cart.TotalAmount = cart.CartItems.Sum(ci => ci.TotalPrice);
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return await GetUserCartAsync(userId);
        }

        public async Task<CartDTO> UpdateCartItemAsync(string userId, int itemId, UpdateCartItemDTO dto)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) throw new Exception("Cart not found");

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == itemId);
            if (cartItem == null) throw new Exception("Cart item not found");

            if (dto.Quantity <= 0)
            {
                cart.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity = dto.Quantity;
                cartItem.TotalPrice = cartItem.Quantity * cartItem.UnitPrice;
            }

            cart.TotalAmount = cart.CartItems.Sum(ci => ci.TotalPrice);
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return await GetUserCartAsync(userId);
        }

        public async Task<bool> RemoveFromCartAsync(string userId, int itemId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return false;

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == itemId);
            if (cartItem == null) return false;

            cart.CartItems.Remove(cartItem);
            cart.TotalAmount = cart.CartItems.Sum(ci => ci.TotalPrice);
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return false;

            cart.CartItems.Clear();
            cart.TotalAmount = 0;
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
