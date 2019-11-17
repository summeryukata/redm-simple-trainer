using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    class Commands : BaseScript
    {
        /*static string texDic = "";
        static string texName = "";

        public static async Task DrawTexture()
        {
            float x = 0.3f;
            float y = 0.3f;

            const float height = 1080f;
            float ratio = 16f/9f;
            var width = height * ratio;

            float newWidth = x / width * 100;
            float newHeight = y / height * 100;

            Debug.WriteLine($"sizex: {newWidth} sizey: {newHeight}");

            Drawing.DrawTexture(texDic, texName, 0.5f, 0.5f, newWidth, newHeight, 0, 255, 255, 255, 255, true);
        }

        static ulong[] natives = new ulong[]
        {
            0xBB697756309D77EE,
            0x2C5BD9A43987AA27,
            0x0CF3A965906452031,
            0x62BE3ECC79FBD004,
            0x1F13D5AE5CB17E17,
            0x0AA81B5F10BC43AC2,
            0x0CD9AB83489430EA,
            0x0D2BA051B94CA9BCC,
            0x0CB215C4B56A7FAE7,
            0x12B6281B6C6706C0,
            0x756C7B4C43DF0422,
            0x501D52D24EA8934,
            0x0A1AF16083320065A,
        };

        [Command("drawtex")]
        async Task DrawTex(string[] args)
        {
            texDic = args[0];
            texName = args[1];
        }*/

        [Command("cl_drawFps", Restricted = false)]
        public async Task DrawFps(string[] args)
        {
            void ShowUsage()
            {
                Debug.WriteLine("usage: cl_drawFps [value]");
                Debug.WriteLine("possible values: 1(true)/0(false)");
            }

            if (args.Count() < 1 || args.Count() > 1)
            {
                ShowUsage();
                return;
            }

            string lower = args[0].ToLowerInvariant();

            if (lower == "1" || lower == "true")
            {
                frameCount = 0;
                lastTime = DateTime.Now;
                lastFps = 0;
                Tick += FpsTick;
                return;
            }
            if (lower == "0" || lower == "false")
            {
                Tick -= FpsTick;
                return;
            }

            ShowUsage();
            await Task.FromResult(0);
        }

        static int frameCount;
        static DateTime lastTime;
        static int lastFps;

        static async Task FpsTick()
        {
            frameCount++;

            if ((DateTime.Now - lastTime).TotalSeconds >= 1)
            {
                lastFps = frameCount;
                frameCount = 0;
                lastTime = DateTime.Now;
            }

            Drawing.DrawText($"FPS: {lastFps}", Drawing.g_optionsFont, 0.01f, 0.01f, 0.3f, 0.3f, 255, 255, 255, 255);

            await Task.FromResult(0);
        }
    }
}
