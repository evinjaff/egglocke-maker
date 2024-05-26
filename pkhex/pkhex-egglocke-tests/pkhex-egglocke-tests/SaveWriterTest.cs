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


    }
}