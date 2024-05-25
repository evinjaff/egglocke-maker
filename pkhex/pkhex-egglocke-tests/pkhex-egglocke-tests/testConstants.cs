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
        public static string BLANK_SOULSILVER_SAVE => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"testSources\emptySoulSilver.sav");
        public static string BLANK_SOULSILVER_OT_STRING => "Egg";


        // Gen 6 - Omega Ruby Save File
        public static string BLANK_OMEGARUBY_SAVE => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"testSources\emptyOmegaRuby.sav");


    }
}
