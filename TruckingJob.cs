using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;

namespace MyProject
{
    
    class TruckingJob : Script
    {
        static int MAX_PLAYERS = NAPI.Server.GetMaxPlayers();
        static Vector3 truckingjobpos = new Vector3(902.3652, -1259.04, 28.64511);
        Vehicle[] truckstj = new Vehicle[5];


        [ServerEvent(Event.ResourceStart)]
        public void ResourceStart()
        {
            truckstj[0] = NAPI.Vehicle.CreateVehicle(VehicleHash.Hauler, new Vector3(907.3564, -1223.568, 24.75286), 181.7828f, 1, 1, "LS2203GN", 255, false, false, 0);
            truckstj[1] = NAPI.Vehicle.CreateVehicle(VehicleHash.Hauler, new Vector3(914.0662, -1222.818, 24.78706), 184.7243f, 1, 1, "LS2103GN", 255, false, false, 0);
            truckstj[2] = NAPI.Vehicle.CreateVehicle(VehicleHash.Packer, new Vector3(935.4578, -1238.723, 24.81491), 126.7368f, 1, 1, "LS2204GN", 255, false, false, 0);
            truckstj[3] = NAPI.Vehicle.CreateVehicle(VehicleHash.Packer, new Vector3(886.4308, -1255.348, 25.35702), 281f, 1, 1, "LS2101GN", 255, false, false, 0);
            truckstj[4] = NAPI.Vehicle.CreateVehicle(VehicleHash.Phantom, new Vector3(938.4507, -1226.286, 24.90275), 97.89416f, 1, 1, "LS2105GN", 255, false, false, 0);

        }
        [ServerEvent(Event.PlayerConnected)]
        public void PlayerConnected(Client player)
        {
            player.SetSharedData("IsTrucking", false);

        }
        [Command("gotot")]
        public void gotot(Client sender)
        {
            NAPI.Entity.SetEntityPosition(sender, new Vector3(938.4507, -1226.286, 24.90275));
     
        }
        [Command("startroute")]
        public void startroute(Client sender)
        {
            if (sender.GetSharedData("IsTrucking") is false) { sender.SendChatMessage("~r~ ERROR ~w~:Go to trucking company and take a job"); }
            if (!sender.IsInVehicle) { sender.SendChatMessage("~r~ ERROR ~w~:You are not in truck!"); }
            for(int x = 0; x< 5; x++)
            {
                if (NAPI.Player.GetPlayerVehicle(sender) == truckstj[x]) {
                    if(truckstj[x].TraileredBy == truckstj[x])
                    {
                        sender.SendChatMessage("~g~ message ~w~:Kamion ima prikolicu!");
                    }
                }
            }
            
            
        }
        [Command("trucking")]
        public void trucking(Client sender)
        {
            if (sender.GetSharedData("IsTrucking") is true) { sender.SendChatMessage("~r~ ERROR ~w~:You already accepted this job"); }           
            if (sender.Position.DistanceTo(truckingjobpos) < 5) {
                if(sender.GetSharedData("IsTrucking") is false)
                {
                    sender.SendChatMessage("~g~Message~w~: Take available truck, attach trailer and type in ~g~/startroute");
                    sender.SetSharedData("IsTrucking", true);
                }
            }
        }

    }
}
