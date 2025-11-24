using AutoMapper;
using ITI.Pharmacy.Core.DTOs;
using ITI.Pharmacy.Core.Entities;

namespace ITI.Pharmacy.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<User, UserDTO>();

            // PharmacyBranch
            CreateMap<PharmacyBranch, PharmacyBranchDTO>();
            CreateMap<CreatePharmacyBranchDTO, PharmacyBranch>();

            // Product
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<CreateProductDTO, Product>();
            CreateMap<UpdateProductDTO, Product>();

            // Category mappings
            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count(p => p.IsActive)));

            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<UpdateCategoryDTO, Category>();

        
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name));

            // Banner
            CreateMap<Banner, BannerDTO>();
            CreateMap<CreateBannerDTO, Banner>();
            CreateMap<UpdateBannerDTO, Banner>();

            // Notifications
            CreateMap<UserNotification, NotificationDTO>();
        }
    }
}
