using System;
using System.Collections.Generic;
using GTANetworkAPI;

namespace MyProject
{
    public class PlayerIds : Script
    {
        public List<Client> players = new List<Client>();
        public Dictionary<Client, NetHandle> playerLabels = new Dictionary<Client, NetHandle>();

        [ServerEvent(Event.ResourceStart)]
        public void ResourceStart()
        {
            for (var i = 0; i < NAPI.Server.GetMaxPlayers(); i++)
            {
                players.Add(null);
            }
        }
        [ServerEvent(Event.PlayerConnected)]
        public void PlayerConnected(Client player)
        {
            int index = getFreeId();
            if (index != -1)
            {
                players[index] = player;
                NAPI.Data.SetEntitySharedData(player, "PlayerID", index);
            }
            

        }
        [ServerEvent(Event.PlayerDisconnected)]
        public void PlayerDisconnected(Client player, string reason)
        {
            int index = players.IndexOf(player);
            if (index != -1)
            {
                players[players.IndexOf(player)] = null;
            }

        }

        [Command("id", "~y~USAGE: ~w~/id [id/PartOfName]")]
        public void GetPlayerId(Client sender, string idOrName)
        {
            Client target = findPlayer(sender, idOrName);
            if (target != null)
            {
                sender.SendChatMessage("~y~Player found: " + target.Name + " - ID: " + getIdFromClient(target));
            }
        }

        /// <summary>
        /// Find a player given a partial name or a ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="idOrName"></param>
        /// <returns>null or Client</returns>
        public Client findPlayer(Client sender, string idOrName)
        {
            int id;

            // If idOrName is Numeric
            if (int.TryParse(idOrName, out id))
            {
                return getClientFromId(sender, id);
            }

            Client returnClient = null;
            int playersCount = 0;
            foreach (var player in players)
            {
                // Skip if list element is null
                if (player == null) continue;


                // If player name contains provided name
                if (player.Name.ToLower().Contains(idOrName.ToLower()))
                {
                    // If player name == provided name
                    if ((player.Name.Equals(idOrName, StringComparison.OrdinalIgnoreCase)))
                    {
                        return player;
                    }
                    else
                    {
                        playersCount++;
                        returnClient = player;
                    }
                }
            }


            if (playersCount != 1)
            {
                if (playersCount > 0)
                {
                    sender.SendChatMessage("~r~ERROR: ~w~Multiple users found.");
                }
                else
                {
                    sender.SendChatMessage("~r~ERROR: ~w~Player name not found.");
                }
                return null;
            }

            return returnClient;
        }

        /// <summary>
        /// Gets the Client from a give ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="id"></param>
        /// <returns>null or Client</returns>
        public Client getClientFromId(Client sender, int id)
        {
            if (players[id] == null)
            {
                sender.SendChatMessage("~r~ERROR: ~w~Player ID not found.");
                return null;
            }

            return players[id];
        }

        /// <summary>
        /// Gets the ID from Client
        /// </summary>
        /// <param name="target"></param>
        /// <returns>id or -1 in case of don't find the player</returns>
        public int getIdFromClient(Client target)
        {
            return players.IndexOf(target);
        }

        /// <summary>
        /// Gets the first null element in the player list
        /// </summary>
        /// 
        /// <returns>index of the first element null or -1 in case of don't find any null element</returns>
        public int getFreeId()
        {
            foreach (var item in players)
            {
                if (item == null)
                {
                    return this.players.IndexOf(item);
                }
            }

            return -1;
        }
    }
}
