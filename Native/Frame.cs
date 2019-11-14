using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    internal class Frame : Globals
    {
        private static async Task CurrentTime()
        {
            // set current time whenever a change is detected
            if (g_currentTimeMinutes != previousCurrentTime)
            {
                if (g_currentTimeMinutes == 60)
                {
                    g_currentTimeMinutes = 0;

                    g_currentTimeHours++;

                    if (g_currentTimeHours > 23)
                    {
                        g_currentTimeHours = 0;
                    }
                }

                
                Function.Call((Hash)0x669E223E64B1903C, g_currentTimeHours, g_currentTimeMinutes, 0, 0, 0);

                previousCurrentTime = g_currentTimeMinutes;
            }
            else
            {
                g_currentTimeHours = Function.Call<int>(Hash.GET_CLOCK_HOURS);
                g_currentTimeMinutes = Function.Call<int>(Hash.GET_CLOCK_MINUTES);

                previousCurrentTime = g_currentTimeMinutes;
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
                Drawing.DrawText($"{coords.ToString()}", g_optionsFont, 0.4f, 0.98f, 0.30f, 0.30f, 255, 255, 255, 255);
            }

            await Task.FromResult(0);
        }

        private static async Task DebugControls()
        {
            if (g_debugControls)
            {
                float controlY = 0.01f;

                foreach (Control ctrl in Enum.GetValues(typeof(Control)))
                { 
                    if (Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, 0, (uint)ctrl))
                    {
                        Drawing.DrawText(ctrl.ToString(), Globals.g_optionsFont, 0.8f, controlY, 0.25f, 0.25f, 255, 255, 255, 255);
                        controlY += 0.02f;
                    }
                }

                await Task.FromResult(0);
            }
        }

        internal static async Task InfiniteStamina()
        {
            if (g_infiniteStamina)
            {
                Function.Call(Hash.RESTORE_PLAYER_STAMINA, API.PlayerPedId(), 100.0f);
                await Task.FromResult(0);
            }
        }

        internal static async Task ResurrectIfDead()
        {
            int ped = API.PlayerPedId();
            if (Function.Call<bool>(Hash.IS_ENTITY_DEAD, ped))
            {
                Toast.AddToast("You seem to have died. Resurrecting", 5000, 0.18f, 0.05f);
                Function.Call(Hash.NETWORK_RESURRECT_LOCAL_PLAYER, Client.SpawnLocation.X, Client.SpawnLocation.Y, Client.SpawnLocation.Z, 0, 0, 0, 0);
            }
            await Task.FromResult(0);
        }

        internal static async Task RunFunctions()
        {
            await CurrentTime();
            await IngameCoords();
            await CurrentWeather();
            await DebugControls();
            await InfiniteStamina();
            await ResurrectIfDead();
        }
    }
}
