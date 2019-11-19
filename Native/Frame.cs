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

        internal static async Task BalloonHandling()
        {
            if (Menus.Misc.balloonEntity != -1 && Function.Call<bool>(Hash.DOES_ENTITY_EXIST, Menus.Misc.balloonEntity) && Function.Call<bool>(Hash.IS_PED_IN_ANY_VEHICLE, API.PlayerPedId(), false))
            {
                float y = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, Control.VehMoveUpOnly);
                float left = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, Control.VehMoveLeftOnly);
                float right = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, Control.VehMoveRightOnly);

                if (y > 0.01f)
                {
                    Function.Call(Hash.SET_VEHICLE_FORWARD_SPEED, Menus.Misc.balloonEntity, y * 10);
                }

                float heading = Function.Call<float>(Hash.GET_ENTITY_HEADING, Menus.Misc.balloonEntity);
                bool set = false;

                if (left > 0.01f)
                {
                    heading += left;

                    if (heading >= 360.0f)
                    {
                        heading = 0f;
                    }

                    set = true;
                }

                if (right > 0.01f)
                {
                    heading -= right;

                    if (heading <= 0.0f)
                    {
                        heading = 359.99f;
                    }

                    set = true;
                }

                if (set)
                {
                    Function.Call(Hash.SET_ENTITY_HEADING, Menus.Misc.balloonEntity, heading);
                }
            }

            await Task.FromResult(0);
        }

        internal static async Task RunFunctions()
        {
            await CurrentTime();
            await CurrentWeather();
            await BalloonHandling();
        }
    }
}
