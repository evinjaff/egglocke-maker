using pkhexEgglocke;

namespace pkhexEgglockeTests
{
    [TestClass]
    public class SaveWriterTest
    {


        [TestMethod]
        public void TestSaveImportConstructorDoesNotCrash()
        {

            SaveWriter sw = new SaveWriter(testConstants.BLANK_SOULSILVER_SAVE);

        }

        [TestMethod]
        public void TestSaveImportOTMatches()
        {
            string expectedOT = testConstants.BLANK_SOULSILVER_OT_STRING;
            SaveWriter sw = new SaveWriter(testConstants.BLANK_SOULSILVER_SAVE);
            string actualOT = sw.getOTString();

            Assert.AreEqual(expectedOT, actualOT);
            

        }


    }
}