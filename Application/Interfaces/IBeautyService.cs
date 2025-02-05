using Application.DTOS;

namespace Application.Interfaces
{
    public interface IBeautyService
    {
        public CustomResponseDTO<List<ServiceForBeautyCenterDTO>> AddBeautyService(List<ServiceForBeautyCenterDTO> beautyDTO);
        public CustomResponseDTO<List<BeautyCenterDTO>> GetAllBeautyCenters(string customerId, int page, int pageSize, int govId, int cityId);
        public CustomResponseDTO<List<BeautyCenterDTO>> GetBeautyCenterByName(string name);
        public CustomResponseDTO<BeautyCenterDTO> GetBeautyCenterById(int id);
        public Task<CustomResponseDTO<BeautyCenterDTO>> AddBeautyCenter(AddBeautyCenterDTO beautyCenterDTO);
        public Task<CustomResponseDTO<AddBeautyCenterDTO>> UpdateBeautyCenter(AddBeautyCenterDTO beautyCenterDTO, int id);
        public CustomResponseDTO<AddBeautyCenterDTO> DeleteBeautyCenterById(int id);
        public CustomResponseDTO<ServiceForBeautyCenterDTO> RemoveBeautyService(int beautyID, int ServiceID);
    }
}
