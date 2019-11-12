using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client;
using CitizenFX;

namespace client
{
    internal class Frame : Globals
    {
        private static async Task CurrentTime()
        {
            // set current time whenever a change is detected
            if (g_currentTime != previousCurrentTime)
            {
                Debug.WriteLine($"Setting time to {g_currentTime}");

                Function.Call(Hash.ADVANCE_CLOCK_TIME_TO, g_currentTime, 0, 0);

                previousCurrentTime = g_currentTime;
            }

            await Task.FromResult(0);
        }

        private static async Task CurrentWeather()
        {
            if (g_currentWeatherIdx != previousWeatherIdx)
            {
                var h = GenHash(g_weathers[g_currentWeatherIdx]);

                Debug.WriteLine($"Setting weather to {g_weathers[g_currentWeatherIdx]} ({h})");

                Function.Call(Hash._SET_WEATHER_TYPE_TRANSITION, h, h, 0.5, true);

                previousWeatherIdx = g_currentWeatherIdx;
            }

            await Task.FromResult(0);
        }

        private static async Task IngameCoords()
        {
            if (g_ingameCoords)
            {
                Vector3 coords = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, Function.Call<int>(Hash.PLAYER_PED_ID), false);
                Drawing.DrawText($"{coords.ToString()}", g_optionsFont, 0.4f, 0.98f, 0.30f, 0.30f, 255, 255, 255, 255, false);
            }

            await Task.FromResult(0);
        }

        private static async Task DebugControls()
        {
            if (g_debugControls)
            {
                float controlY = 0.01f;

                foreach (var fi in typeof(Control).GetFields())
                { 
                    if (Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, 0, (uint)fi.GetValue(null)))
                    {
                        Drawing.DrawText(fi.Name, Globals.g_optionsFont, 0.8f, controlY, 0.25f, 0.25f, 255, 255, 255, 255, false);
                        controlY += 0.02f;
                    }
                }

                await Task.FromResult(0);
            }
        }

        internal static async Task RunFunctions()
        {
            await CurrentTime();
            await IngameCoords();
            await CurrentWeather();
            await DebugControls();
        }
    }
}
