using Application.DTOS;
using Application.Interfaces;
using AutoMapper;
using Core.Entities;

namespace Application.Services
{
    public class GovernorateServices : IGovernorateServices
    {
        private readonly IGovernorateRepository governorateRepository;
        private readonly IMapper mapper;

        public GovernorateServices(
            IGovernorateRepository governorateRepository,
            IMapper mapper
        )
        {
            this.governorateRepository = governorateRepository;
            this.mapper = mapper;
        }

        public List<GovernorateDTO> GetAll()
        {
            List<Governorate> governoratesList = governorateRepository.GetAll();
            List<GovernorateDTO> governoratesDTO = mapper.Map<List<GovernorateDTO>>(governoratesList);
            return governoratesDTO;
        }

        public GovernorateDTO GetById(int governorateId)
        {
            Governorate governorate = governorateRepository.GetById(governorateId);
            GovernorateDTO governorateDTO = mapper.Map<GovernorateDTO>(governorate);
            return governorateDTO;
        }
    }
}
