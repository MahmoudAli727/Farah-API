using Application.DTOS;
using Application.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopDressesController : ControllerBase
    {
        private readonly IShopDressesService _dressesService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShopDressesController(IShopDressesService dressesService, UserManager<ApplicationUser> userManager)
        {
            _dressesService = dressesService;
            _userManager = userManager;
        }

        [HttpGet]
        public ActionResult GetAllShopDresses(int page = 1, int pageSize = 12, int govId = 0, int cityId = 0)
        {
            try
            {
                var response = _dressesService.GetAllShopDresses(page, pageSize, govId, cityId);

                if (response.Data == null || !response.Data.Any())
                {
                    return NotFound(new CustomResponseDTO<List<ShopDressesDTo>>
                    {
                        Data = null,
                        Message = "No Shop Dresses Found",
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
                    Message = "Error occurred while retrieving shop dresses",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message, ex.StackTrace }
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpGet("GetShopDressesByName")]
        public ActionResult GetShopDressesByName(string name)
        {
            try
            {
                var response = _dressesService.GetShopDressesByName(name);
                if (response.Data.Count > 0)
                {
                    return Ok(response);
                }
                return NotFound("No Shop Dresses match with this name");
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "Error occurred while retrieving shop dresses by name",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message, ex.StackTrace }
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpGet("GetShopDressesById")]
        public ActionResult GetShopDressesById(int id)
        {
            try
            {
                var response = _dressesService.GetShopDressesById(id);
                if (response.Data != null)
                {
                    return Ok(response);
                }
                return NotFound("No Shop Dresses match with this Id");
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "Error occurred while retrieving shop dresses by Id",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message, ex.StackTrace }
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost]
        [Authorize]
        public  ActionResult AddShopDresses([FromForm] AddShopDressDTO shopDressDto)
        {
            try
            {
                string ownerID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                shopDressDto.OwnerID = ownerID;

                var response = _dressesService.AddShopDress(shopDressDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "Error occurred while adding shop dress",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(errorResponse);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult UpdateShopDress([FromForm] ShopDressesDTo shopDressesDto, int id)
        {
            try
            {
                string ownerID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                shopDressesDto.OwnerID = ownerID;

                var response =  _dressesService.UpdateShopDress(shopDressesDto, id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "Error occurred while updating shop dress",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(errorResponse);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult DeleteShopDressById(int id)
        {
            try
            {
                var response =  _dressesService.DeleteShopDressById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "Error occurred while deleting shop dress",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(errorResponse);
            }
        }
    }
}
