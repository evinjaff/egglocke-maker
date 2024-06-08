using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grapevine;
using PKHeX.Core;

namespace pkhex_egglocke
{
    [RestResource]
    public class MyResource
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
            string filePath = "C:\\Users\\Evin Jaff\\Downloads\\Pokemon.mp3";

            context.Response.ContentType = "audio/mpeg";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=blank.mp3");
            context.Response.ContentLength64 = new FileInfo(filePath).Length;

            await context.Response.SendResponseAsync(File.ReadAllBytes(filePath));

        }

    }
}
