using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Scripts
{
    public class DeathScreen
    {
        static bool isDead = false;
        static bool justDied = false;

        static int fgAlpha = 255;
        static int bgAlpha = 0;
        static bool hasFadedIn;

        static int deathTime;

        public static async Task Tick()
        {
            int ped = API.PlayerPedId();

            if (Function.Call<bool>(Hash.IS_ENTITY_DEAD, ped))
            {
                if (!isDead)
                {
                    justDied = true;
                    isDead = true;
                }
                else if (justDied)
                {
                    justDied = false;
                }
            }

            if (justDied)
            {
                justDied = false;

                deathTime = Function.Call<int>(Hash.GET_GAME_TIMER);
            }


            if (isDead)
            {
                Drawing.DrawTexture("frontend_store", "store_background", 0.5f, 0.5f, 1.0f, 1.0f, .0f, 255, 255, 255, bgAlpha, true);

                float scaleBgX = 5f;
                float scaleBgY = scaleBgX + 0.05f;

                float scaleFgX = 5f;
                float scaleFgY = scaleFgX;

                int timer = Function.Call<int>(Hash.GET_GAME_TIMER);

                if (bgAlpha < 200 && !hasFadedIn)
                {
                    bgAlpha += (int)(2000 * Function.Call<float>(Hash.GET_FRAME_TIME)); // 2000 * frameTime = 200ms to get to 200
                }
                else if (!hasFadedIn)
                {
                    hasFadedIn = true;
                }

                if (hasFadedIn && timer - deathTime >= 2800)
                {
                    bgAlpha -= (int)(2000 * Function.Call<float>(Hash.GET_FRAME_TIME)); // 2000 * frameTime = 200ms to get to 200
                    fgAlpha -= (int)(2050 * Function.Call<float>(Hash.GET_FRAME_TIME)); // 2000 * frameTime = 200ms to get to 200

                    if (bgAlpha < 0) bgAlpha = 0;
                    if (fgAlpha < 0) fgAlpha = 0;
                }

                if (timer - deathTime >= 300)
                {
                    Drawing.DrawText("WASTED", 23, 0.5f, 0.348f, scaleBgX, scaleBgY, 0, 0, 0, bgAlpha, true);
                    Drawing.DrawText("WASTED", 23, 0.5f, 0.343f, scaleFgX, scaleFgY, 203, 16, 16, fgAlpha, true);
                }

                if (timer - deathTime >= 3000)
                {
                    bgAlpha = 0;
                    fgAlpha = 255;

                    hasFadedIn = false;
                    isDead = false;

                    Function.Call(Hash.NETWORK_RESURRECT_LOCAL_PLAYER, Client.SpawnLocation.X, Client.SpawnLocation.Y, Client.SpawnLocation.Z, 0, 0, 0, 0);
                    await Menus.ChangeModel.SetModel(Client.SpawnModel, false); // this is just to trigger model fade-in.. to make it less awkward
                }
            }

            await Task.FromResult(0);
        }
    }
}
