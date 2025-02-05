using Application.DTOS;
using Application.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentation.Hubs;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeautyCenterController : ControllerBase
    {
        private readonly IBeautyService _beautyService;
        private readonly IHubContext<NotificationsHub> notificationHub;
        private readonly UserManager<ApplicationUser> userManager;

        public BeautyCenterController(IBeautyService beautyService, UserManager<ApplicationUser> _userManager, IHubContext<NotificationsHub> notificationHub)
        {
            _beautyService = beautyService;
            this.notificationHub = notificationHub;
            _userManager = userManager;
        }

        [HttpGet]
        public ActionResult GetAllBeautyCenters(int page = 1, int pageSize = 12, int govId = 0, int cityId = 0)
        {
            try
            {
                string customerId = User.FindFirstValue("uid");
                var response = _beautyService.GetAllBeautyCenters(customerId, page, pageSize, govId, cityId);
                if (response.Data == null || !response.Data.Any())
                {
                    return NotFound(new CustomResponseDTO<List<BeautyCenterDTO>>
                    {
                        Data = null,
                        Message = "No Beauty Centers Found",
                        Succeeded = false,
                        Errors = null
                    });
                }
                return Ok(response);

            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "Error while retrieving the beauty centers",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(errorResponse);
            }
        }


        [HttpGet("GetBeautyCentersByName")]
        public ActionResult GetBeautyCentersByName(string name)
        {
            try
            {
                var response = _beautyService.GetBeautyCenterByName(name);
                if (response.Data.Count > 0)
                {
                    return Ok(response);
                }
                return StatusCode(500, "No Beauty Center match with this name");

            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "ايروووووووووووووور",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message, ex.StackTrace }
                };
                return StatusCode(500, errorResponse);
            }
        }


        [HttpGet("GetBeautyCentersById/{id}")]
        public ActionResult GetBeautyCentersById(int id)
        {
            try
            {
                var response = _beautyService.GetBeautyCenterById(id);
                if (response.Data != null)
                {
                    return Ok(response);
                }
                return StatusCode(500, "No Beauty Center match with this Id");

            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "ايروووووووووووووور",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message, ex.StackTrace }
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost("AddBeautyService")]
        public IActionResult AddBeautyService(List<ServiceForBeautyCenterDTO> beautyDTO)
        {
            try
            {
                var response = _beautyService.AddBeautyService(beautyDTO);

                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "ايروووووووووووووور",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message, ex.StackTrace }
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpDelete("RemoveBeautyService")]
        public IActionResult RemoveBeautyService(int beautyID, int ServiceID)
        {
            try
            {
                var response = _beautyService.RemoveBeautyService(beautyID, ServiceID);

                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "ايروووووووووووووور",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message, ex.StackTrace }
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddBeautyCenter([FromForm] AddBeautyCenterDTO beautyCenterDTO)
        {
            string OwnerID = User.FindFirstValue("uid");
            try
            {
                beautyCenterDTO.OwnerID = OwnerID;
                var response = await _beautyService.AddBeautyCenter(beautyCenterDTO);
                await notificationHub.Clients.All.SendAsync("newServicesAdded", "بيوتي سنتر");

                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "حدث خطأ أثناء إضافة البيوتي سنتر",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(errorResponse);
            }
        }




        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateBeautyCenter(AddBeautyCenterDTO beautyCenterDTO, int id)
        {
            string OwnerID = User.FindFirstValue("uid");
            try
            {
                beautyCenterDTO.OwnerID = OwnerID;
                var response = await _beautyService.UpdateBeautyCenter(beautyCenterDTO, id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "حدث خطأ أثناء تعديل البيوتي سنتر",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(errorResponse);
            }
        }



        [HttpDelete]
        //[Authorize]
        public ActionResult DeleteBeautyCenterById(int id)
        {
            try
            {
                var response = _beautyService.DeleteBeautyCenterById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "حدث خطأ أثناء حذف البيوتي سنتر",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(errorResponse);
            }
        }







    }
}

//////////////////////////////////////////////////////////////////
//public PropertyPaginationResponseDTO GetAllProperties(int pageNum, int pageSize, int numOfRooms, string priceRange, int govId, int city)
//{
//    //Pagination 


