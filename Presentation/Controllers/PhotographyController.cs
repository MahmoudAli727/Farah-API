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
    public class PhotographyController : ControllerBase
    {
        private readonly IPhotographyService _photoService;
        private readonly IHubContext<NotificationsHub> notificationHub;
        private readonly UserManager<ApplicationUser> userManager;

        public PhotographyController(IPhotographyService photoService, UserManager<ApplicationUser> _userManager, IHubContext<NotificationsHub> notificationHub)
        {
            _photoService = photoService;
            this.notificationHub = notificationHub;
            _userManager = userManager;
        }


        [HttpGet]
        public IActionResult GetAllPhotographer(int page = 1, int pageSize = 12)
        {
            try
            {
                string customerId = User.FindFirstValue("uid");
                var response = _photoService.GetAllPhotographer(customerId,page, pageSize);
                if (response.Data == null || !response.Data.Any())
                {
                    return NotFound(new CustomResponseDTO<List<PhotographyDTO>>
                    {
                        Data = null,
                        Message = "No Photographers Found",
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
                    Message = "Error while retrieving the photographers",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(errorResponse);
            }
        }


        [HttpGet("GetPhotographerById")]
        public ActionResult GetPhotographerById(int id)
        {
            try
            {
                var response = _photoService.GetPhotographerById(id);
                if (response.Data != null)
                {
                    return Ok(response);
                }
                return StatusCode(500, "No Photographer match with this Id");

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
        public async Task<ActionResult> AddPhotographer([FromForm] AddPhotographyDTO photographyDTO)
        {
            string OwnerID = User.FindFirstValue("uid");
            try
            {
                photographyDTO.OwnerID = OwnerID;
                var response = await _photoService.AddPhotographer(photographyDTO);
                await notificationHub.Clients.All.SendAsync("newServicesAdded", "مصور");

                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "حدث خطأ أثناء إضافة  الفوتوغرافر",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(errorResponse);
            }
        }



        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdatePhotographer(int id, [FromForm] AddPhotographyDTO photographyDTO)
        {
            string OwnerID = User.FindFirstValue("uid");
            try
            {
                photographyDTO.OwnerID = OwnerID;
                var response =await _photoService.UpdatePhotographer(id, photographyDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "حدث خطأ أثناء تحديث الفوتوجرافر",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(errorResponse);
            }
        }


        [HttpDelete]
        [Authorize]
        public ActionResult DeleteBeautyCenterById(int id)
        {
            try
            {
                var response = _photoService.DeletePhotographerById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "حدث خطأ أثناء حذف  الفوتوجرافر",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(errorResponse);
            }
        }

    }
}
