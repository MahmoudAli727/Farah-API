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
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly IAccountService _accountService;
        private readonly IGoogleAuthService googleAuthService;
        private readonly IHubContext<NotificationsHub> notificationHub;
        private readonly IAdminService AdminService;
        public AccountController(UserManager<ApplicationUser> _userManager, IConfiguration _config, IAccountService accountService, IGoogleAuthService googleAuthService,
            IHubContext<NotificationsHub> notificationHub, IAdminService adminService)
        {
            userManager = _userManager;
            config = _config;
            _accountService = accountService;
            this.googleAuthService = googleAuthService;
            this.notificationHub = notificationHub;
            AdminService = adminService;
        }

        [HttpGet("OwnerServices")]
        public IActionResult GetOwnerServices()
        {
            string ownerID = User.FindFirstValue("uid");

            try
            {
                var ownerServices = _accountService.GetOwnerServices(ownerID);
                return Ok(new CustomResponseDTO<AllServicesDTO>()
                {
                    Data = ownerServices,
                    Message = "Services retrieved Successfully",
                    Succeeded = true,
                    Errors = null,
                    PaginationInfo = null,

                });
            }
            catch
            (Exception ex)
            {
                return BadRequest(new CustomResponseDTO<AllServicesDTO>()
                {
                    Data = null,
                    Message = ex.Message,
                    Succeeded = false,
                    Errors = new List<string> { ex.Message },
                    PaginationInfo = null,

                });
            }
        }

        [HttpPost("ownerRegister")]
        public async Task<ActionResult> Register([FromForm] OwnerRegisterDTO ownerRegisterModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _accountService.OwnerRegisterAsync(ownerRegisterModel);

            if (response.Succeeded)
            {
                await notificationHub.Clients.All.SendAsync("newOwnerRegister");
                return Ok(response);
            }
            else
            {
                return BadRequest(new { message = response.Message, errors = response.Errors });
            }
        }

        [HttpPost("customerRegister")]
        public async Task<ActionResult> Register(CustomerRegisterDTO customerRegisterModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _accountService.CustomerRegisterAsync(customerRegisterModel);

            if (response.Succeeded)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(new { message = response.Message, errors = response.Errors });
            }
        }


        [HttpPost("confirmEmail")]
        public async Task<ActionResult> ConfirmEmail(string otp)
        {
            string Email = User.FindFirstValue(ClaimTypes.Email);

            var result = await _accountService.ConfirmEmailAsync(Email, otp);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPost("forgetPassword")]
        public async Task<ActionResult> ForgetPassword(string Email)
        {
            var result = await _accountService.ForgetPassword(Email);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }
        //[HttpPost("createNewPassword")]
        //public async Task<ActionResult> CreateNewPassword(string Email)
        //{

        //}

        [HttpGet("resendOTP")]
        public async Task<ActionResult> GetNewOTP()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                return BadRequest(new { message = "User Not Found , Please Login" });

            }
            var result = await _accountService.SendNewOTPAsync(email);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserDTO loginUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _accountService.Login(loginUserModel);

            if (response.Succeeded)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(new { message = response.Message, errors = response.Errors });
            }
        }


        [HttpPost("changePassword")]
        [Authorize]
        public async Task<ActionResult> ChangePassword(ChangePasswordDTO changePasswordDto)
        {

            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                return BadRequest(new { message = "User Not Found , Please Login" });

            }
            var response = await _accountService.ChangePasswordAsync(changePasswordDto, email);

            if (response.Succeeded)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(new { message = response.Message, errors = response.Errors });
            }
        }


        [HttpGet("getOwnerInfo")]
        public async Task<ActionResult> GetOwnerProfileInfo()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            if (Email == null)
            {
                return BadRequest(new { message = "البريد الإلكتروني غير متوفر ." });
            }

            var response = await _accountService.GetOwnerInfo(Email);
            if (!response.Succeeded)
            {
                return BadRequest(new { message = response.Message });
            }

            return Ok(response);
        }

        [HttpHead("getOwnerInfo")]

        public async Task<IActionResult> HeadOwnerProfileInfo()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                return BadRequest(new { message = "البريد الإلكتروني غير متوفر ." });
            }

            var response = await _accountService.GetOwnerInfo(email);
            if (!response.Succeeded)
            {
                return BadRequest(new { message = response.Message });
            }

            Response.Headers.Add("X-Owner-Info", "Available");
            Response.Headers.Add("X-Owner-Info-Status", "Complete");

            return NoContent();
        }


        [HttpPut("updateOwnerInfo")]
        public async Task<ActionResult> UpdateOwnerInfo([FromForm] OwnerAccountInfoDTO updateDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                return BadRequest("لم يتم العثور على البريد الإلكتروني للمستخدم.");
            }

            var response = await _accountService.UpdateOwnerInfo(updateDto, email);
            if (!response.Succeeded)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }

        [HttpGet("getCustomerInfo")]
        public async Task<ActionResult> GetCustomerInfo()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                return BadRequest(new { message = "البريد الإلكتروني غير متوفر." });
            }

            var response = await _accountService.GetCustomerInfo(email);
            if (!response.Succeeded)
            {
                return BadRequest(new { message = response.Message });
            }

            return Ok(response);
        }

        [HttpPut("updateCustomerInfo")]
        public async Task<ActionResult> UpdateCustomerInfo([FromForm] CustomerAccountInfoDTO updateDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                return BadRequest("لم يتم العثور على البريد الإلكتروني للمستخدم.");
            }
            var response = await _accountService.UpdateCustomerInfo(updateDto, email);
            if (!response.Succeeded)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }

        [HttpPost("googleLogin")]
        public async Task<ActionResult> GoogleSignIn([FromBody] GoogleTokenDTO googleTokenDto)
        {
            var result = await googleAuthService.GoogleSignIn(googleTokenDto.googleToken);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        public class GoogleTokenDTO
        {
            public string googleToken { get; set; }
        }


    }
}
