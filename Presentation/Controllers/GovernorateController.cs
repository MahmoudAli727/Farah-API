using Application.DTOS;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GovernorateController : ControllerBase
    {
        private readonly IGovernorateServices governorateServices;

        public GovernorateController(IGovernorateServices governorateServices)
        {
            this.governorateServices = governorateServices;
        }


        [HttpGet]
        public IActionResult GetAllGovernorate()
        {
            try
            {
                List<GovernorateDTO> governorateDTOs = governorateServices.GetAll();

                if (governorateDTOs == null || governorateDTOs.Count == 0)
                {
                    var customResponse = new CustomResponseDTO<GovernorateDTO>
                    {
                        Succeeded = true,
                        Message = "No governorates found",
                        Data = null,
                        Errors = null
                    };
                    return NotFound(customResponse);
                }

                var customResponseWithData = new CustomResponseDTO<List<GovernorateDTO>>
                {
                    Succeeded = true,
                    Message = "Governorates successfully retrieved",
                    Data = governorateDTOs,
                    Errors = null
                };
                return Ok(customResponseWithData);
            }
            catch (Exception ex)
            {
                var customErrorResponse = new CustomResponseDTO<GovernorateDTO>
                {
                    Succeeded = false,
                    Message = "An error occurred while retrieving governorates",
                    Data = null,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(customErrorResponse);
            }
        }



        [HttpGet("{id:int}")]
        public IActionResult GetGovernorateById(int id)
        {
            try
            {
                GovernorateDTO governorateDTO = governorateServices.GetById(id);

                if (governorateDTO == null)
                {
                    var errorResponse = new CustomResponseDTO<GovernorateDTO>
                    {
                        Succeeded = false,
                        Message = $"Governorate with ID {id} not found",
                        Data = null,
                        Errors = null
                    };
                    return NotFound(errorResponse);
                }

                var customResponseDTO = new CustomResponseDTO<GovernorateDTO>
                {
                    Succeeded = true,
                    Message = "Governorate successfully retrieved",
                    Data = governorateDTO,
                    Errors = null
                };
                return Ok(customResponseDTO);
            }
            catch (Exception ex)
            {
                var customErrorResponse = new CustomResponseDTO<GovernorateDTO>
                {
                    Succeeded = false,
                    Message = "An error occurred while retrieving the governorate",
                    Data = null,
                    Errors = new List<string> { ex.Message }
                };
                return BadRequest(customErrorResponse);
            }
        }


    }
}
