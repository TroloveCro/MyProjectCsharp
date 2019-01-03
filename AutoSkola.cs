using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GTANetworkAPI;
namespace MyProject
{
    class AutoSkola : Script
    {
        Vehicle[] drivingschoolveh = new Vehicle[NAPI.Server.GetMaxPlayers()];
        [RemoteEvent("SpawnDrivingVehicle")]
        public void SpawnDrivingVehicle(Client player, object[] arguments)
        {            
            int category = (int)arguments[1];
            NAPI.Data.SetEntitySharedData(player, "TakingTest", true);
            switch (category)
            {
                case 1:
                    drivingschoolveh[NAPI.Data.GetEntitySharedData(player, "PlayerID")] = NAPI.Vehicle.CreateVehicle(VehicleHash.Asea, new Vector3(-219.8647, -1171.897, 22.26661), new Vector3(0.02840148, 0.2682525, 2.483459), 1, 1, "VapidDS", 0, false, true, player.Dimension);
                    break;
                case 2:
                    drivingschoolveh[NAPI.Data.GetEntitySharedData(player, "PlayerID")] = NAPI.Vehicle.CreateVehicle(VehicleHash.Mule, new Vector3(-219.8647, -1171.897, 22.26661), new Vector3(0.02840148, 0.2682525, 2.483459), 1, 1, "VapidDS", 0, false, true, player.Dimension);
                    break;
                case 3:
                    drivingschoolveh[NAPI.Data.GetEntitySharedData(player, "PlayerID")] = NAPI.Vehicle.CreateVehicle(VehicleHash.Hauler, new Vector3(-219.8647, -1171.897, 22.26661), new Vector3(0.02840148, 0.2682525, 2.483459), 1, 1, "VapidDS", 0, false, true, player.Dimension);
                    break;
                case 4:
                    drivingschoolveh[NAPI.Data.GetEntitySharedData(player, "PlayerID")] = NAPI.Vehicle.CreateVehicle(VehicleHash.Forklift, new Vector3(-219.8647, -1171.897, 22.26661), new Vector3(0.02840148, 0.2682525, 2.483459), 1, 1, "VapidDS", 0, false, true, player.Dimension);
                    break;
                default:
                    player.SendChatMessage("~r~Driving school unexpected error, please call administrator or take screenshot of your problem.");
                    break;
            }
        }
        [ServerEvent(Event.PlayerEnterVehicle)]
        public void OnPlayerEnterVehicle(Client player, Vehicle vehicle, sbyte seatID)
        {
            if(Convert.ToBoolean(NAPI.Data.GetEntitySharedData(player, "TakingTest")))// Ako je igrac zavrsio class test i zapoceo driving test
            {
                if (vehicle == drivingschoolveh[NAPI.Data.GetEntitySharedData(player, "PlayerID")]) // ako je vozilo u koje ulazi jednako vozilu od autoskole
                {
                    if (seatID == -1) // ako je sjeo na vozacevo mjesto
                    {
                        string name = player.Name;
                        name.Replace("_", ".");
                        player.SendChatMessage("~b~Instructor~w~: Hi, " + name + ". Are you ready for short drive?");
                        player.SendChatMessage("~b~Instructor~w~: Just follow my instructions and dont be afraid.");
                        switch(vehicle.ClassName)
                        {
                            case 1:
                                NAPI.ClientEvent.TriggerClientEvent(player, "StartBCategory");
                                break;
                            case 20:
                                NAPI.ClientEvent.TriggerClientEvent(player, "StartCCECategory");
                                break;
                            case 11:
                                NAPI.ClientEvent.TriggerClientEvent(player, "StartFGCategory");
                                break;
                            default:
                                    player.SendChatMessage("~r~Driving school unexpected error, please call administrator or take screenshot of your problem.");
                                break;

                        }

                    }
                    else
                    {
                        player.SendChatMessage("~y~Warning~w~: You must take driver seat if you want to start driving test.");
                    }
                }
            }

        }
    }
}
