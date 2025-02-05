using Application.DTOS;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Core.Entities;
using Core.Enums;

namespace Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository AdminRepository;
        private readonly IBeautyRepository _beautyCenterRepository;
        private readonly IHallRepository _hallRepository;
        private readonly ICarRepository _carRepository;
        private readonly IPhotographyRepository _photographyRepository;
        private readonly IShopDressesRepository _shopDressesRepository;

        private readonly IRepository<Service> _serviceRepository;

        private readonly ICustomerRepository _customerRepository;

        private readonly IMapper Mapper;

        public AdminService(IAdminRepository adminRepository, IMapper _mapper, IBeautyRepository beautyCenterRepository,
        IHallRepository hallRepository,
        ICarRepository carRepository,
        IPhotographyRepository photographyRepository,


        IRepository<Service> serviceRepository,


        IShopDressesRepository shopDressesRepository, ICustomerRepository customerRepository)

        {
            _beautyCenterRepository = beautyCenterRepository;
            _hallRepository = hallRepository;
            _carRepository = carRepository;
            _photographyRepository = photographyRepository;
            _shopDressesRepository = shopDressesRepository;
            AdminRepository = adminRepository;
            _serviceRepository = serviceRepository;
            Mapper = _mapper;
            _customerRepository = customerRepository;
        }




        public CustomResponseDTO<AllServicesDTO> GetAllServices(ServiceStatus ServiceStatus, int page, int pageSize)
        {

            var allServices = AdminRepository.GetAllServices(ServiceStatus);

            var orderedServices = allServices.OrderBy(s => s.CreatedAt).ThenBy(s => !s.IsAdminSeen).ToList();


            

            AdminRepository.updateServices(allServices);
            AdminRepository.Save();
            var paginatedList = PaginationHelper.Paginate(orderedServices, page, pageSize);

            var allServicesDTO = new AllServicesDTO
            {
                BeautyCenters = new List<BeautyCenterDTO>(),
                Halls = new List<HallDTO>(),
                Cars = new List<CarDTO>(),
                Photographys = new List<PhotographyDTO>(),
                ShopDresses = new List<ShopDressesDTo>()
            };

            foreach (var Service in paginatedList.Items)
            {
                switch (Service)
                {
                    case BeautyCenter beautyCenter:
                        allServicesDTO.BeautyCenters.Add(Mapper.Map<BeautyCenterDTO>(beautyCenter));
                        break;
                    case Hall hall:
                        allServicesDTO.Halls.Add(Mapper.Map<HallDTO>(hall));
                        break;
                    case Car car:
                        allServicesDTO.Cars.Add(Mapper.Map<CarDTO>(car));
                        break;
                    case Photography photography:
                        allServicesDTO.Photographys.Add(Mapper.Map<PhotographyDTO>(photography));
                        break;
                    case ShopDresses shopDresses:
                        allServicesDTO.ShopDresses.Add(Mapper.Map<ShopDressesDTo>(shopDresses));
                        break;
                }
            }
            var paginationInfo = PaginationHelper.GetPaginationInfo(paginatedList);

            var response = new CustomResponseDTO<AllServicesDTO>
            {
                Data = allServicesDTO,
                Message = "تم",
                Succeeded = true,
                Errors = null,
                PaginationInfo = paginationInfo
            };
            return response;
        }
        public CustomResponseDTO<List<OwnerDTO>> GetAllOwners(int page, int pageSize)
        {
            IQueryable<Owner> allOwners = AdminRepository.GetAllOwners();

            AdminRepository.updateOwners(allOwners.ToList());
            AdminRepository.Save();

            var paginatedList = PaginationHelper.Paginate(allOwners, page, pageSize);

            List<OwnerDTO> owners = Mapper.Map<List<OwnerDTO>>(paginatedList.Items);
            var paginationInfo = PaginationHelper.GetPaginationInfo(paginatedList);

            var response = new CustomResponseDTO<List<OwnerDTO>>
            {
                Data = owners,
                Message = "تم",
                Succeeded = true,
                Errors = null,
                PaginationInfo = paginationInfo
            };

            return response;
        }
        public CustomResponseDTO<List<CustomerDTO>> GetAllCustomers(string? customerName,bool? isBlocked, int page, int pageSize)
        {
            IQueryable<Customer> allCustomers = AdminRepository.GetAllCustomers(customerName,isBlocked);
            var paginatedList = PaginationHelper.Paginate(allCustomers, page, pageSize);

            List<CustomerDTO> customers = Mapper.Map<List<CustomerDTO>>(paginatedList.Items);
            var paginationInfo = PaginationHelper.GetPaginationInfo(paginatedList);

            var response = new CustomResponseDTO<List<CustomerDTO>>
            {
                Data = customers,
                Message = "تم",
                Succeeded = true,
                Errors = null,
                PaginationInfo = paginationInfo
            };

            return response;
        }
        public CustomResponseDTO<object> GetServiceTypeByID(int id)
        {
            Service Service = AdminRepository.GetServiceById(id);

            switch (Service)
            {
                case BeautyCenter beautyCenter:
                    return new CustomResponseDTO<object>()
                    {
                        Data = Mapper.Map<BeautyCenterDTO>(beautyCenter),
                        Message = "بيوتي سنتر",
                        Succeeded = true,
                        Errors = null,
                    };

                    break;
                case Hall hall:
                    return new CustomResponseDTO<object>()
                    {
                        Data = Mapper.Map<HallDTO>(hall),
                        Message = "قاعه",
                        Succeeded = true,
                        Errors = null,
                    };
                    break;
                case Car car:
                    return new CustomResponseDTO<object>()
                    {
                        Data = Mapper.Map<CarDTO>(car),
                        Message = "عربيه",
                        Succeeded = true,
                        Errors = null,
                    };
                    break;
                case Photography photography:
                    return new CustomResponseDTO<object>()
                    {
                        Data = Mapper.Map<PhotographyDTO>(photography),
                        Message = "مصور",
                        Succeeded = true,
                        Errors = null,
                    };
                    break;
                case ShopDresses shopDresses:
                    return new CustomResponseDTO<object>()
                    {
                        Data = Mapper.Map<ShopDressesDTo>(shopDresses),
                        Message = "فساتين",
                        Succeeded = true,
                        Errors = null,
                    };
                    break;
            }
            return new CustomResponseDTO<object>()
            {
                Data = null,
                Message = "حدث خطأ",
                Succeeded = false,
                Errors = null,
            };
        }

        public CustomResponseDTO<OwnerDTO> GetOwnerById(string ownerId)
        {
            Owner owner = AdminRepository.GetOwnerById(ownerId);

            if (owner == null)
            {
                return new CustomResponseDTO<OwnerDTO>
                {
                    Data = null,
                    Message = "تعذر العثور علي المالك",
                    Succeeded = false,
                    Errors = new List<string> { "تعذر العثور علي المالك" }
                };
            }

            OwnerDTO ownerDTO = Mapper.Map<OwnerDTO>(owner);

            return new CustomResponseDTO<OwnerDTO>
            {
                Data = ownerDTO,
                Message = "تم",
                Succeeded = true,
                Errors = null
            };
        }
        public void Delete(int id)
        {
            // any logic
            AdminRepository.Delete(id);
        }


        public CustomResponseDTO<List<ApplicationUserDTO>> SearchUsersByName(string name)
        {
            var users = AdminRepository.SearchUsersByName(name);
            if (users == null || users.Count == 0)
            {
                return new CustomResponseDTO<List<ApplicationUserDTO>>
                {
                    Data = null,
                    Message = "لا يوجد مستخدمين",
                    Succeeded = false,
                    Errors = new List<string> { "لا يوجد مستخدمين" }
                };
            }

            var userDTOs = Mapper.Map<List<ApplicationUserDTO>>(users);
            return new CustomResponseDTO<List<ApplicationUserDTO>>
            {
                Data = userDTOs,
                Message = "Success",
                Succeeded = true,
                Errors = null
            };
        }
        public CustomResponseDTO<List<OwnerDTO>> GetFilteredOwners(string? ownerName, UserType? userType,OwnerAccountStatus? status, bool? isBlocked, int page, int pageSize)
        {
            var filteredOwners = AdminRepository.GetOwnersByStatus(ownerName,userType,status, isBlocked);
            AdminRepository.updateOwners(filteredOwners.ToList());
            AdminRepository.Save();


            var paginatedList = PaginationHelper.Paginate(filteredOwners, page, pageSize);
            if (!paginatedList.Items.Any())
            {
                return new CustomResponseDTO<List<OwnerDTO>>
                {
                    Data = null,
                    Message = "تعذر العثور علي المالك",
                    Succeeded = false,
                    Errors = null
                };
            }

            var paginationInfo = PaginationHelper.GetPaginationInfo(paginatedList);

            List<OwnerDTO> owners = Mapper.Map<List<OwnerDTO>>(paginatedList.Items);
            var response = new CustomResponseDTO<List<OwnerDTO>>
            {
                Data = owners,
                Message = "تم",
                Succeeded = true,
                Errors = null,
                PaginationInfo = paginationInfo
            };

            return response;
        }

        public Owner GetById(string id)
        {
            return AdminRepository.GetById(id);
        }

        public void Update(Owner obj)
        {
            AdminRepository.Update(obj);
        }

        public void Save()
        {
            AdminRepository.Save();
        }

        public CustomResponseDTO<object> BlockOwner(string ownerId)
        {
            Owner owner = AdminRepository.GetById(ownerId);

            if (owner == null)
            {
                return new CustomResponseDTO<object>
                {
                    Data = null,
                    Message = "المالك غير موجود",
                    Succeeded = false,
                    Errors = new List<string> { "المالك غير موجود" }
                };
            }

            if (owner.IsBlocked)
            {
                return new CustomResponseDTO<object>
                {
                    Data = owner.UserName,
                    Message = "المالك محظور بالفعل",
                    Succeeded = false,
                    Errors = null
                };
            }

            owner.IsBlocked = true;
            AdminRepository.Update(owner);
            AdminRepository.Save();

            return new CustomResponseDTO<object>
            {
                Data = owner.UserName,
                Message = "تم حظر المالك بنجاح",
                Succeeded = true,
                Errors = null
            };
        }


        public CustomResponseDTO<object> UnblockOwner(string ownerId)
        {
            Owner owner = AdminRepository.GetById(ownerId);

            if (owner == null)
            {
                return new CustomResponseDTO<object>
                {
                    Data = null,
                    Message = "المالك غير موجود",
                    Succeeded = false,
                    Errors = new List<string> { "المالك غير موجود" }
                };
            }

            if (!owner.IsBlocked)
            {
                return new CustomResponseDTO<object>
                {
                    Data = owner.UserName,
                    Message = "المالك غير محظور بالفعل",
                    Succeeded = false,
                    Errors = null
                };
            }

            owner.IsBlocked = false;
            AdminRepository.Update(owner);
            AdminRepository.Save();

            return new CustomResponseDTO<object>
            {
                Data = owner.UserName,
                Message = "تم رفع الحظر عن المالك بنجاح",
                Succeeded = true,
                Errors = null
            };
        }

        public CustomResponseDTO<object> AcceptOwner(string ownerId)
        {
            Owner owner = AdminRepository.GetById(ownerId);

            if (owner == null)
            {
                return new CustomResponseDTO<object>
                {
                    Data = null,
                    Message = "تعذر العثور علي المالك",
                    Succeeded = false,
                    Errors = new List<string> { "تعذر العثور علي المالك" }
                };
            }

            if (owner.AccountStatus == OwnerAccountStatus.Accepted)
            {
                return new CustomResponseDTO<object>
                {
                    Data = owner.AccountStatus,
                    Message = "المالك تم قبوله مسبقا",
                    Succeeded = false,
                    Errors = null
                };
            }

            owner.AccountStatus = OwnerAccountStatus.Accepted;
            AdminRepository.Update(owner);
            AdminRepository.Save();

            return new CustomResponseDTO<object>
            {
                Data = owner.AccountStatus,
                Message = "تم قبول المالك",
                Succeeded = true,
                Errors = null
            };
        }

        public CustomResponseDTO<object> DeclineOwner(string ownerId)
        {
            Owner owner = AdminRepository.GetById(ownerId);

            if (owner == null)
            {
                return new CustomResponseDTO<object>
                {
                    Data = null,
                    Message = "تعذر العثور علي المالك",
                    Succeeded = false,
                    Errors = new List<string> { "تعذر العثور علي المالك" }
                };
            }

            if (owner.AccountStatus == OwnerAccountStatus.Decline)
            {
                return new CustomResponseDTO<object>
                {
                    Data = owner.AccountStatus,
                    Message = "المالك تم رفضه مسبقا",
                    Succeeded = false,
                    Errors = null
                };
            }

            owner.AccountStatus = OwnerAccountStatus.Decline;
            AdminRepository.Update(owner);
            AdminRepository.Save();

            return new CustomResponseDTO<object>
            {
                Data = owner.AccountStatus,
                Message = "تم رفض المالك",
                Succeeded = true,
                Errors = null
            };
        }


        public CustomResponseDTO<object> BlockCustomer(string customerId)
        {
            Customer customer = _customerRepository.GetCustomerById(customerId);

            if (customer == null)
            {
                return new CustomResponseDTO<object>
                {
                    Data = null,
                    Message = "العميل غير موجود",
                    Succeeded = false,
                    Errors = new List<string> { "العميل غير موجود" }
                };
            }

            if (customer.IsBlocked)
            {
                return new CustomResponseDTO<object>
                {
                    Data = customer.UserName,
                    Message = "العميل محظور بالفعل",
                    Succeeded = false,
                    Errors = null
                };
            }

            customer.IsBlocked = true;
            _customerRepository.Update(customer);
            _customerRepository.Save();

            return new CustomResponseDTO<object>
            {
                Data = customer.UserName,
                Message = "تم حظر العميل بنجاح",
                Succeeded = true,
                Errors = null
            };
        }

        public CustomResponseDTO<object> UnblockCustomer(string customerId)
        {
            Customer customer = _customerRepository.GetCustomerById(customerId);

            if (customer == null)
            {
                return new CustomResponseDTO<object>
                {
                    Data = null,
                    Message = "العميل غير موجود",
                    Succeeded = false,
                    Errors = new List<string> { "العميل غير موجود" }
                };
            }

            if (!customer.IsBlocked)
            {
                return new CustomResponseDTO<object>
                {
                    Data = customer.UserName,
                    Message = "العميل غير محظور بالفعل",
                    Succeeded = false,
                    Errors = null
                };
            }

            customer.IsBlocked = false;
            _customerRepository.Update(customer);
            _customerRepository.Save();

            return new CustomResponseDTO<object>
            {
                Data = customer.UserName,
                Message = "تم رفع الحظر عن العميل بنجاح",
                Succeeded = true,
                Errors = null
            };
        }

        public CustomResponseDTO<object> AcceptService(int id)
        {
            Service service = _serviceRepository.GetById(id);
            if (service == null)
            {
                return new CustomResponseDTO<object>
                {
                    Data = null,
                    Message = "Service not found",
                    Succeeded = false,
                    Errors = new List<string> { "Service not found" }
                };

            }
            if (service.ServiceStatus == ServiceStatus.Accepted)
            {
                return new CustomResponseDTO<object>
                {
                    Data = service.ServiceStatus,
                    Message = " service is already accepted",
                    Succeeded = false,
                    Errors = null
                };

            }
            service.ServiceStatus = ServiceStatus.Accepted;
            _serviceRepository.Update(service);
            AdminRepository.Save();
            return new CustomResponseDTO<object>
            {
                Data = service.ServiceStatus,
                Message = "Service accepted",
                Succeeded = true,
                Errors = null
            };

        }

        public CustomResponseDTO<object> DeclineService(int id)
        {

            Service service = _serviceRepository.GetById(id);
            if (service == null)
            {
                return new CustomResponseDTO<object>
                {
                    Data = null,
                    Message = "Service not found",
                    Succeeded = false,
                    Errors = new List<string> { "Service not found" }
                };

            }
            if (service.ServiceStatus == ServiceStatus.Decline)
            {
                return new CustomResponseDTO<object>
                {
                    Data = service.ServiceStatus,
                    Message = " service is already declined",
                    Succeeded = false,
                    Errors = null
                };

            }
            service.ServiceStatus = ServiceStatus.Decline;
            _serviceRepository.Update(service);
            AdminRepository.Save();
            return new CustomResponseDTO<object>
            {
                Data = service.ServiceStatus,
                Message = "Service declined",
                Succeeded = true,
                Errors = null
            };

        }
        public void DeleteImage(int serviceId, string imageName)
        {
            var service = AdminRepository.GetServiceById(serviceId);
            if (service == null) throw new Exception("Service not found");

            string imagePath = null;

            switch (service)
            {
                case Car car:
                    if (car.Pictures != null)
                    {
                        var carImage = car.Pictures.FirstOrDefault(p => p.Url.Contains(imageName));
                        if (carImage != null)
                        {
                            imagePath = carImage.Url;
                            car.Pictures.Remove(carImage);
                            _carRepository.Update(car);
                        }
                    }
                    break;

                case Hall hall:
                    if (hall.Pictures != null)
                    {
                        var hallImage = hall.Pictures.FirstOrDefault(p => p.Url.Contains(imageName));
                        if (hallImage != null)
                        {
                            imagePath = hallImage.Url;
                            hall.Pictures.Remove(hallImage);
                            _hallRepository.Update(hall);
                        }
                    }
                    break;

                case BeautyCenter beautyCenter:
                    if (beautyCenter.ImagesBeautyCenter != null)
                    {
                        var beautyImage = beautyCenter.ImagesBeautyCenter.FirstOrDefault(p => p.ImageUrl.Contains(imageName));
                        if (beautyImage != null)
                        {
                            imagePath = beautyImage.ImageUrl;
                            beautyCenter.ImagesBeautyCenter.Remove(beautyImage);
                            _beautyCenterRepository.Update(beautyCenter);
                        }
                    }
                    break;

                case Photography photography:
                    if (photography.Images != null)
                    {
                        var photoImage = photography.Images.FirstOrDefault(p => p.ImageURL.Contains(imageName));
                        if (photoImage != null)
                        {
                            imagePath = photoImage.ImageURL;
                            photography.Images.Remove(photoImage);
                            _photographyRepository.Update(photography);
                        }
                    }
                    break;

                default:
                    throw new Exception("Unknown service type");
            }

            if (imagePath != null)
            {
                string basePath = "wwwroot";
                var fullPath = Path.Combine(basePath, imagePath.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }

            AdminRepository.Save();
        }
    }
}
