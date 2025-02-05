using Application.DTOS;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class PhotographyService : IPhotographyService
    {
        private readonly IMapper Mapper;
        private readonly IPhotographyRepository photoRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        public PhotographyService(IMapper mapper, IPhotographyRepository _photoRepository, IFavoriteRepository favoriteRepository)
        {
            Mapper = mapper;
            photoRepository = _photoRepository;
            _favoriteRepository = favoriteRepository;
        }

        public CustomResponseDTO<List<PhotographyDTO>> GetAllPhotographer(string customerId,int page, int pageSize)
        {
            var photographies = photoRepository.GetAll();
            var favoriteServiceIds = _favoriteRepository.GetAllFavoritesForCustomer(customerId)
                        .Select(f => f.ServiceId)
                        .ToHashSet();

            var paginatedList = PaginationHelper.Paginate(photographies, page, pageSize);
            //var photographyDTOs = Mapper.Map<List<PhotographyDTO>>(paginatedList.Items);

            var paginationInfo = PaginationHelper.GetPaginationInfo(paginatedList);

            var photographers = paginatedList.Items.Select(photographer =>
            {
                var photographerDto = Mapper.Map<PhotographyDTO>(photographer);
                photographerDto.IsFavorite = favoriteServiceIds.Contains(photographer.ID);
                return photographerDto;
            }).ToList();

            var response = new CustomResponseDTO<List<PhotographyDTO>>
            {
                Data = photographers,
                Message = "عـــــــــاش  الله ينور",
                Succeeded = true,
                Errors = null,
                PaginationInfo = paginationInfo
            };
            return response;
        }
        public async Task<CustomResponseDTO<AddPhotographyDTO>> AddPhotographer(AddPhotographyDTO PhotographerDto)
        {
            try
            {
                var imagePaths = await ImageSavingHelper.SaveImagesAsync(PhotographerDto.Pictures, "PhotographerImages");
                var photographer = Mapper.Map<Photography>(PhotographerDto);
                photographer.Images = imagePaths.Select(path => new Portfolio { ImageURL = path }).ToList();
                PhotographerDto.PictureUrls = imagePaths;

                photoRepository.Insert(photographer);
                photoRepository.Save();

                var resultDTO = Mapper.Map<AddPhotographyDTO>(photographer);

                var response = new CustomResponseDTO<AddPhotographyDTO>
                {
                    Data = resultDTO,
                    Message = "تم إضافة الفوتوجرافر بنجاح",
                    Succeeded = true,
                    Errors = null,
                    PaginationInfo = null
                };

                return response;
            }
            catch (DbUpdateException dbEx)
            {
                // Log inner exception details
                var innerExceptionMessage = dbEx.InnerException?.Message ?? dbEx.Message;

                var errorResponse = new CustomResponseDTO<AddPhotographyDTO>
                {
                    Data = null,
                    Message = "حدث خطأ أثناء إضافة الفوتوجرافر",
                    Succeeded = false,
                    Errors = new List<string> { innerExceptionMessage }
                };
                return errorResponse;
            }
        }

        public CustomResponseDTO<PhotographyDTO> GetPhotographerById(int id)
        {
            var photography = photoRepository.GetById(id);

            var photographyDTO = Mapper.Map<PhotographyDTO>(photography);
            if (photography.FavoriteServices.Count == 0)
                photographyDTO.IsFavorite = false;
            else
                photographyDTO.IsFavorite = true;

            var response = new CustomResponseDTO<PhotographyDTO>
            {
                Data = photographyDTO,
                Message = "عـــــــــاش  الله ينور",
                Succeeded = true,
                Errors = null,
                PaginationInfo = null
            };

            return response;
        }





        public async Task<CustomResponseDTO<AddPhotographyDTO>> UpdatePhotographer(int id, AddPhotographyDTO PhotographerDto)
        {
            try
            {
                var photographer = photoRepository.GetById(id);
                if (photographer == null)
                {
                    return new CustomResponseDTO<AddPhotographyDTO>
                    {
                        Data = null,
                        Message = "Photographer not found",
                        Succeeded = false,
                        Errors = new List<string> { "Not found" }
                    };
                }

                var AllPhotoImages = photoRepository.getAllImages(id);
                // Update basic details
                photographer.Description = PhotographerDto.Description;


                if (PhotographerDto.Pictures != null && PhotographerDto.Pictures.Any())
                {
                    var imagePaths = await ImageSavingHelper.SaveImagesAsync(PhotographerDto.Pictures, "PhotographerImages");
                    foreach (var img in imagePaths)
                    {
                        AllPhotoImages.Add(img);
                    }
                    PhotographerDto.PictureUrls = AllPhotoImages;
                    photographer.Images.Clear();
                    foreach (var item in AllPhotoImages)
                    {
                        photographer.Images.Add(new Portfolio { ImageURL = item, PhotographerId = photographer.ID });
                    }
                }

                photoRepository.Update(photographer);
                photoRepository.Save();

                var resultDTO = Mapper.Map<AddPhotographyDTO>(photographer);

                var response = new CustomResponseDTO<AddPhotographyDTO>
                {
                    Data = resultDTO,
                    Message = "تم تحديث الفوتوجرافر بنجاح",
                    Succeeded = true,
                    Errors = null,
                    PaginationInfo = null
                };

                return response;
            }
            catch (DbUpdateException dbEx)
            {
                var innerExceptionMessage = dbEx.InnerException?.Message ?? dbEx.Message;

                var errorResponse = new CustomResponseDTO<AddPhotographyDTO>
                {
                    Data = null,
                    Message = "حدث خطأ أثناء تحديث الفوتوجرافر",
                    Succeeded = false,
                    Errors = new List<string> { innerExceptionMessage }
                };
                return errorResponse;
            }
        }

        public CustomResponseDTO<AddPhotographyDTO> DeletePhotographerById(int id)
        {
            try
            {
                photoRepository.Delete(id);
                photoRepository.Save();

                var response = new CustomResponseDTO<AddPhotographyDTO>
                {
                    Data = null,
                    Message = "تم حذف  الفوتوجرافر بنجاح",
                    Succeeded = true,
                    Errors = null,
                    PaginationInfo = null
                };

                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<AddPhotographyDTO>
                {
                    Data = null,
                    Message = "حدث خطأ أثناء حذف  الفوتوجرافر",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return errorResponse;
            }
        }


    }
}
