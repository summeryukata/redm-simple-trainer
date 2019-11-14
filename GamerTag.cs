using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    public class GamerTag
    {
        public static int CreateGamerTag(int playerPed, string playerName)
        {
            return Function.Call<int>(Hash.CREATE_FAKE_MP_GAMER_TAG, playerPed, playerName, false, false, "CFX", 0);
        }

        enum State
        {
            Disconnected,
            Connected,
            HasGamerTag
        }

        static Dictionary<int, State> players = new Dictionary<int, State>();
        static Dictionary<int, int> gamerTags = new Dictionary<int, int>();
        static Dictionary<int, int> blips = new Dictionary<int, int>();

        static void UpdatePlayers()
        {
            for (int i = 0; i < 255; i++)
            {
                if (Function.Call<bool>(Hash.NETWORK_IS_PLAYER_CONNECTED, i))
                {
                    if (!players.ContainsKey(i) || players[i] == State.Disconnected)
                    {
                        players[i] = State.Connected;
                    }
                }
                else
                {
                    if (players.ContainsKey(i))
                    {
                        players[i] = State.Disconnected;
                    }
                }
            }
        }

        static int lastUpdate = 0;

        public static async Task Tick()
        {
            int time = API.GetGameTimer();
            if (time - lastUpdate > 100)
            {
                UpdatePlayers();
                lastUpdate = time;
            }

            foreach (var p in players)
            {
                if (p.Value == State.Connected)
                {
                    string name = Function.Call<string>(Hash.GET_PLAYER_NAME, p.Key);

                    int gt = CreateGamerTag(API.GetPlayerPed(p.Key), name);

                    gamerTags[p.Key] = gt;

                    int blip = Function.Call<int>((Hash)0x23f74c2fda6e7c61, 422991367, API.GetPlayerPed(p.Key));

                    Function.Call(Hash.SET_BLIP_NAME_TO_PLAYER_NAME, p.Key);
                    Function.Call(Hash.SET_BLIP_SCALE, blip, 0.90f);

                    blips[p.Key] = blip;

                    Debug.WriteLine($"Creating gamer tag and blip for {name}");
                }
            }

            foreach (var p in players.Where(p => p.Value == State.Connected).ToArray())
            {
                players[p.Key] = State.HasGamerTag;
            }

            foreach (var p in players.Where(p => p.Value == State.Disconnected).ToArray())
            {
                if (gamerTags.ContainsKey(p.Key))
                {
                    Function.Call(Hash.REMOVE_MP_GAMER_TAG, p.Value);
                }

                if (blips.ContainsKey(p.Key))
                {
                    Function.Call(Hash.REMOVE_BLIP, blips[p.Key]);
                }

                Debug.WriteLine($"Creating gamer tag and blip for {p.Key}");
            }
        }
    }
}
