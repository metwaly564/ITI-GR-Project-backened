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

        
            CreateMap<Banner, BannerDTO>();
            CreateMap<CreateBannerDTO, Banner>();
            CreateMap<UpdateBannerDTO, Banner>();

  
            CreateMap<UserNotification, NotificationDTO>();
        }
    }
}