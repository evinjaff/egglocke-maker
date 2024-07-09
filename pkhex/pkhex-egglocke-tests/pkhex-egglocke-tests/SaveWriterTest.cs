using PKHeX.Core;
using pkhexEgglocke;
using System.Reflection;

namespace pkhexEgglockeTests
{
    [TestClass]
    public class SaveWriterTest
    {


        [TestMethod]
        public void TestSaveImportConstructorDoesNotCrashOnValidFile()
        {

            SaveWriter sw = new SaveWriter(testConstants.JOHTO_PLUS_SOUL_SILVER_SAVE);

        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestSaveExportConstructorCrashesOnInvalidFile()
        {
            string bad_filepath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"testSources\dneSoulSilver.sav");
            SaveWriter sw = new SaveWriter(bad_filepath);

            
        }

        [TestMethod]
        public void TestSaveImportOTMatches()
        {
            string expectedOT = testConstants.BLANK_SOULSILVER_OT_STRING;
            SaveWriter sw = new SaveWriter(testConstants.JOHTO_PLUS_SOUL_SILVER_SAVE);
            string actualOT = sw.getOTString();

            Assert.AreEqual(expectedOT, actualOT);
            

        }

        [TestMethod]
        public void TestSaveImportVersionMatches()
        {
            string expectedOT = testConstants.BLANK_SOULSILVER_OT_STRING;
            SaveWriter sw = new SaveWriter(testConstants.JOHTO_PLUS_SOUL_SILVER_SAVE);
            string actualOT = sw.getOTString();

            Assert.AreEqual(expectedOT, actualOT);


        }

        [TestMethod]
        public void TestMassAddWorks()
        {
            SaveWriter sw = new SaveWriter(testConstants.JOHTO_PLUS_SOUL_SILVER_SAVE);

            sw.massAddEggs(testConstants.BLANK_GEN4_LEGENDARY_TRIO);


        }

        [TestMethod]
        public void TestNaturesSetCorrectifNotShiny()
        {
            SaveWriter sw = new SaveWriter(testConstants.JOHTO_PLUS_SOUL_SILVER_SAVE);

            EggCreator ec = EggCreator.decodeJSON(testConstants.BLANK_GEN4_MAREEP_VALID, true);

            sw.addEgg(ec, 1);

            // get egg at box index 1
            IList<PKM> boxData = sw.getBox();

            Nature expectedNature = Nature.Adamant;

            Assert.AreEqual(expectedNature, boxData[1].Nature);

        }

        [TestMethod]
        public void TestNaturesSetCorrectifShiny()
        {
            SaveWriter sw = new SaveWriter(testConstants.JOHTO_PLUS_SOUL_SILVER_SAVE);

            EggCreator ec = EggCreator.decodeJSON(testConstants.BLANK_GEN4_SHINY_MAREEP_VALID, true);

            sw.addEgg(ec, 1);

            // get egg at box index 1
            IList<PKM> boxData = sw.getBox();

            Nature expectedNature = Nature.Adamant;

            Assert.AreEqual(expectedNature, boxData[1].Nature);

        }

        [TestMethod]
        public void TestShinyCorrect()
        {
            SaveWriter sw = new SaveWriter(testConstants.JOHTO_PLUS_SOUL_SILVER_SAVE);

            EggCreator ec = EggCreator.decodeJSON(testConstants.BLANK_GEN4_SHINY_MAREEP_VALID, true);

            sw.addEgg(ec, 1);

            // get egg at box index 1
            IList<PKM> boxData = sw.getBox();

            bool isShiny = boxData[1].IsShiny;

            Assert.IsTrue(isShiny);


        }

        [TestMethod]
        public void TestNotShinyCorrect()
        {
            SaveWriter sw = new SaveWriter(testConstants.JOHTO_PLUS_SOUL_SILVER_SAVE);

            EggCreator ec = EggCreator.decodeJSON(testConstants.BLANK_GEN4_MAREEP_VALID, true);

            sw.addEgg(ec, 1);

            // get egg at box index 1
            IList<PKM> boxData = sw.getBox();

            bool isShiny = boxData[1].IsShiny;

            Assert.IsFalse(isShiny);


        }





    }
}