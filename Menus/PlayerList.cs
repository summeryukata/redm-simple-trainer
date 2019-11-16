using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Menus
{
    internal class PlayerList : Drawing
    {
        public static int[] Players = new int[] { };

        public static async Task Draw()
        {
            SetMenuTitle("Online Players", "Re-open to recache");

            foreach (int p in Players)
            {
                string playerName = Function.Call<string>(Hash.GET_PLAYER_NAME, p);

                int idx = AddMenuEntry(playerName);

                if (IsEntryPressed(idx) && Function.Call<bool>(Hash.IS_PLAYER_PLAYING, p))
                {
                    PlayerDetails.SelectedPlayer = p;
                    PlayerDetails.SelectedPlayerName = playerName;
                    ChangeSubmenu(MenuId.MENU_PLAYERLIST_DETAILS);
                }
            }

            await Task.FromResult(0);
        }
    }
}
