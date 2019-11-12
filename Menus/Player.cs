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

        public static async Task Draw()
        {
            SetMenuTitle("Player", "play with yourself!");

            AddMenuOption("Change Model", MenuId.MENU_CHANGE_MODEL);
            var r = AddBool("Disable Ragdoll?", ref disableRagdoll);
            var i = AddBool("Invincible?", ref isInvincible);

            var pedId = Function.Call<int>(Hash.PLAYER_PED_ID);

            if (IsEntryPressed(r))
            {
                Debug.WriteLine($"SET_PED_CAN_RAGDOLL to {!disableRagdoll} for me ({pedId})");
                Function.Call(Hash.SET_PED_CAN_RAGDOLL, pedId, !disableRagdoll);
            }

            if (IsEntryPressed(i))
            {
                Debug.WriteLine($"SET_ENTITY_INVINCIBLE to {isInvincible} for me ({pedId})");
                Function.Call(Hash.SET_ENTITY_INVINCIBLE, pedId, isInvincible);
            }

            StyleMenu();

            await Task.FromResult(0);
        }
    }
}
