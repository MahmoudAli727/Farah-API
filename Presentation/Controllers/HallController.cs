using Application.DTOS;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentation.Hubs;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallController : ControllerBase
    {
        private readonly IHallService HallService;
        private readonly IHubContext<NotificationsHub> notificationHub;

        public HallController(IHallService _hallService, IHubContext<NotificationsHub> notificationHub)
        {
            HallService = _hallService;
            this.notificationHub = notificationHub;
        }

        [HttpGet("AllHalls")]
        public IActionResult GetAll(int page = 1, int pageSize = 12, string priceRange = "all", int govId = 0, int cityId = 0)
        {
            try
            {
                string customerId = User.FindFirstValue("uid");
                var response = HallService.GetAllHalls(customerId, page, pageSize, priceRange, govId, cityId);
                if (response.Data == null || !response.Data.Any())
                {
                    return NotFound(new CustomResponseDTO<List<HallDTO>>
                    {
                        Data = null,
                        Message = "No Halls Found",
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
                    Message = "Error while retrieving the Halls",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(errorResponse);
            }
        }


        [HttpPost("AddHall")]
        public async Task<ActionResult> AddHall([FromForm] AddHallDTO hallDTO)
        {
            string OwnerID = User.FindFirstValue("uid");

            try
            {
                hallDTO.OwnerID = OwnerID;
                var Hall = await HallService.AddHall(hallDTO);
                await notificationHub.Clients.All.SendAsync("newServicesAdded", "قاعة");

                return Ok(new CustomResponseDTO<HallDTO>
                {
                    Data = Hall,
                    Message = "Hall added successfully",
                    Succeeded = true,
                    Errors = null
                });
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "Error while adding the Hall",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(errorResponse);
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> EditHall([FromForm] HallDTO hallDto, int id)
        {
            string ownerID = User.FindFirstValue("uid");

            try
            {
                hallDto.OwnerID = ownerID;
                hallDto.HallID = id;
                var hall = await HallService.EditHall(id, hallDto);

                return Ok(new CustomResponseDTO<HallDTO>
                {
                    Data = hall,
                    Message = "Hall updated successfully",
                    Succeeded = true,
                    Errors = null
                });
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "Error while updating the Hall",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(errorResponse);
            }
        }


        [HttpDelete]
        public IActionResult DeleteHall(int id)
        {
            try
            {
                HallService.Delete(id);

                return Ok(new CustomResponseDTO<object>
                {
                    Data = id,
                    Message = "Hall Deleted successfully",
                    Succeeded = true,
                    Errors = null
                });
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "Error while Deleting the Hall",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(errorResponse);
            }
        }
        [HttpGet("HallByID")]
        public IActionResult GetHallByID(int id)
        {

            var hall = HallService.GetHallById(id);
            if (!hall.Succeeded)
            {
                return NotFound(hall);
            }
            return Ok(hall);

        }
    }
}