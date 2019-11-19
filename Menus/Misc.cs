using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Menus
{
    internal class Misc : Drawing
    {
        public static int balloonEntity = -1;

        static bool ingameCoords = false;
        static bool debugControls = false;

        public static async Task Draw()
        {
            SetMenuTitle("Utils", "miscellaneous things~");

            AddBool("Display coords", ref ingameCoords, cb: new Action<bool>((val) =>
            {
                Vector3 coords = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, Function.Call<int>(Hash.PLAYER_PED_ID), false);
                DrawText($"{coords.ToString()}", g_optionsFont, 0.4f, 0.98f, 0.30f, 0.30f, 255, 255, 255, 255);
            }));

            AddBool("Debug controls", ref debugControls, cb: new Action<bool>((val) =>
            {
                float controlY = 0.01f;

                foreach (Control ctrl in Enum.GetValues(typeof(Control)))
                {
                    if (Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, 0, (uint)ctrl))
                    {
                        DrawText(ctrl.ToString(), g_optionsFont, 0.8f, controlY, 0.25f, 0.25f, 255, 255, 255, 255);
                        controlY += 0.02f;
                    }
                }
            }));

            int balloon = AddMenuEntry("Spawn balloon");

            int defaultSpawn = AddMenuEntry("Set default spawn");

            if (IsEntryPressed(balloon))
            {
                Vector3 pos = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, API.PlayerPedId());
                balloonEntity = await Main.CreateVehicle(GenHash("hotAirBalloon01"), pos, 0);
            }

            if (IsEntryPressed(defaultSpawn))
            {
                Vector3 vec = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, Function.Call<int>(Hash.PLAYER_PED_ID));
                Storage.Set("SpawnLocation", vec);

                Client.SpawnLocation = vec;

                Scripts.Toast.AddToast("Spawn location set!", 3000, 0.25f + (0.3f / 2), GetCurrentActiveY());
            }

            await Task.FromResult(0);
        }
    }
}