//    //Search 
//    var query = dbContext.Properties.AsQueryable();

//    if (numOfRooms > 0)
//    {
//        query = query.Where(p => p.RoomsNumber == numOfRooms);
//    }

//    if (priceRange != "all")
//    {
//        int minPrice;
//        int maxPrice;

//        if (priceRange.StartsWith("<"))
//        {
//            maxPrice = int.Parse(priceRange.Substring(1));
//            query = query.Where(p => p.Price < maxPrice);
//        }
//        else if (priceRange.StartsWith(">"))
//        {
//            minPrice = int.Parse(priceRange.Substring(1));
//            query = query.Where(p => p.Price > minPrice);
//        }
//        else
//        {
//            var priceRanges = priceRange.Split('-').Select(int.Parse).ToArray();
//            if (priceRanges.Length == 2)
//            {
//                minPrice = priceRanges[0];
//                maxPrice = priceRanges[1];
//                query = query.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
//            }
//        }
//    }

//    if (govId > 0)
//    {
//        query = query.Where(p => p.GovernorateID == govId);
//    }

//    if (city > 0)
//    {
//        query = query.Where(p => p.City == city);
//    }


//    int total = query.Count();
//    var paginatedList = new PaginatedList(total, pageNum, pageSize);

//    int totalPages = paginatedList.TotalPages;
//    int currentPage = paginatedList.CurrentPage;
//    int startPage = paginatedList.StartPage;
//    int endPage = paginatedList.EndPage;

//    int skipedData = (pageNum - 1) * pageSize;
//    int cityId = Convert.ToInt32(city);
//    List<Properties> properties = query.Skip(skipedData).Take(pageSize).ToList();
//    List<displayPropertyDTO> displayPropertyDTOs = mapper.Map<List<displayPropertyDTO>>(properties);

//    foreach (var property in displayPropertyDTOs)
//    {
//        property.isForSale = property.status == "Sale";
//        property.Address = dbContext.Governorates.Where(g => g.GovernorateID == property.govID)
//                                                 .Select(g => g.Name)
//                                                 .FirstOrDefault() + ", " +
//                          dbContext.Cities.Where(c => c.Id == property.cityId)
//        .Select(c => c.Name)
//                                          .FirstOrDefault();
//        property.imageUrl = dbContext.PropertyImages.Where(img => img.PropertyId == property.id)
//                                                     .Select(img => img.ImageUrl)
//                                                     .FirstOrDefault();
//    }

//    var responseDTO = new PropertyPaginationResponseDTO
//    {
//        Properties = displayPropertyDTOs,
//        PaginationInfo = new PaginationInfoDTO
//        {
//            Total = total,
//            TotalPages = totalPages,
//            CurrentPage = currentPage,
//            StartPage = startPage,
//            EndPage = endPage
//        }
//    };

//    return responseDTO;
//}

//[HttpGet]
//public async Task<ActionResult> GetAllProperty(int pageNum = 1, int pageSize = 6, int numOfRooms = 0, string priceRange = "all", int govId = 0, int cityId = 0)
//{
//    try
//    {
//        PropertyPaginationResponseDTO displayPropertyDTO = propertyServices.GetAllProperties(pageNum, pageSize, numOfRooms, priceRange, govId, cityId);
//        if (displayPropertyDTO == null)
//        {
//            var customResponseWithNoDate = new CustomResponseDTO
//            {
//                Success = false,
//                Message = "The Properties Is Null ",
//                Data = null,
//                Errors = null
//            };
//            return BadRequest(customResponseWithNoDate);
//        }

//        var customResponseSuccessfully = new CustomResponseDTO
//        {
//            Success = true,
//            Data = displayPropertyDTO,
//            Message = "data successfully retrieved",
//            Errors = null
//        };
//        return Ok(customResponseSuccessfully);

//    }
//    catch (Exception ex)
//    {
//        var customResponse = new CustomResponseDTO
//        {
//            Success = false,
//            Message = "An error occurred while retrieving the Properties",
//            Data = null,
//            Errors = new List<string> { ex.Message }
//        };
//        return BadRequest(customResponse);
//    }
//}
