using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScaleWorkerService
{
    internal class FaceImage
    {
        public string GatewayId { get; set; }
        public string FaceNo { get; set; }
        public string ImageType { get; set; }
        public string Image { get; set; }
        public long Ctime { get; set; }
    }
}
