// See https://aka.ms/new-console-template for more information


using PKHeX.Core;
using pkhexEgglocke;
using System.Reflection;
using Grapevine;
using System;



class Program {

    public static void Main(string[] args)
    {

        RestServerBuilder restServerBuilder = RestServerBuilder.UseDefaults();

        // host on 0.0.0.0


        // Spin up the REST server
        using (var server = RestServerBuilder.UseDefaults().Build())
        {
            server.Prefixes.Add("http://*:1234/");

            server.Start();

            Console.WriteLine("Press enter to stop the server");
            Console.ReadLine();

            // block the main thread forever
            while (true) { }

        }
        

        var BLANK_SOULSILVER_SAVE = @"C:\Users\Evin Jaff\Downloads\johtocomplete.sav";

        // SaveWriter sw = new SaveWriter(BLANK_SOULSILVER_SAVE);

        //EggCreator pc = new EggCreator();

        //sw.addEgg(pc, 1);

        // Output the new one 
        //sw.export("testPROG.sav");

        //Console.WriteLine("Dumped to testPROG.sav!!");
        var BLANK_GEN4_MAREEP_VALID = Path.Combine("C:\\Users\\Evin Jaff\\Documents\\egglocke-maker\\pkhex\\pkhex-egglocke-tests\\pkhex-egglocke-tests\\testSources", "Mareep.json");
        var BLANK_GEN4_LEGENDARY_TRIO = Path.Combine("C:\\Users\\Evin Jaff\\Documents\\egglocke-maker\\pkhex\\pkhex-egglocke-tests\\pkhex-egglocke-tests\\testSources", "LegendaryTrio.json");

        // EggCreator ec = EggCreator.decodeJSON(BLANK_GEN4_MAREEP_VALID, true);

        // sw.massAddEggs(BLANK_GEN4_LEGENDARY_TRIO);

        // sw.export("testPROG.sav");

        // Console.WriteLine("Dumped testPROG");

    }

}
