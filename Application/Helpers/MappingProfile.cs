using Application.DTOS;
using AutoMapper;
using Core.Entities;

namespace Application.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {


            CreateMap<Customer, CustomerRegisterDTO>();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Customer, CustomerAccountInfoDTO>().ReverseMap();

            CreateMap<BeautyCenter, BeautyCenterDTO>()
                .ForMember(dest => dest.BeautyCenterId, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.ImagesBeautyCenter.Select(p => p.ImageUrl).ToList()))
                .ForMember(dest => dest.Services, opt => opt.MapFrom(src => src.ServicesForBeautyCenter
                    .Select(s => new ServiceForBeautyCenterDTO
                    {
                        ID = s.Id,
                        BeautyCenterId = s.BeautyCenterId,
                        Name = s.Name,
                        Description = s.Description,
                        Price = (decimal)s.Price,
                        Appointment = s.Appointment
                    })))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews))
                .ReverseMap();

            CreateMap<BeautyCenter, AddBeautyCenterDTO>()
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.ImagesBeautyCenter.Select(p => p.ImageUrl).ToList()))
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                //.ForMember(dest => dest.Services, opt => opt.MapFrom(src => new ServiceForBeautyCenterDTO
                //{
                //    Name = src.ServicesForBeautyCenter.FirstOrDefault().Name,
                //    Description = src.ServicesForBeautyCenter.FirstOrDefault().Description,
                //    Price = src.ServicesForBeautyCenter.FirstOrDefault().Price ?? 0,
                //    Appointment = src.ServicesForBeautyCenter.FirstOrDefault().Appointment ?? DateTime.MinValue
                //}))
                .ReverseMap();

            CreateMap<ServiceForBeautyCenter, ServiceForBeautyCenterDTO>().ReverseMap();


            CreateMap<ShopDressesDTo, ShopDresses>()
           .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ShopDressesID))
           .ForMember(dest => dest.Dresses, opt => opt.MapFrom(src => src.Dresses));

            CreateMap<ShopDresses, ShopDressesDTo>()
                .ForMember(dest => dest.ShopDressesID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Dresses, opt => opt.MapFrom(src => src.Dresses));



            CreateMap<AddShopDressDTO, ShopDresses>()
                .ForMember(dest => dest.Dresses, opt => opt.MapFrom(src => src.Dresses));

            CreateMap<ShopDresses, AddShopDressDTO>()
                .ForMember(dest => dest.Dresses, opt => opt.MapFrom(src => src.Dresses));



            // Map DressDto to Dress and vice versa
            CreateMap<DressDto, Dress>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrls))
                .ForMember(dest => dest.Shop, opt => opt.Ignore()); // Ignoring Shop to avoid circular reference
            CreateMap<Dress, DressDto>()
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.Images, opt => opt.Ignore());



            CreateMap<AddPhotographyDTO, Photography>()
          .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.PictureUrls)).ReverseMap();

            CreateMap<Photography, AddPhotographyDTO>()
          .ForMember(dest => dest.PictureUrls, opt => opt.MapFrom(src => src.Images.Select(image => image.ImageURL)))
          .ReverseMap()
          .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.PictureUrls.Select(url => new Portfolio
          { ImageURL = url })));

            CreateMap<string, Portfolio>()
                .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.PhotographerId, opt => opt.Ignore())
                .ForMember(dest => dest.Photographer, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());


            CreateMap<Photography, PhotographyDTO>()
         .ForMember(dest => dest.PictureUrls, opt => opt.MapFrom(src => src.Images.Select(image => image.ImageURL)))
            .ForMember(dest => dest.photographyID, opt => opt.MapFrom(src => src.ID))
            .ReverseMap();


            CreateMap<OwnerRegisterDTO, Owner>();
            CreateMap<CustomerRegisterDTO, Customer>();
            CreateMap<Owner, OwnerDTO>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserDTO>().ReverseMap();


            CreateMap<Governorate, GovernorateDTO>();
            CreateMap<GovernorateDTO, Governorate>();

            CreateMap<City, CityDTO>();
            CreateMap<CityDTO, City>();


            // Map Car to CarDTO, handling the Pictures and PictureUrls properties separately
            CreateMap<Car, CarDTO>()
                .ForMember(dest => dest.CarID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.PictureUrls, opt => opt.MapFrom(src => src.Pictures.Select(p => p.Url).ToList()))
                .ForMember(dest => dest.Pictures, opt => opt.Ignore());


            // Map CarDTO to Car, converting PictureUrls to CarPicture entities
            CreateMap<CarDTO, Car>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.CarID))
                .ForMember(dest => dest.Pictures, opt => opt.MapFrom(src => src.PictureUrls.Select(url => new CarPicture { Url = url }).ToList()));


            /////////////////////////////////////////////////// Add Car ////////////////////////////////////////////////////////

            CreateMap<Car, AddCarDTO>()
               .ForMember(dest => dest.PictureUrls, opt => opt.MapFrom(src => src.Pictures.Select(p => p.Url).ToList()))
               .ForMember(dest => dest.Pictures, opt => opt.Ignore());


            // Map CarDTO to Car, converting PictureUrls to CarPicture entities
            CreateMap<AddCarDTO, Car>()
                .ForMember(dest => dest.Pictures, opt => opt.MapFrom(src => src.PictureUrls.Select(url => new CarPicture { Url = url }).ToList()));

            // Map CarUpdateDTO to Car
            CreateMap<CarUpdateDTO, Car>()
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.Pictures, opt => opt.Ignore());


            /////////////////////////////////////////////// Hall ////////////////////////////////

            // Hall to HallDTO mapping
            CreateMap<Hall, HallDTO>()
                .ForMember(dest => dest.Features, opt => opt.MapFrom(src => src.Features.Select(f => f.Feature).ToList()))
                .ForMember(dest => dest.HallID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.PictureUrls, opt => opt.MapFrom(src => src.Pictures.Select(p => p.Url).ToList()))
                .ForMember(dest => dest.Pictures, opt => opt.Ignore());

            // HallDTO to Hall mapping
            CreateMap<HallDTO, Hall>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.HallID))
                .ForMember(dest => dest.Pictures, opt => opt.MapFrom(src => src.PictureUrls.Select(url => new HallPicture { Url = url }).ToList()))
                .ForMember(dest => dest.Features, opt => opt.Ignore());  // Assuming Features are handled elsewhere

            // Hall to AddHallDTO mapping
            CreateMap<Hall, AddHallDTO>()
                .ForMember(dest => dest.PictureUrls, opt => opt.MapFrom(src => src.Pictures.Select(p => p.Url).ToList()))
                .ForMember(dest => dest.Pictures, opt => opt.Ignore())
                .ForMember(dest => dest.Features, opt => opt.MapFrom(src => src.Features.Select(f => f.Feature).ToList()));

            // AddHallDTO to Hall mapping
            CreateMap<AddHallDTO, Hall>()
                .ForMember(dest => dest.Features, opt => opt.MapFrom(src => src.Features.Select(f => new HallFeature { Feature = f }).ToList()))
                .ForMember(dest => dest.Pictures, opt => opt.MapFrom(src => src.PictureUrls.Select(url => new HallPicture { Url = url }).ToList()));

            ////////// Get Users Info
            CreateMap<Owner, OwnerAccountInfoDTO>();
            ////////// Set Users Info
            CreateMap<OwnerAccountInfoDTO, Owner>()
                    .ForMember(dest => dest.Email, opt => opt.Ignore())
                    .ForMember(dest => dest.SSN, opt => opt.Ignore())
                    .ForMember(dest => dest.FirstName, opt => opt.Condition(src => src.FirstName != null))
                    .ForMember(dest => dest.LastName, opt => opt.Condition(src => src.LastName != null))
                    .ForMember(dest => dest.GovID, opt => opt.Condition(src => src.GovID != null))
                    .ForMember(dest => dest.CityID, opt => opt.Condition(src => src.CityID != null))
                    .ForMember(dest => dest.YourFavirotePerson, opt => opt.Condition(src => src.YourFavirotePerson != null))
                    .ForMember(dest => dest.ProfileImage, opt => opt.Condition(src => src.ProfileImage != null));
        }
    }
}
