using Application.DTOS;

namespace Application.Interfaces
{
    public interface IGovernorateServices
    {
        public List<GovernorateDTO> GetAll();
        public GovernorateDTO GetById(int governorateId);
    }
}
