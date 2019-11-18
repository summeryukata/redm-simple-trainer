using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Menus
{
    internal class ChangeModel : Drawing
    {
        public static async Task SetModel(string name)
        {
            int model = GenHash(name);
            int player = Function.Call<int>(Hash.PLAYER_ID);

            await Main.PerformRequest(model);

            Function.Call(Hash.SET_PLAYER_MODEL, player, model, false);
            Function.Call((Hash)0x283978A15512B2FE, Function.Call<int>(Hash.PLAYER_PED_ID), true);
            Function.Call(Hash.SET_MODEL_AS_NO_LONGER_NEEDED, model);
            
            Toast.AddToast($"Switching to {name}!", 3000, 0.25f + (0.3f / 2), GetCurrentActiveY());
        }

        public static async Task Draw()
        {
            SetMenuTitle("Change Model", "mmmmmmmmodel");

            foreach (string p in NameArrays.Peds)
            {
                int i = AddMenuEntry(p);

                if (IsEntryPressed(i))
                {
                    await SetModel(p);
                }
            }

            await Task.FromResult(0);
        }
    }
}
