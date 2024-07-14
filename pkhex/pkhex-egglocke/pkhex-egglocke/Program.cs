// See https://aka.ms/new-console-template for more information


using PKHeX.Core;
using pkhexEgglocke;
using System.Reflection;
using Grapevine;
using System;



class Program {

    public static void Main(string[] args)
    {

        // See if there is an -api flag in the args
        // If there is, start the API server
        // If not, start the CLI
        if (args.Length == 0)
        {
            Console.WriteLine("No mode set, provide one of the following flags: -api, -cli");

            args = new string[] { Console.ReadLine() };
        }
            // host on

        //
        if (args.Length > 0 && args[0] == "-api")
        {
            Console.WriteLine("Starting API server");

            RestServerBuilder restServerBuilder = RestServerBuilder.UseDefaults();

            // host on 0.0.0.0
            // Spin up the REST server
            using (var server = RestServerBuilder.UseDefaults().Build())
            {
                server.Prefixes.Add("http://+:1235/");

                server.Start();

                Console.WriteLine("Press enter to stop the server");
                Console.ReadLine();

                // block the main thread forever
                while (true) { }

            }

        }
        else
        {
            Console.WriteLine("Starting CLI");


            // yields array of dynamic objects
            SaveWriter sw = new SaveWriter(@"support/Complete_SoulSilver.sav");

            String test = "{\r\n        \"dexNumber\": 483,\r\n        \"ball\": 2,\r\n        \"language\": 1,\r\n        \"ability\": 28,\r\n        \"nature\": 12,\r\n        \"OT\": \"Dawn\",\r\n        \"OTGender\": 1,\r\n        \"nickname\": \"Dialga\",\r\n        \"IV\": [ 31, 31, 31, 31, 31, 31 ],\r\n        \"EV\": [ 0, 0, 0, 0, 0, 0 ],\r\n        \"moves\": [ 425, 262 ],\r\n        \"movespp\": [ 30, 40 ],\r\n        \"heldItem\": 12,\r\n        \"isShiny\": false\r\n\r\n    }";

            EggCreator[] eggArray = new EggCreator[1];
            for (int i = 0; i < eggArray.Length; i++)
            {
                EggCreator constructed_egg_creator = EggCreator.decodeJSON(test, false);
                sw.addEgg(constructed_egg_creator, i + 1);
            }

            byte[] saveFile = sw.exportRawBytes();

            // dump to file
            sw.export("testPROG.sav");


        }
        

        

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
