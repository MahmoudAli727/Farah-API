using Application.DTOS;

namespace Application.Interfaces
{
    public interface IPhotographyService
    {
        CustomResponseDTO<List<PhotographyDTO>> GetAllPhotographer(string customerId,int page, int pageSize);


        CustomResponseDTO<PhotographyDTO> GetPhotographerById(int id);
        Task<CustomResponseDTO<AddPhotographyDTO>> AddPhotographer(AddPhotographyDTO PhotographerDTO);
        Task<CustomResponseDTO<AddPhotographyDTO>> UpdatePhotographer(int id, AddPhotographyDTO PhotographerDto);

        CustomResponseDTO<AddPhotographyDTO> DeletePhotographerById(int id);
    }
}
