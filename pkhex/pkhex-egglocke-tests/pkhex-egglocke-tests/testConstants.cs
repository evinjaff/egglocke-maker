using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace pkhexEgglockeTests
{
    public static class testConstants
    {
        // Gen 4 - Soul Silver Blank Save File
        public static string JOHTO_PLUS_SOUL_SILVER_SAVE => Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"testSources"), "emptySoulSilver.sav");
        public static string BLANK_SOULSILVER_OT_STRING => "Egg";

        // Gen 4 - Egg JSONs
        public static string BLANK_GEN4_MAREEP_VALID => Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"testSources"), "Mareep.json");
        public static string BLANK_GEN4_LEGENDARY_TRIO => Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"testSources"), "LegendaryTrio.json");


        // Gen 6 - Omega Ruby Save File
        public static string BLANK_OMEGARUBY_SAVE => Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"testSources"), "emptyOmegaRuby.sav");


        


    }
}
