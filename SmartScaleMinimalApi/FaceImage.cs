using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartScaleMinimalApi
{
    internal class FaceImage
    {
        public string Id { get; set; }
        public string GatewayId { get; set; }
        public string FaceNo { get; set; }
        public string ImageType { get; set; }
        public string Image { get; set; }
        public long Ctime { get; set; }
    }
}
