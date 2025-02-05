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
    public class CarController : ControllerBase
    {
        private readonly ICarService carService;
        private readonly IHubContext<NotificationsHub> notificationHub;

        public CarController(ICarService _carService, IHubContext<NotificationsHub> notificationHub)
        {
            carService = _carService;
            this.notificationHub = notificationHub;
        }

        [HttpGet("Cars")]
        public IActionResult GetAllCars(int page = 1, int pageSize = 12, string priceRange = "all", int govId = 0, int cityId = 0)
        {
            try
            {
                string customerId = User.FindFirstValue("uid");
                var response = carService.GetAllCars(customerId, page, pageSize, priceRange, govId, cityId);



                if (response.Data == null || !response.Data.Any())
                {
                    return NotFound(new CustomResponseDTO<List<CarDTO>>
                    {
                        Data = null,
                        Message = "No Cars Found",
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
                    Message = "Error while retrieving the cars",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(errorResponse);
            }
        }

        [HttpPost("AddCar")]
        public async Task<ActionResult> AddCar(AddCarDTO carDto) // Use FromForm to handle file uploads
        {
            string OwnerID = User.FindFirstValue("uid");

            try
            {
                carDto.OwnerID = OwnerID;
                var car = await carService.AddCar(carDto);
                await notificationHub.Clients.All.SendAsync("newServicesAdded", "عربية");

                return Ok(new CustomResponseDTO<CarDTO>
                {
                    Data = car,
                    Message = "Car added successfully",
                    Succeeded = true,
                    Errors = null
                });
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "Error while adding the car",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(errorResponse);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditCar(CarDTO carDto, int id)
        {
            string OwnerID = User.FindFirstValue("uid");

            try
            {
                carDto.OwnerID = OwnerID;
                carDto.CarID = id;
                var car = await carService.EditCar(id, carDto);

                return Ok(new CustomResponseDTO<CarDTO>
                {
                    Data = car,
                    Message = "Car updated successfully",
                    Succeeded = true,
                    Errors = null
                });
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "Error while updating the car",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(errorResponse);
            }
        }

        [HttpDelete]
        public IActionResult DeleteCar(int id)
        {
            try
            {
                carService.Delete(id);

                return Ok(new CustomResponseDTO<object>
                {
                    Data = id,
                    Message = "Car Deleted successfully",
                    Succeeded = true,
                    Errors = null
                });
            }
            catch (Exception ex)
            {
                var errorResponse = new CustomResponseDTO<List<string>>
                {
                    Data = null,
                    Message = "Error while Deleting the car",
                    Succeeded = false,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("CarByID")]
        public IActionResult GetCarByID(int id)
        {

            var Car = carService.GetCarById(id);
            if (!Car.Succeeded)
            {
                return NotFound(Car);
            }
            return Ok(Car);

        }
    }
}
