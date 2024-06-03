using pkhexEgglocke;
using pkhexEgglockeTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pkhexEgglockeTests
{
    [TestClass]
    public class EggCreatorTests
    {
        [TestMethod]
        public void TestDecoder()
        {
            SaveWriter sw = new SaveWriter(testConstants.JOHTO_PLUS_SOUL_SILVER_SAVE);

            EggCreator ec = EggCreator.decodeJSON(testConstants.BLANK_GEN4_MAREEP_VALID, true);

            sw.addEgg(ec, 1);

            sw.export("testPROG.sav");
        
        }


    }
}
