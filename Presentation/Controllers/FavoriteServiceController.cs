using Application.DTOS;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteServiceController : ControllerBase
    {
        private readonly IFavoriteServiceLayer FavoriteService;

        public FavoriteServiceController(IFavoriteServiceLayer _favoriteService)
        {
            FavoriteService = _favoriteService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            string CustomerID = User.FindFirstValue("uid");
            try
            {
                var response = FavoriteService.GetAll(CustomerID);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Add")]
        public IActionResult AddServiceToFav(int serviceID)
        {
            string CustomerID = User.FindFirstValue("uid");
        
            try
            {
                FavoriteService.AddServiceToFav(serviceID, CustomerID);
                return Ok(new CustomResponseDTO<Object>
                {
                    Data = null,
                    Message = "Serviec Added To Fav Successfully",
                    Succeeded = true,
                    Errors = null
                });
            }
            catch (Exception ex) {

                return NotFound(new CustomResponseDTO<Object>
                {
                    Data = null,
                    Message = "Failed To Add Service To Fav",
                    Succeeded = true,
                    Errors = null
                });
            }
        }

        [HttpPost("Remove")]
        public IActionResult RemoveServiceFromFav(int serviceID)
        {
            string CustomerID = User.FindFirstValue("uid");

            try
            {
                FavoriteService.RemoveServiceFromFav(serviceID, CustomerID);
                return Ok(new CustomResponseDTO<Object>
                {
                    Data = null,
                    Message = "Serviec Removed Successfully",
                    Succeeded = true,
                    Errors = null
                });
            }
            catch (Exception ex)
            {

                return NotFound(new CustomResponseDTO<Object>
                {
                    Data = null,
                    Message = "Failed To Remove Service",
                    Succeeded = true,
                    Errors = null
                });
            }
        }



        [HttpPost("Toggle")]
        public IActionResult ToggleFavorite(int serviceID)
        {
            string CustomerID = User.FindFirstValue("uid");

            try
            {
                bool isAdded = FavoriteService.ToggleFavorite(serviceID, CustomerID);
                return Ok(new CustomResponseDTO<Object>
                {
                    Data = isAdded,
                    Message = isAdded ? "تم اضافة السيرفس الي المفضله" : "تم حذف السيرفس من المفضله",
                    Succeeded = true,
                    Errors = null
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new CustomResponseDTO<Object>
                {
                    Data = null,
                    Message = "حدث خطأ ",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}
