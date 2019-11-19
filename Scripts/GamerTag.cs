using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Scripts
{
    public class GamerTag
    {
        enum State
        {
            Disconnected,
            Connected,
            HasGamerTag
        }

        class PlayerInfo
        {
            public State State { get; set; } = State.Disconnected;
            public int PedId { get; set; } = -1;
            public int BlipId { get; set; } = -1;
            public int GamerTagId { get; set; } = -1;
        }

        static Dictionary<int, PlayerInfo> players = new Dictionary<int, PlayerInfo>();

        public static async Task Tick()
        {
            int CreateBlipForPlayer(int playerId, int pedId)
            {
                int blip = Function.Call<int>((Hash)0x23f74c2fda6e7c61, 422991367, pedId);

                Function.Call(Hash.SET_BLIP_NAME_TO_PLAYER_NAME, playerId);
                Function.Call(Hash.SET_BLIP_SCALE, blip, 0.90f);

                return blip;
            }

            int CreateGamerTagForPlayer(int playerPed, string playerName)
            {
                return Function.Call<int>(Hash.CREATE_FAKE_MP_GAMER_TAG, playerPed, playerName, true, true, "CFX", 3);
            }

            for (int i = 0; i < 32; i++)
            {
                int ped = API.GetPlayerPed(i);
                string name = Function.Call<string>(Hash.GET_PLAYER_NAME, i);

                if (Function.Call<bool>(Hash.NETWORK_IS_PLAYER_CONNECTED, i))
                {
                    if (!players.ContainsKey(i) || players[i].State == State.Disconnected)
                    {
                        players[i] = new PlayerInfo
                        {
                            State = State.Connected
                        };
                    }
                    else if (players[i].State == State.Connected)
                    {
                        if (Function.Call<bool>(Hash.DOES_ENTITY_EXIST, ped))
                        {
                            players[i].PedId = ped;
                            players[i].State = State.HasGamerTag;

                            if (ped != API.PlayerPedId())
                            {
                                if (!Function.Call<bool>(Hash.DOES_BLIP_EXIST, players[i].BlipId))
                                {
                                    players[i].BlipId = CreateBlipForPlayer(i, ped);
                                }

                                if (!Function.Call<bool>(Hash.IS_MP_GAMER_TAG_ACTIVE, players[i].GamerTagId))
                                {
                                    players[i].GamerTagId = CreateGamerTagForPlayer(ped, name);
                                }

                                Debug.WriteLine($"Created gamer tag and blip for {name}");
                            }
                            else
                            {
                                Debug.WriteLine("Skipped creating gamer tag for local player");
                            }
                        }
                    }
                    else if (players[i].State == State.HasGamerTag)
                    {
                        if (ped != API.PlayerPedId())
                        {
                            if (ped != players[i].PedId)
                            {
                                Function.Call(Hash.REMOVE_MP_GAMER_TAG, players[i].GamerTagId);
                                Function.Call(Hash.REMOVE_BLIP, players[i].BlipId);

                                players[i].PedId = ped;
                                players[i].BlipId = CreateBlipForPlayer(i, ped);
                                players[i].GamerTagId = CreateGamerTagForPlayer(i, name);

                                Debug.WriteLine($"Re-created gamer tag and blip for {name}");
                            }
                        }
                    }
                }
                else if (players.ContainsKey(i) && players[i].State != State.Disconnected)
                {
                    players[i].State = State.Disconnected;
                }
            }

            await Task.FromResult(0);
        }
    }
}
