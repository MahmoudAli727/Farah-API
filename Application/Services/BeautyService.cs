using Application.DTOS;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.ConstrainedExecution;

namespace Application.Services
{
    public class BeautyService : IBeautyService
    {
        private readonly IBeautyRepository _beautyRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IMapper _mapper;

        public BeautyService(IBeautyRepository beautyRepository, IMapper mapper, IFavoriteRepository favoriteRepository)
        {
            _beautyRepository = beautyRepository;
            _mapper = mapper;
            _favoriteRepository = favoriteRepository;
        }
        public CustomResponseDTO<List<BeautyCenterDTO>> GetAllBeautyCenters(string customerId, int page, int pageSize, int govId, int cityId)
        {
            var beautyCenters = _beautyRepository.GetAllBeautyCenters().AsQueryable();
            var favoriteServiceIds = _favoriteRepository.GetAllFavoritesForCustomer(customerId)
                        .Select(f => f.ServiceId)
                        .ToHashSet();

            if (govId > 0)
            {
                beautyCenters = beautyCenters.Where(p => p.Gove == govId);
            }


            if (cityId > 0)
            {
                beautyCenters = beautyCenters.Where(p => p.City == cityId);
            }

            var paginatedList = PaginationHelper.Paginate(beautyCenters, page, pageSize);
            var paginationInfo = PaginationHelper.GetPaginationInfo(paginatedList);
            //var beautyCenterDTOs = _mapper.Map<List<BeautyCenterDTO>>(paginatedList.Items);
            if (!paginatedList.Items.Any())
            {
                return new CustomResponseDTO<List<BeautyCenterDTO>>
                {
                    Data = null,
                    Message = "لا يوجد مراكز تجميل",
                    Succeeded = false,
                    Errors = new List<string> { "لا يوجد مراكز تجميل" },
                    PaginationInfo = null
                };
            }

            if (cityId > 0)
            {
                beautyCenters = beautyCenters.Where(p => p.City == cityId);
            }

            var BeautyCenters = paginatedList.Items.Select(beauty =>
            {
                var beautyDto = _mapper.Map<BeautyCenterDTO>(beauty);
                beautyDto.IsFavorite = favoriteServiceIds.Contains(beauty.ID);
                return beautyDto;
            }).ToList();


            var response = new CustomResponseDTO<List<BeautyCenterDTO>>
            {
                Data = BeautyCenters,
                Message = "عـــــــــاش  الله ينور",
                Succeeded = true,
                Errors = null,
                PaginationInfo = paginationInfo
            };
            return response;
        }



        public CustomResponseDTO<List<BeautyCenterDTO>> GetBeautyCenterByName(string name)
        {
            var beautyCenters = _beautyRepository.GetBeautyCenterByName(name);
            var beautyCenterDTOs = _mapper.Map<List<BeautyCenterDTO>>(beautyCenters);

            var response = new CustomResponseDTO<List<BeautyCenterDTO>>
            {
                Data = beautyCenterDTOs,
                Message = "عـــــــــاش  الله ينور",
                Succeeded = true,
                Errors = null,
                PaginationInfo = null
            };

            return response;
        }



        public CustomResponseDTO<BeautyCenterDTO> GetBeautyCenterById(int id)
        {
            var beautyCenter = _beautyRepository.GetServiceById(id);

            var beautyCenterDTO = _mapper.Map<BeautyCenterDTO>(beautyCenter);

            if (beautyCenter.FavoriteServices.Count == 0)
                beautyCenterDTO.IsFavorite = false;
            else
                beautyCenterDTO.IsFavorite = true;

            var response = new CustomResponseDTO<BeautyCenterDTO>
            {
                Data = beautyCenterDTO,
                Message = "عـــــــــاش  الله ينور",
                Succeeded = true,
                Errors = null,
                PaginationInfo = null
            };

            return response;
        }

