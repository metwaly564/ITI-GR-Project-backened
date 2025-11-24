using AutoMapper;
using ITI.Pharmacy.Core.DTOs;
using ITI.Pharmacy.Core.Entities;
using ITI.Pharmacy.Core.Interfaces.Services;
using ITI.Pharmacy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.Infrastructure.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public WishlistService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<WishlistDTO> GetUserWishlistAsync(string userId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.Products)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
            {
                // Create wishlist if it doesn't exist
                wishlist = new Wishlist { UserId = userId };
                _context.Wishlists.Add(wishlist);
                await _context.SaveChangesAsync();
            }

            return new WishlistDTO
            {
                Id = wishlist.Id,
                UserId = wishlist.UserId,
                Items = wishlist.Products.Select(p => new WishlistItemDTO
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    Price = p.DiscountPrice ?? p.Price,
                    ImageUrl = p.ImageUrl,
                    AddedAt = DateTime.UtcNow // You might want to store this in a join table
                }).ToList()
            };
        }

        public async Task<bool> AddToWishlistAsync(string userId, int productId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.Products)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
            {
                wishlist = new Wishlist { UserId = userId };
                _context.Wishlists.Add(wishlist);
                await _context.SaveChangesAsync();
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null || !product.IsActive) return false;

            if (!wishlist.Products.Any(p => p.Id == productId))
            {
                wishlist.Products.Add(product);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> RemoveFromWishlistAsync(string userId, int productId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.Products)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null) return false;

            var product = wishlist.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null) return false;

            wishlist.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearWishlistAsync(string userId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.Products)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null) return false;

            wishlist.Products.Clear();
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
