// See https://aka.ms/new-console-template for more information


using pkhexEgglocke;
using System.Reflection;

class Program {

    public static void Main(string[] args)
    {

        var BLANK_SOULSILVER_SAVE = @"C:\Users\Evin Jaff\Downloads\johtocomplete.sav";

        SaveWriter sw = new SaveWriter(BLANK_SOULSILVER_SAVE);

        //EggCreator pc = new EggCreator();

        //sw.addEgg(pc, 1);

        // Output the new one 
        //sw.export("testPROG.sav");

        //Console.WriteLine("Dumped to testPROG.sav!!");
        var BLANK_GEN4_MAREEP_VALID = Path.Combine("C:\\Users\\Evin Jaff\\Documents\\egglocke-maker\\pkhex\\pkhex-egglocke-tests\\pkhex-egglocke-tests\\testSources", "Mareep.json");

        EggCreator ec = EggCreator.decodeJSON(BLANK_GEN4_MAREEP_VALID);

        sw.addEgg(ec, 1);

        sw.export("testPROG.sav");

        Console.WriteLine("Dumped testPROG");

    }

}
