using Application.DTOS;

namespace Application.Interfaces
{
    public interface IShopDressesService
    {

        CustomResponseDTO<List<ShopDressesDTo>> GetAllShopDresses(int page, int pageSize, int govId,int cityId);

        CustomResponseDTO<List<ShopDressesDTo>> GetShopDressesByName(string name);

        CustomResponseDTO<ShopDressesDTo> GetShopDressesById(int id);
        Task<CustomResponseDTO<ShopDressesDTo>> AddShopDress(AddShopDressDTO ShopDress);
        CustomResponseDTO<ShopDressesDTo> UpdateShopDress(ShopDressesDTo ShopDress, int id);

        CustomResponseDTO<ShopDressesDTo> DeleteShopDressById(int id);
    }
}
