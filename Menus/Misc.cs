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
        public static async Task Draw()
        {
            SetMenuTitle("Utils", "miscellaneous things~");

            AddBool("Display coords?", ref g_ingameCoords);
            AddBool("Debug controls?", ref g_debugControls);

            int defaultSpawn = AddMenuEntry("Set default spawn");

            StyleMenu();

            if (IsEntryPressed(defaultSpawn))
            {
                Vector3 vec = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, Function.Call<int>(Hash.PLAYER_PED_ID));
                Storage.Set("SpawnLocation", vec);

                Toast.AddToast("Spawn location set!", 3000, 0.25f + (0.3f / 2), GetCurrentActiveY());
            }

            await Task.FromResult(0);
        }
    }
}
