using Application.DTOS;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Core.Entities;

namespace Application.Services
{
    public class HallService : IHallService
    {
        private readonly IMapper Mapper;
        private readonly IHallRepository HallRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        public HallService(IMapper _Mapper, IHallRepository _HallRepository, IFavoriteRepository favoriteRepository)
        {
            HallRepository = _HallRepository;
            Mapper = _Mapper;
            _favoriteRepository = favoriteRepository;
        }
        public void Delete(int id)
        {
            try
            {
                Hall car = HallRepository.GetById(id);
                if (car == null)
                {
                    throw new Exception("تعذر العثور علي القاعه");
                }
                car.IsDeleted = true;

                HallRepository.Update(car);
                HallRepository.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CustomResponseDTO<List<HallDTO>> GetAllHalls(string customerId,int page, int pageSize, string priceRange, int govId, int cityId)
        {
            var query = HallRepository.GetAll();
            var favoriteServiceIds = _favoriteRepository.GetAllFavoritesForCustomer(customerId)
                        .Select(f => f.ServiceId)
                        .ToHashSet();

            // Apply price range filter
            if (!string.IsNullOrEmpty(priceRange) && priceRange != "all")
            {
                int minPrice;
                int maxPrice;

                if (priceRange.StartsWith("<"))
                {
                    maxPrice = int.Parse(priceRange.Substring(1));
                    query = query.Where(h => h.Price < maxPrice);
                }
                else if (priceRange.StartsWith(">"))
                {
                    minPrice = int.Parse(priceRange.Substring(1));
                    query = query.Where(h => h.Price > minPrice);
                }
                else
                {
                    var priceRanges = priceRange.Split('-').Select(int.Parse).ToArray();
                    if (priceRanges.Length == 2)
                    {
                        minPrice = priceRanges[0];
                        maxPrice = priceRanges[1];
                        query = query.Where(h => h.Price >= minPrice && h.Price <= maxPrice);
                    }
                }
            }
            if (govId > 0)
            {
                query = query.Where(p => p.GovernorateID == govId);
            }

            if (cityId > 0)
            {
                query = query.Where(p => p.City == cityId);
            }
            // Check if any halls match the filters
            var paginatedList = PaginationHelper.Paginate(query, page, pageSize);

            if (!paginatedList.Items.Any())
            {
                return new CustomResponseDTO<List<HallDTO>>
                {
                    Data = null,
                    Message = "لا يوجد قاعات",
                    Succeeded = false,
                    Errors = new List<string> { "لا يوجد قاعات" },
                    PaginationInfo = null
                };
            }

            //var HallsDTO = Mapper.Map<List<HallDTO>>(paginatedList.Items);

            var paginationInfo = PaginationHelper.GetPaginationInfo(paginatedList);
            var halls = paginatedList.Items.Select(hall =>
            {
                var hallDto = Mapper.Map<HallDTO>(hall);
                hallDto.IsFavorite = favoriteServiceIds.Contains(hall.ID);
                return hallDto;
            }).ToList();
            return new CustomResponseDTO<List<HallDTO>>
            {
                Data = halls,
                Message = "تم",
                Succeeded = true,
                Errors = null,
                PaginationInfo = paginationInfo
            };
        }

        public async Task<HallDTO> AddHall(AddHallDTO HallDto)
        {
            var hall = Mapper.Map<Hall>(HallDto);

            var imagePaths = await ImageSavingHelper.SaveImagesAsync(HallDto.Pictures, "Halls");
            hall.Pictures = imagePaths.Select(path => new HallPicture { Url = path }).ToList();
            HallDto.PictureUrls = imagePaths;

            HallRepository.Insert(hall);
            HallRepository.Save();

            return Mapper.Map<HallDTO>(hall);
        }

        //public async Task<HallDTO> EditHall(int id, HallDTO hallDto)
        //{
        //    var existingHall = HallRepository.GetById(id);
        //    if (existingHall == null)
        //    {
        //        throw new Exception("تعذر العثور علي القاعه");
        //    }

        //    Mapper.Map(hallDto, existingHall);

        //    if (hallDto.Pictures != null && hallDto.Pictures.Any())
        //    {
        //        var imagePaths = await ImageSavingHelper.SaveImagesAsync(hallDto.Pictures, "Halls");
        //        existingHall.Pictures = imagePaths.Select(path => new HallPicture { Url = path }).ToList();
        //        hallDto.PictureUrls = imagePaths;
        //    }

        //    UpdateHallFeatures(existingHall, hallDto.Features);

        //    HallRepository.Update(existingHall);
        //    HallRepository.Save();
        //    return Mapper.Map<HallDTO>(existingHall);
        //}

        public async Task<HallDTO> EditHall(int id, HallDTO hallDto)
        {
            var existingHall = HallRepository.GetById(id);
            if (existingHall == null)
            {
                throw new Exception("تعذر العثور علي القاعه");
            }

            var AllHallImages = await HallRepository.getAllImages(id);
            Mapper.Map(hallDto, existingHall);

            if (hallDto.Pictures != null && hallDto.Pictures.Any())
            {
                var imagePaths = await ImageSavingHelper.SaveImagesAsync(hallDto.Pictures, "Halls");

                //existingHall.Pictures = imagePaths.Select(path => new HallPicture { Url = path }).ToList();
                foreach (var img in imagePaths)
                {
                    AllHallImages.Add(img);
                }
                hallDto.PictureUrls = AllHallImages;
                existingHall.Pictures.Clear();

                foreach (var item in AllHallImages)
                {
                    existingHall.Pictures.Add(new HallPicture { Url = item, HallID = existingHall.ID });
                }
            }

            UpdateHallFeatures(existingHall, hallDto.Features);
            HallRepository.Update(existingHall);
            HallRepository.Save();
            return Mapper.Map<HallDTO>(existingHall);
        }
        private void UpdateHallFeatures(Hall existingHall, List<string> newFeatures)
        {
            var existingFeatures = existingHall.Features.ToList();

            var featuresToAdd = newFeatures
                .Where(nf => !existingFeatures.Any(ef => ef.Feature == nf))
                .Select(nf => new HallFeature
                {
                    Feature = nf,
                    HallId = existingHall.ID,
                }).ToList();

            var featuresToDelete = existingFeatures
                .Where(ef => !newFeatures.Any(nf => nf == ef.Feature))
                .ToList();

            // Remove deleted features
            foreach (var feature in featuresToDelete)
            {
                existingHall.Features.Remove(feature);
            }

            // Add new features
            foreach (var feature in featuresToAdd)
            {
                existingHall.Features.Add(feature);
            }
        }
        public CustomResponseDTO<HallDTO> GetHallById(int id)
        {
            Hall hall = HallRepository.GetById(id);

            if (hall == null)
            {
                return new CustomResponseDTO<HallDTO>()
                {
                    Data = null,
                    Message = "تعذر العثور علي القاعه",
                    Succeeded = false,
                    Errors = null,
                };
            }

            HallDTO hallDTO = Mapper.Map<HallDTO>(hall);

            if (hall.FavoriteServices.Count == 0)
                hallDTO.IsFavorite = false;
            else
                hallDTO.IsFavorite = true;

            return new CustomResponseDTO<HallDTO>
            {
                Data = hallDTO,
                Message = "تم",
                Succeeded = true,
                Errors = null
            };
        }

    }
}
