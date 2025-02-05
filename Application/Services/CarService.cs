using Application.DTOS;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Core.Entities;

namespace Application.Services
{
    public class CarService : ICarService
    {

        private readonly IMapper Mapper;
        private readonly ICarRepository carRepository;
        private readonly IFavoriteRepository favoriteRepository;

        public CarService(IMapper mapper, ICarRepository _carRepository, IFavoriteRepository _favoriteRepository)
        {
            Mapper = mapper;
            carRepository = _carRepository;
            favoriteRepository = _favoriteRepository;
        }

        public CustomResponseDTO<List<CarDTO>> GetAllCars(string customerId,int page, int pageSize, string priceRange, int govId, int city)
        {
            var allCars = carRepository.GetAll();
            var favoriteServiceIds = favoriteRepository.GetAllFavoritesForCustomer(customerId)
                                    .Select(f => f.ServiceId)
                                    .ToHashSet();


            if (!string.IsNullOrEmpty(priceRange) && priceRange != "all")
            {
                int minPrice;
                int maxPrice;

                if (priceRange.StartsWith("<"))
                {
                    maxPrice = int.Parse(priceRange.Substring(1));
                    allCars = allCars.Where(h => h.Price < maxPrice);
                }
                else if (priceRange.StartsWith(">"))
                {
                    minPrice = int.Parse(priceRange.Substring(1));
                    allCars = allCars.Where(h => h.Price > minPrice);
                }
                else
                {
                    var priceRanges = priceRange.Split('-').Select(int.Parse).ToArray();
                    if (priceRanges.Length == 2)
                    {
                        minPrice = priceRanges[0];
                        maxPrice = priceRanges[1];
                        allCars = allCars.Where(h => h.Price >= minPrice && h.Price <= maxPrice);
                    }
                }
            }
            if (govId > 0)
            {
                allCars = allCars.Where(p => p.GovernorateID == govId);
            }

            if (city > 0)
            {
                allCars = allCars.Where(p => p.City == city);
            }

            var paginatedList = PaginationHelper.Paginate(allCars, page, pageSize);

            if (!paginatedList.Items.Any())
            {
                return new CustomResponseDTO<List<CarDTO>>
                {
                    Data = null,
                    Message = "لا يوجد سيارات",
                    Succeeded = false,
                    Errors = new List<string> { "لا يوجد سيارات" },
                    PaginationInfo = null
                };
            }


            var paginationInfo = PaginationHelper.GetPaginationInfo(paginatedList);

            var cars = paginatedList.Items.Select(car =>
            {
                var carDto = Mapper.Map<CarDTO>(car);
                carDto.IsFavorite = favoriteServiceIds.Contains(car.ID);
                return carDto;
            }).ToList();

            //var cars = Mapper.Map<List<CarDTO>>(paginatedList.Items);

            return new CustomResponseDTO<List<CarDTO>>
            {
                Data = cars,
                Message = "تم",
                Succeeded = true,
                Errors = null,
                PaginationInfo = paginationInfo
            };

        }

        public async Task<CarDTO> AddCar(AddCarDTO carDto)
        {
            var car = Mapper.Map<Car>(carDto);



            var imagePaths = await ImageSavingHelper.SaveImagesAsync(carDto.Pictures, "Cars");
            car.Pictures = imagePaths.Select(path => new CarPicture { Url = path }).ToList();
            carDto.PictureUrls = imagePaths;

            carRepository.Insert(car);
            carRepository.Save();

            return Mapper.Map<CarDTO>(car);
        }

        public async Task<CarDTO> EditCar(int id, CarDTO carDto)
        {
            var existingCar = carRepository.GetCarById(id);
            if (existingCar == null)
            {
                throw new Exception("تعذر العثور علي السياره");
            }


            var AllCarImages = carRepository.getAllImages(id);
            Mapper.Map(carDto, existingCar);

            if (carDto.Pictures != null && carDto.Pictures.Any())
            {
                var imagePaths = await ImageSavingHelper.SaveImagesAsync(carDto.Pictures, "Cars");

                foreach (var img in imagePaths)
                {
                    AllCarImages.Add(img);
                }
                carDto.PictureUrls = AllCarImages;
                existingCar.Pictures.Clear();
                foreach (var item in AllCarImages)
                {
                    existingCar.Pictures.Add(new CarPicture { Url = item, CarID = existingCar.ID });
                }
            }

            carRepository.Update(existingCar);
            carRepository.Save();
            return Mapper.Map<CarDTO>(existingCar);
        }
        public void Delete(int id)
        {
            try
            {
                Car car = carRepository.GetCarById(id);
                if (car == null)
                {
                    throw new Exception("تعذر العثور علي السياره");
                }
                car.IsDeleted = true;

                carRepository.Update(car);
                carRepository.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CustomResponseDTO<CarDTO> GetCarById(int id)
        {
            Car car = carRepository.GetCarById(id);



            if (car == null)
            {
                return new CustomResponseDTO<CarDTO>()
                {
                    Data = null,
                    Message = "تعذر العثور علي السياره",
                    Succeeded = false,
                    Errors = null,
                };
            }

            CarDTO carDTO = Mapper.Map<CarDTO>(car);

            if (car.FavoriteServices.Count == 0)
                carDTO.IsFavorite = false;
            else
                carDTO.IsFavorite = true;

            return new CustomResponseDTO<CarDTO>
            {
                Data = carDTO,
                Message = "تم",
                Succeeded = true,
                Errors = null
            };
        }

    }
}
