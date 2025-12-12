using AutoMapper;
using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models;

namespace ITI.Pharmacy.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
     
            CreateMap<User, UserDTO>();


            CreateMap<PharmacyBranch, PharmacyBranchDTO>();
            CreateMap<CreatePharmacyBranchDTO, PharmacyBranch>();

     
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src => src.ImageUrl ?? string.Empty))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.DiscountPrice, opt => opt.MapFrom(src => src.DiscountPrice));

            CreateMap<CreateProductDTO, Product>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ProductImage));
            
            // Note: UpdateProductDTO is handled manually in ProductService.UpdateProductAsync
            // This mapping is kept for potential future use but is not currently used
            CreateMap<UpdateProductDTO, Product>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName ?? string.Empty))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ProductImage ?? string.Empty))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.ProductStocks, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            // Category mappings
            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count(p => p.IsActive)));

            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<UpdateCategoryDTO, Category>();

        
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City != null ? src.City.Name : null));

        
            CreateMap<Banner, BannerDTO>();
            CreateMap<CreateBannerDTO, Banner>();
            CreateMap<UpdateBannerDTO, Banner>();

  
            CreateMap<UserNotification, NotificationDTO>();
        }
    }
}