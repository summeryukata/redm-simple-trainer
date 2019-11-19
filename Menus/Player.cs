using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Menus
{
    internal class Player : Drawing
    {
        static bool disableRagdoll = false;
        static bool isInvincible = false;
        static bool infiniteStamina = false;

        static float pedScale = 1.0f;

        public static async Task Draw()
        {
            SetMenuTitle("Player", "play with yourself!");

            AddMenuOption("Change model", MenuId.MENU_CHANGE_MODEL);

            AddBool("Disable ragdoll", ref disableRagdoll, cb: new Action<bool>((val) =>
            {
                Function.Call(Hash.SET_PED_CAN_RAGDOLL, API.PlayerPedId(), !disableRagdoll);
            }));

            AddBool("Invincible", ref isInvincible, cb: new Action<bool>((val) =>
            {
                Function.Call(Hash.SET_ENTITY_INVINCIBLE, API.PlayerPedId(), isInvincible);
            }));

            AddBool("Infinite stamina", ref infiniteStamina, cb: new Action<bool>((val) =>
            {
                if (val)
                {
                    Function.Call(Hash.RESTORE_PLAYER_STAMINA, API.PlayerId(), 100.0f);
                }
            }));

            AddFloat("Ped Scale", ref pedScale, 0.1f, 115.0f, 0.2f);
            int resetScale = AddMenuEntry("Reset ped scale");

            int ped = API.PlayerPedId();

            Function.Call(Hash._SET_PED_SCALE, ped, pedScale);

            if (IsEntryPressed(resetScale))
            {
                pedScale = 1.0f;
                Function.Call((Hash)0x283978A15512B2FE, ped, "REAR");
            }

            int model = AddMenuEntry("Set default model");

            if (IsEntryPressed(model))
            {
                Client.SpawnModel = ChangeModel.lastChosenModel;
                Storage.Set("SpawnModel", ChangeModel.lastChosenModel);

                Scripts.Toast.AddToast($"Default model set!", 3000, 0.25f + (0.3f / 2), GetCurrentActiveY());
            }

            await Task.FromResult(0);
        }
    }
}
