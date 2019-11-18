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

            int model = AddMenuEntry("Set default model");

            if (IsEntryPressed(model))
            {
                Client.SpawnModel = ChangeModel.lastChosenModel;
                Storage.Set("SpawnModel", ChangeModel.lastChosenModel);

                Toast.AddToast($"Default model set!", 3000, 0.25f + (0.3f / 2), GetCurrentActiveY());
            }

            await Task.FromResult(0);
        }
    }
}
