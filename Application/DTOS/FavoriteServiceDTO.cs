using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS
{
    public class FavoriteServiceDTO
    {
        public string CustomerID { get; set; }
        public List<BeautyCenterDTO> BeautyCenters { get; set; }
        public List<HallDTO> Halls { get; set; }
        public List<CarDTO> Cars { get; set; }
        public List<PhotographyDTO> Photographys { get; set; }
        public List<ShopDressesDTo> ShopDresses { get; set; }
    }
}