        public async Task<CustomResponseDTO<BeautyCenterDTO>> AddBeautyCenter(AddBeautyCenterDTO beautyCenterDTO)
        {
            try
            {
                var imagePaths = await ImageSavingHelper.SaveImagesAsync(beautyCenterDTO.Images, "BeautyCenterImages");

                var beautyCenter = _mapper.Map<BeautyCenter>(beautyCenterDTO);
                beautyCenter.ImagesBeautyCenter = imagePaths.Select(path => new ImagesBeautyCenter { ImageUrl = path }).ToList();
                beautyCenterDTO.ImageUrls = imagePaths;

                // Map single service object to ServicesForBeautyCenter collection
                //beautyCenter.ServicesForBeautyCenter = new List<ServiceForBeautyCenter>
                //    {
                //        _mapper.Map<ServiceForBeautyCenter>(beautyCenterDTO.Services)
                //    };

                _beautyRepository.Insert(beautyCenter);
                _beautyRepository.Save();

                var resultDTO = _mapper.Map<BeautyCenterDTO>(beautyCenter);

                var response = new CustomResponseDTO<BeautyCenterDTO>
                {
                    Data = resultDTO,
                    Message = "تم إضافة البيوتي سنتر بنجاح",
                    Succeeded = true,
                    Errors = null,
                    PaginationInfo = null
                };

                return response;
            }
            catch (DbUpdateException dbEx)
            {
                var innerExceptionMessage = dbEx.InnerException?.Message ?? dbEx.Message;

                var errorResponse = new CustomResponseDTO<BeautyCenterDTO>
                {
                    Data = null,
                    Message = "حدث خطأ أثناء إضافة البيوتي سنتر",
                    Succeeded = false,
                    Errors = new List<string> { innerExceptionMessage }
                };
                return errorResponse;
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<BeautyCenterDTO>
                {
                    Data = null,
                    Message = "حدث خطأ أثناء إضافة البيوتي سنتر",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return errorResponse;
            }
        }




        public async Task<CustomResponseDTO<AddBeautyCenterDTO>> UpdateBeautyCenter(AddBeautyCenterDTO beautyCenterDTO, int id)
        {
            try
            {
                var beautyCenter = _beautyRepository.GetServiceById(id);
                if (beautyCenter == null)
                {
                    return new CustomResponseDTO<AddBeautyCenterDTO>
                    {
                        Data = null,
                        Message = "البيوتي سنتر غير موجود",
                        Succeeded = false,
                        Errors = new List<string> { "Beauty center not found" }
                    };
                }


                var AllBeautyImages = _beautyRepository.getAllImages(id);

                // Update the existing beautyCenter entity with new values from the DTO
                beautyCenter.Name = beautyCenterDTO.Name;
                beautyCenter.Description = beautyCenterDTO.Description;
                beautyCenter.Gove = beautyCenterDTO.Gove;
                beautyCenter.City = beautyCenterDTO.City;
                //beautyCenter.ServicesForBeautyCenter = _mapper.Map<List<ServiceForBeautyCenter>>(beautyCenterDTO.Services);



                if (beautyCenterDTO.Images != null && beautyCenterDTO.Images.Any())
                {
                    var imagePaths = await ImageSavingHelper.SaveImagesAsync(beautyCenterDTO.Images, "BeautyCenterImages");
                    foreach (var img in imagePaths)
                    {
                        AllBeautyImages.Add(img);
                    }
                    beautyCenterDTO.ImageUrls = AllBeautyImages;
                    beautyCenter.ImagesBeautyCenter.Clear();

                    foreach (var item in AllBeautyImages)
                    {
                        beautyCenter.ImagesBeautyCenter.Add(new ImagesBeautyCenter { ImageUrl = item, BeautyCenterId = beautyCenter.ID });
                    }

                }
               

                _beautyRepository.Update(beautyCenter);
                _beautyRepository.Save();

                // Map the updated entity back to the DTO
                var resultDTO = _mapper.Map<AddBeautyCenterDTO>(beautyCenter);

                var response = new CustomResponseDTO<AddBeautyCenterDTO>
                {
                    Data = resultDTO,
                    Message = "تم تعديل البيوتي سنتر بنجاح",
                    Succeeded = true,
                    Errors = null,
                    PaginationInfo = null
                };

                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<AddBeautyCenterDTO>
                {
                    Data = null,
                    Message = "حدث خطأ أثناء تعديل البيوتي سنتر",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return errorResponse;
            }
        }


        public CustomResponseDTO<AddBeautyCenterDTO> DeleteBeautyCenterById(int id)
        {
            try
            {
                _beautyRepository.Delete(id);
                _beautyRepository.Save();

                var response = new CustomResponseDTO<AddBeautyCenterDTO>
                {
                    Data = null,
                    Message = "تم حذف البيوتي سنتر بنجاح",
                    Succeeded = true,
                    Errors = null,
                    PaginationInfo = null
                };

                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<AddBeautyCenterDTO>
                {
                    Data = null,
                    Message = "حدث خطأ أثناء حذف البيوتي سنتر",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return errorResponse;
            }
        }

        public CustomResponseDTO<List<ServiceForBeautyCenterDTO>> AddBeautyService(List<ServiceForBeautyCenterDTO> beautyDTOs)
        {
            try
            {
                var services = _mapper.Map<List<ServiceForBeautyCenter>>(beautyDTOs);

                foreach (var service in services)
                {
                    _beautyRepository.InsertService(service);
                }

                _beautyRepository.Save();

                return new CustomResponseDTO<List<ServiceForBeautyCenterDTO>>()
                {
                    Data = beautyDTOs,
                    Message = "تم اضافة الخدمه بنجاح",
                    Succeeded = true,
                    Errors = null,
                    PaginationInfo = null
                };
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<ServiceForBeautyCenterDTO>>
                {
                    Data = null,
                    Message = "حدث خطأ اثناء اضافة الخدمه",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return errorResponse;
            }
        }

        public CustomResponseDTO<ServiceForBeautyCenterDTO> RemoveBeautyService(int beautyID, int ServiceID)
        {
            try
            {
                ServiceForBeautyCenter serviceForBeautyCenter = _beautyRepository.GetBeautyService(beautyID, ServiceID);

                _beautyRepository.RemoveService(serviceForBeautyCenter);
                _beautyRepository.Save();
                var service = _mapper.Map<ServiceForBeautyCenterDTO>(serviceForBeautyCenter);

                return new CustomResponseDTO<ServiceForBeautyCenterDTO>()
                {
                    Data = service,
                    Message = "تم حذف الخدمه بنجاح",
                    Succeeded = true,
                    Errors = null,
                    PaginationInfo = null
                };
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<ServiceForBeautyCenterDTO>
                {
                    Data = null,
                    Message = "حدث خطأ اثناء حذف الخدمه",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return errorResponse;
            }
        }
    }
}




