using Application.DTOS;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityServices cityServices;

        public CityController(ICityServices cityServices)
        {
            this.cityServices = cityServices;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetAllCities(int id)
        {
            try
            {
                List<CityDTO> cityDTOs = cityServices.GetAll(id);

                if (cityDTOs == null || cityDTOs.Count == 0)
                {
                    var customResponse = new CustomResponseDTO<List<CityDTO>>
                    {
                        Succeeded = false,
                        Message = $"No cities found for governorate with ID {id}",
                        Data = null,
                        Errors = null
                    };
                    return NotFound(customResponse);
                }
                var customResponseWithData = new CustomResponseDTO<List<CityDTO>>
                {
                    Succeeded = true,
                    Message = "Cities successfully retrieved",
                    Data = cityDTOs,
                    Errors = null
                };
                return Ok(customResponseWithData);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
                var customErrorResponse = new CustomResponseDTO<CityDTO>
                {
                    Succeeded = false,
                    Message = "An error occurred while processing the request",
                    Data = null,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(customErrorResponse);
            }
        }

        [HttpGet("getCityById")]
        public IActionResult GetCityById(int id)
        {
            try
            {
                CityDTO city = cityServices.GetById(id);

                if (city == null)
                {
                    var customResponse = new CustomResponseDTO<CityDTO>
                    {
                        Succeeded = false,
                        Message = $"No cities found for governorate with ID {id}",
                        Data = null,
                        Errors = null
                    };
                    return NotFound(customResponse);
                }
                var customResponseWithData = new CustomResponseDTO<CityDTO>
                {
                    Succeeded = true,
                    Message = "Cities successfully retrieved",
                    Data = city,
                    Errors = null
                };
                return Ok(customResponseWithData);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
                var customErrorResponse = new CustomResponseDTO<CityDTO>
                {
                    Succeeded = false,
                    Message = "An error occurred while processing the request",
                    Data = null,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(customErrorResponse);
            }
        }

    }
}
