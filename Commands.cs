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

            Drawing.DrawText($"FPS: {lastFps}", 1, 0.01f, 0.01f, 0.3f, 0.3f, 255, 255, 255, 255, false);

            await Task.FromResult(0);
        }
    }
}
