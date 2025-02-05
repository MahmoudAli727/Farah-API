using Application.DTOS;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Core.Entities;

namespace Application.Services
{
    public class ShopDressesService : IShopDressesService
    {
        private readonly IShopDressesRepository _shopRepository;
        private readonly IMapper _mapper;

        public ShopDressesService(IShopDressesRepository shopRepository, IMapper mapper)
        {
            _shopRepository = shopRepository;
            _mapper = mapper;
        }
        public async Task<CustomResponseDTO<ShopDressesDTo>> AddShopDress(AddShopDressDTO shopDressDto)
        {
            try
            {
                // Save images and update DressDto objects
                foreach (var dress in shopDressDto.Dresses)
                {
                    if (dress.Images != null && dress.Images.Count > 0)
                    {
                        var imageNames = await ImageSavingHelper.SaveImagesAsync(dress.Images, "ShopDressesImages");
                        dress.ImageUrls = imageNames.Select(imageName => Path.Combine("Images", "ShopDressesImages", imageName)).ToList();
                    }
                }

                // Map DTO to domain model
                var shopDress = _mapper.Map<ShopDresses>(shopDressDto);

                // Insert and save shopDress
                _shopRepository.Insert(shopDress);
                _shopRepository.Save();

                // Map domain model back to DTO
                var resultDTO = _mapper.Map<ShopDressesDTo>(shopDress);

                // Create successful response
                var response = new CustomResponseDTO<ShopDressesDTo>
                {
                    Data = resultDTO,
                    Message = "تم إضافة محل الفساتين بنجاح",
                    Succeeded = true,
                    Errors = null,
                    PaginationInfo = null
                };

                return response;
            }
            catch (Exception ex)
            {
                // Create error response
                var errorResponse = new CustomResponseDTO<ShopDressesDTo>
                {
                    Data = null,
                    Message = "حدث خطأ أثناء إضافة محل الفساتين",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return errorResponse;
            }
        }


        public CustomResponseDTO<List<ShopDressesDTo>> GetAllShopDresses(int page, int pageSize, int govId, int cityId)
        {
            var AllShopDresses = _shopRepository.GetAllShopDresses();
            if (govId > 0)
            {
                AllShopDresses = AllShopDresses.Where(p => p.GovernorateID == govId);
            }

            if (cityId > 0)
            {
                AllShopDresses = AllShopDresses.Where(p => p.City == cityId);
            }
            var paginatedList = PaginationHelper.Paginate(AllShopDresses, page, pageSize);

            var ShopDressesDTOs = _mapper.Map<List<ShopDressesDTo>>(paginatedList.Items);

            var paginationInfo = PaginationHelper.GetPaginationInfo(paginatedList);

            var response = new CustomResponseDTO<List<ShopDressesDTo>>
            {
                Data = ShopDressesDTOs,
                Message = "عـــــــــاش  الله ينور",
                Succeeded = true,
                Errors = null,
                PaginationInfo = paginationInfo
            };
            return response;
        }

        public CustomResponseDTO<List<ShopDressesDTo>> GetShopDressesByName(string name)
        {
            var ShopDresses = _shopRepository.GetShopDressesByName(name);
            var shopDressesDto = _mapper.Map<List<ShopDressesDTo>>(ShopDresses);
            var response = new CustomResponseDTO<List<ShopDressesDTo>>
            {
                Data = shopDressesDto,
                Message = "عـــــــــاش  الله ينور",
                Succeeded = true,
                Errors = null,
                PaginationInfo = null
            };

            return response;
        }


        public CustomResponseDTO<ShopDressesDTo> GetShopDressesById(int id)
        {
            var shopDress = _shopRepository.GetById(id);
            var shopDressDTO = _mapper.Map<ShopDressesDTo>(shopDress);

            var response = new CustomResponseDTO<ShopDressesDTo>
            {
                Data = shopDressDTO,
                Message = "عـــــــــاش  الله ينور",
                Succeeded = true,
                Errors = null,
                PaginationInfo = null
            };

            return response;
        }


        public CustomResponseDTO<ShopDressesDTo> UpdateShopDress(ShopDressesDTo shopDressesDTo, int id)
        {
            try
            {

                var shopDress = _shopRepository.GetById(id);
                if (shopDress == null)
                {
                    return new CustomResponseDTO<ShopDressesDTo>
                    {
                        Data = null,
                        Message = "محل الفساتين غير موجود",
                        Succeeded = false,
                        Errors = new List<string> { "Shop dress not found" }
                    };
                }

                // Map basic changes from the DTO to the existing entity
                _mapper.Map(shopDressesDTo, shopDress);

                // Update or add dresses
                foreach (var dressDto in shopDressesDTo.Dresses)
                {
                    var existingDress = shopDress.Dresses.FirstOrDefault(d => d.Id == dressDto.Id);
                    if (existingDress != null)
                    {
                        // Update existing dress
                        _mapper.Map(dressDto, existingDress);
                    }
                    else
                    {
                        // Add new dress
                        var newDress = _mapper.Map<Dress>(dressDto);
                        shopDress.Dresses.Add(newDress);
                    }
                }

                // Remove dresses not present in the DTO
                var dressesToRemove = shopDress.Dresses.Where(d => !shopDressesDTo.Dresses.Any(dto => dto.Id == d.Id)).ToList();
                foreach (var dressToRemove in dressesToRemove)
                {
                    shopDress.Dresses.Remove(dressToRemove);
                }

                // Update the existing entity in the repository
                _shopRepository.Update(shopDress);
                _shopRepository.Save();

                // Map the updated entity back to the DTO
                var resultDTO = _mapper.Map<ShopDressesDTo>(shopDress);

                var response = new CustomResponseDTO<ShopDressesDTo>
                {
                    Data = resultDTO,
                    Message = "تم تعديل محل الفساتين بنجاح",
                    Succeeded = true,
                    Errors = null,
                    PaginationInfo = null
                };

                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<ShopDressesDTo>
                {
                    Data = null,
                    Message = "حدث خطأ أثناء تعديل محل الفساتين",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return errorResponse;
            }
        }



        public CustomResponseDTO<ShopDressesDTo> DeleteShopDressById(int id)
        {
            _shopRepository.Delete(id);
            _shopRepository.Save();
            try
            {
                var response = new CustomResponseDTO<ShopDressesDTo>
                {
                    Data = null,
                    Message = "تم حذف  محل الفساتين بنجاح",
                    Succeeded = true,
                    Errors = null,
                    PaginationInfo = null
                };

                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<ShopDressesDTo>
                {
                    Data = null,
                    Message = "حدث خطأ أثناء حذف البيوتي سنتر",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return errorResponse;
            }

        }


    }
}
