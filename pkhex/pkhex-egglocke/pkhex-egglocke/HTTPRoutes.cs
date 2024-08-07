﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grapevine;
using Newtonsoft.Json;
using PKHeX.Core;
using pkhexEgglocke;

namespace pkhex_egglocke
{
    [RestResource]
    public class MyResource: Resource
    {
        [RestRoute("Get", "/api/test")]
        public async Task Test(IHttpContext context)
        {
            await context.Response.SendResponseAsync("Successfully hit the test route!");
        }

        [RestRoute("Get", "/api/startfile")]
        public async Task StartFile(IHttpContext context)
        {
            await context.Response.SendResponseAsync("Successfully hit the startfile route!");
        }

        [RestRoute("Get", "/api/file1")]
        public async Task DownloadFile(IHttpContext context)
        {
            string filePath = @"support/Pokemon.mp3";

            context.Response.ContentType = "audio/mpeg";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=blank.mp3");
            context.Response.ContentLength64 = new FileInfo(filePath).Length;

            await context.Response.SendResponseAsync(File.ReadAllBytes(filePath));

        }

        public string getGameSavPath(uint gamecode) {

            switch (gamecode)
            {
                case 8:
                    return @"support/Complete_SoulSilver.sav";
                case 10:
                    return @"support/Blank_Diamond.sav";
                case 11:
                    return @"support/Blank_Pearl.sav";
                case 12:
                    return @"support/Blank_Platinum.sav";
                case 20:
                    return @"support/Blank_White.sav";
                case 21:
                    return @"support/Blank_Black.sav";
                case 23:
                    return @"support/Blank_Black2.sav";


                default:
                    throw new Exception("Invalid game code");
            }
        }

        [RestRoute("POST", "/api/buildSaveFile")]
        public async Task BuildSaveFile(IHttpContext context)
        {
            // Decode the input stream
            var model = await DeserializeAsync<SaveFileGeneratorModel>(context.Request.InputStream, context.CancellationToken);

            string gamePath = getGameSavPath(model.gamecode);

            // yields array of dynamic objects
            SaveWriter sw = new SaveWriter(gamePath);

            EggCreator[] eggArray = new EggCreator[model.eggs.Length];
            for (int i = 0; i < model.eggs.Length; i++) {
                EggCreator constructed_egg_creator = EggCreator.decodeJSON(model.eggs[i].ToString(), false);
                sw.addEgg(constructed_egg_creator, i+1);
            }

            byte[] saveFile = sw.exportRawBytes();

            // Prepare the response as a downloadable file
            context.Response.ContentType = "application/octet-stream";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=blank.sav");
            context.Response.ContentLength64 = saveFile.Length;

            await context.Response.SendResponseAsync(saveFile);

        }


    }
}
