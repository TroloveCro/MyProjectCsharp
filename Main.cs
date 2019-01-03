using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GTANetworkAPI;



namespace MyProject
{
    public class Main : Script
    {
        [Command("vehicle")]
        public void vehicle(Client sender)
        {
            Vehicle veh = NAPI.Vehicle.CreateVehicle(VehicleHash.Kuruma, sender.Position, 0, 1, 1, "233", 0, false, true, sender.Dimension);
            veh.NumberPlate = "bego";
            sender.SetIntoVehicle(veh, -1);
        }
        [Command("save")]
        public void coords(Client player, string coordName)
        {
            Vector3 playerPosGet = NAPI.Entity.GetEntityPosition(player);
            var pPosX = (playerPosGet.X.ToString().Replace(',', '.') + ", ");
            var pPosY = (playerPosGet.Y.ToString().Replace(',', '.') + ", ");
            var pPosZ = (playerPosGet.Z.ToString().Replace(',', '.'));
            Vector3 playerRotGet = NAPI.Entity.GetEntityRotation(player);
            var pRotX = (playerRotGet.X.ToString().Replace(',', '.') + ", ");
            var pRotY = (playerRotGet.Y.ToString().Replace(',', '.') + ", ");
            var pRotZ = (playerRotGet.Z.ToString().Replace(',', '.'));
            if(player.IsInVehicle)
            {
                Vehicle vehicle = player.Vehicle;
                Vector3 vehiclepos = NAPI.Entity.GetEntityPosition(vehicle);
                pPosX = (vehiclepos.X.ToString().Replace(',', '.') + ", ");
                pPosY = (vehiclepos.Y.ToString().Replace(',', '.') + ", ");
                pPosZ = (vehiclepos.Z.ToString().Replace(',', '.'));
                Vector3 vehiclerot = NAPI.Entity.GetEntityRotation(vehicle);
                pRotX = (vehiclerot.X.ToString().Replace(',', '.') + ", ");
                pRotY = (vehiclerot.Y.ToString().Replace(',', '.') + ", ");
                pRotZ = (vehiclerot.Z.ToString().Replace(',', '.'));
            }

            NAPI.Chat.SendChatMessageToPlayer(player, "Your position is: ~y~" + playerPosGet, "~w~Your rotation is: ~y~" + playerRotGet);
            StreamWriter coordsFile;
            if (!File.Exists("SavedCoords.txt"))
            {
                coordsFile = new StreamWriter("SavedCoords.txt");
                
            }
            else
            {
                coordsFile = File.AppendText("SavedCoords.txt");
            }
            NAPI.Chat.SendChatMessageToPlayer(player, "~r~Coordinates have been saved!");
            coordsFile.WriteLine("| " + coordName + " | " + "Saved Coordenates: " + pPosX + pPosY + pPosZ + " Saved Rotation: " + pRotX + pRotY + pRotZ);
            coordsFile.Close();
        }
    }
}