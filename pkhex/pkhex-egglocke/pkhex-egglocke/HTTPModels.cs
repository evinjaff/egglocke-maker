using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pkhex_egglocke
{
    public class SaveFileGeneratorModel
    {
        public int generation { get; set; } = 4;
        public string jsonRaw { get; set; } = "";
        public dynamic[] eggs { get; set; } =Array.Empty<dynamic>();
        public uint gamecode { get; set; } = 0;

    }
}
