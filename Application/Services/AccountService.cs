using Application.DTOS;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Core.Entities;

namespace Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        private readonly IBeautyRepository _beautyCenterRepository;
        private readonly IHallRepository _hallRepository;
        private readonly ICarRepository _carRepository;
        private readonly IPhotographyRepository _photographyRepository;
        private readonly IShopDressesRepository _shopDressesRepository;

        public AccountService(IAccountRepository accountRepository, IMapper mapper,
            IBeautyRepository beautyCenterRepository,
            IHallRepository hallRepository,
            ICarRepository carRepository,
            IPhotographyRepository photographyRepository,
            IShopDressesRepository shopDressesRepository)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _beautyCenterRepository = beautyCenterRepository;
            _hallRepository = hallRepository;
            _carRepository = carRepository;
            _photographyRepository = photographyRepository;
            _shopDressesRepository = shopDressesRepository;
        }
        public AllServicesDTO GetOwnerServices(string ownerID)
        {
            List<BeautyCenter> beauty = _beautyCenterRepository.GetOwnerServices(ownerID);
            List<Hall> halls = _hallRepository.GetOwnerServices(ownerID);
            List<Car> cars = _carRepository.GetOwnerServices(ownerID);
            List<Photography> photographies = _photographyRepository.GetOwnerServices(ownerID);
            List<ShopDresses> shopDresses = _shopDressesRepository.GetOwnerServices(ownerID);

            var compositeDto = new AllServicesDTO
            {
                BeautyCenters = _mapper.Map<List<BeautyCenterDTO>>(beauty),
                Halls = _mapper.Map<List<HallDTO>>(halls),
                Cars = _mapper.Map<List<CarDTO>>(cars),
                Photographys = _mapper.Map<List<PhotographyDTO>>(photographies),
                ShopDresses = _mapper.Map<List<ShopDressesDTo>>(shopDresses)
            };
            return compositeDto;

        }
        public async Task<CustomResponseDTO<AuthUserDTO>> OwnerRegisterAsync(OwnerRegisterDTO registerModel)
        {
            var owner = _mapper.Map<Owner>(registerModel);
            var IDBackImage = await ImageSavingHelper.SaveOneImageAsync(registerModel.IDBackImageFile, "OwnersImages");
            var IDFrontImage = await ImageSavingHelper.SaveOneImageAsync(registerModel.IDFrontImageFile, "OwnersImages");
            var profilePic = await ImageSavingHelper.SaveOneImageAsync(registerModel.ProfileImageFile, "OwnersImages");
            owner.IDBackImage = IDBackImage;
            owner.IDFrontImage = IDFrontImage;
            owner.ProfileImage = profilePic;
            var registrationResult = await _accountRepository.OwnerRegisterAsync(owner, registerModel);

            return new CustomResponseDTO<AuthUserDTO>
            {
                Data = registrationResult,
                Message = registrationResult.Message,
                Succeeded = registrationResult.Succeeded,
                Errors = registrationResult.Errors
            };
        }

        public async Task<CustomResponseDTO<AuthUserDTO>> CustomerRegisterAsync(CustomerRegisterDTO registerModel)
        {
            var customer = _mapper.Map<Customer>(registerModel);
            customer.ProfileImage = "/images/CustomersImages/default-customer-Image.svg";
            var registrationResult = await _accountRepository.CustomerRegisterAsync(customer, registerModel);

            return new CustomResponseDTO<AuthUserDTO>
            {
                Data = registrationResult,
                Message = registrationResult.Message,
                Succeeded = registrationResult.Succeeded,
                Errors = registrationResult.Errors
            };
        }

        public async Task<CustomResponseDTO<AuthUserDTO>> ConfirmEmailAsync(string email, string otp)
        {
            var result = await _accountRepository.ConfirmEmailAsync(email, otp);

            return new CustomResponseDTO<AuthUserDTO>
            {
                Data = result.IsEmailConfirmed ? result : null,
                Message = result.Message,
                Succeeded = result.Succeeded,
                Errors = result.Errors
            };
        }
        public async Task<CustomResponseDTO<string>> SendNewOTPAsync(string email)
        {
            var result = await _accountRepository.SendNewOTPAsync(email);
            if (result)
            {
                return new CustomResponseDTO<string>
                {
                    Data = "OTP sent successfully",
                    Succeeded = true
                };
            }
            else
            {
                return new CustomResponseDTO<string>
                {
                    Message = "Failed to send OTP",
                    Succeeded = false
                };
            }
        }


        public async Task<CustomResponseDTO<AuthUserDTO>> Login(LoginUserDTO loginUser)
        {
            var LoginResult = await _accountRepository.Login(loginUser);

            return new CustomResponseDTO<AuthUserDTO>
            {
                Data = LoginResult,
                Message = LoginResult.Message,
                Succeeded = LoginResult.Succeeded,
                Errors = LoginResult.Errors

            };
        }

        public async Task<CustomResponseDTO<string>> ForgetPassword(string Email)
        {
            var result = await _accountRepository.ForgetPassword(Email);
            if (result)
            {
                return new CustomResponseDTO<string>()
                {
                    Data = "Reset Password Link sent successfully",
                    Succeeded = true
                };
            }
            return new CustomResponseDTO<string>()
            {
                Data = "Failed to send THe Reset Password Link",
                Succeeded = false
            };
        }

        public async Task<CustomResponseDTO<bool>> ChangePasswordAsync(ChangePasswordDTO changePasswordModel, string userEmail)
        {
            var result = await _accountRepository.ChangePasswordAsync(userEmail, changePasswordModel);

            return new CustomResponseDTO<bool>
            {
                Data = result.Succeeded,
                Message = result.Succeeded ? "Password changed successfully" : "Password change failed",
                Succeeded = result.Succeeded,
                Errors = result.Succeeded ? null : result.Errors.Select(e => e.Description).ToList()
            };
        }


        public async Task<CustomResponseDTO<OwnerAccountInfoDTO>> GetOwnerInfo(string email)
        {
            var result = await _accountRepository.GetOwnerInfo(email);
            if (result == null)
            {
                return new CustomResponseDTO<OwnerAccountInfoDTO>()
                {
                    Data = null,
                    Succeeded = false,
                    Message = "حدث خطأ أثناء استرداد معلومات المالك. يرجى التحقق من البريد الإلكتروني والمحاولة مرة أخرى."
                };
            }

            var response = _mapper.Map<OwnerAccountInfoDTO>(result);
            return new CustomResponseDTO<OwnerAccountInfoDTO>()
            {
                Data = response,
                Succeeded = true,
                Message = "تم استرداد معلومات المالك بنجاح."
            };
        }
        public async Task<CustomResponseDTO<CustomerAccountInfoDTO>> GetCustomerInfo(string email)
        {
            var result = await _accountRepository.GetCustomerInfo(email);
            if (result == null)
            {
                return new CustomResponseDTO<CustomerAccountInfoDTO>()
                {
                    Data = null,
                    Succeeded = false,
                    Message = "حدث خطأ أثناء استرداد معلومات العميل. يرجى التحقق من البريد الإلكتروني والمحاولة مرة أخرى."
                };
            }

            var response = _mapper.Map<CustomerAccountInfoDTO>(result);
            return new CustomResponseDTO<CustomerAccountInfoDTO>()
            {
                Data = response,
                Succeeded = true,
                Message = "تم استرداد معلومات العميل بنجاح."
            };
        }

        public async Task<CustomResponseDTO<CustomerAccountInfoDTO>> UpdateCustomerInfo(CustomerAccountInfoDTO infoDTO, string email)
        {
            var customer = await _accountRepository.GetCustomerInfo(email);

            if (customer == null)
            {
                return new CustomResponseDTO<CustomerAccountInfoDTO>
                {
                    Data = null,
                    Succeeded = false,
                    Message = "لم يتم العثور على العميل بهذا البريد الإلكتروني."
                };
            }

            _mapper.Map(infoDTO, customer);

            string newProfileImage;
            if (infoDTO.SetNewProfileImage != null)
            {
                newProfileImage = await ImageSavingHelper.SaveOneImageAsync(infoDTO.SetNewProfileImage, "CustomersImages");
                customer.ProfileImage = newProfileImage;
            }
            var result = await _accountRepository.UpdateCustomerInfo(customer);

            if (!result)
            {
                return new CustomResponseDTO<CustomerAccountInfoDTO>
                {
                    Data = null,
                    Succeeded = false,
                    Message = "فشل تحديث معلومات العميل."
                };
            }
            var returnDTO = _mapper.Map<CustomerAccountInfoDTO>(customer);
            return new CustomResponseDTO<CustomerAccountInfoDTO>
            {
                Data = returnDTO,
                Succeeded = true,
                Message = "تم تحديث معلومات العميل بنجاح."
            };
        }

        public async Task<CustomResponseDTO<OwnerAccountInfoDTO>> UpdateOwnerInfo(OwnerAccountInfoDTO infoDTO, string Email)
        {

            var owner = await _accountRepository.GetOwnerInfo(Email);

            if (owner == null)
            {
                return new CustomResponseDTO<OwnerAccountInfoDTO>
                {
                    Data = null,
                    Succeeded = false,
                    Message = "لم يتم العثور على المالك بهذا البريد الإلكتروني."
                };
            }

            _mapper.Map(infoDTO, owner);

            string newPrfileImag;
            if (infoDTO.SetNewProfileImage != null)
            {
                newPrfileImag = await ImageSavingHelper.SaveOneImageAsync(infoDTO.SetNewProfileImage, "OwnersImages");
                owner.ProfileImage = newPrfileImag;
            }
            var result = await _accountRepository.UpdateOwnerInfo(owner);

            if (!result)
            {
                return new CustomResponseDTO<OwnerAccountInfoDTO>
                {
                    Data = null,
                    Succeeded = false,
                    Message = "فشل تحديث معلومات المالك."
                };
            }
            var ReturnDTO = _mapper.Map<OwnerAccountInfoDTO>(owner);
            return new CustomResponseDTO<OwnerAccountInfoDTO>
            {
                Data = ReturnDTO,
                Succeeded = true,
                Message = "تم تحديث معلومات المالك بنجاح."
            };
        }


    }


}
