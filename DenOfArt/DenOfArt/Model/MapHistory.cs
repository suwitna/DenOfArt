using System;
using System.Collections.Generic;
using System.Text;

namespace DenOfArt.Model
{
    class MapHistory
    {
        public string LoginName { get; set; }
        public string Accuracy { get; set; }
        public string GeoLatitude { get; set; }
        public string GeoLongitude { get; set; }
        public string PinType { get; set; }
        public string PinLabel { get; set; }
        public string PinAddress { get; set; }
        public DateTime SaveTime { get; set; }
    }
}