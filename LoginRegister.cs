using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GTANetworkAPI;
namespace MyProject
{
    class LoginRegister : Script
    {

        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnected(Client player, DisconnectionType type, string reason)
        {
            string Ppath = @"d:\RAGEMP\server-files\Accounts\" + player.SocialClubName + ".txt";
            string[] lines = File.ReadAllLines(Ppath); // Ucitava sve linije 
            using (StreamWriter account = new StreamWriter(Ppath))
            {
                account.WriteLine(lines[0]); // Social club ime
                account.WriteLine(lines[1]); // server ime
                account.WriteLine(lines[2]); // password
                account.WriteLine(player.Health);
                account.WriteLine(player.Armor);
                account.Close();
            }
        }
        [RemoteEvent("StartLogRegSystem")]
        public void StartLogRegSystem(Client player)
        {
            string Ppath = @"d:\RAGEMP\server-files\Accounts\" + player.SocialClubName + ".txt";
            if (File.Exists(Ppath))
            {
                player.SendChatMessage("~g~Message~w~: You are registered on this server~w~.  Please use ~g~/login [password]~w~ to login.");
            }
            else
            {
                player.SendChatMessage("~y~Warning~w~: You are not registered on this server~w~.  Please use ~r~/register [password]~w~ to register.");
            }
        }
        #region Register Command
        [Command("register")]
        public void register(Client player, string password)
        {
            string Ppath = @"d:\RAGEMP\server-files\Accounts\" + player.SocialClubName + ".txt"; 
            if (File.Exists(Ppath)) { player.SendChatMessage("~r~ ERROR ~w~:You already have account! Use /login [password]"); } // Ako igrac ima account
            else if (password.Length < 3) { player.SendChatMessage("~r~ ERROR ~w~:You password is to short, please use longer password!"); } // Ako je sifra manja od 3 slova ili brojke
            using (StreamWriter account = new StreamWriter(Ppath))
            {
                account.WriteLine("{0}", player.SocialClubName); // social club name
                account.WriteLine("{0}", player.Name); // server name
                account.WriteLine("{0}", password); // password
                account.WriteLine("{0}", player.Health); // health
                account.WriteLine("{0}", player.Armor); // health
                account.Close();       

            }
            player.SendChatMessage("~g~ Message ~w~:You sucessfully created account!");
            NAPI.ClientEvent.TriggerClientEvent(player, "LoggedIn");
            
        }
        #endregion
        [Command("login")]
        public void login(Client player, string password)
        {
            string Ppath = @"d:\RAGEMP\server-files\Accounts\" + player.SocialClubName + ".txt";
            if (!File.Exists(Ppath)) { player.SendChatMessage("~r~ ERROR ~w~:You dont have account! Use /register [password]"); } // Ako igrac ima account
            else
            {
                string readedpass = File.ReadLines(Ppath).Skip(2).Take(1).First(); // sifra se nalazi na 3 mjestu odnosno 0: social club name 1: ingame name // 2: password
                if (password == readedpass)
                {
                    player.SendChatMessage("~g~Message~w~: Password is correct");
                    using (StreamReader account = new StreamReader(Ppath))
                    {

                        NAPI.Data.SetEntityData(player, "SocialClubName", account.ReadLine());
                        NAPI.Data.SetEntityData(player, "ServerName", account.ReadLine());
                        NAPI.Data.SetEntityData(player, "Password", account.ReadLine());
                        NAPI.Data.SetEntityData(player, "PlayerHealth", account.ReadLine());
                        NAPI.Data.SetEntityData(player, "PlayerArmour", account.ReadLine());
                        account.Close();


                    }
                    NAPI.ClientEvent.TriggerClientEvent(player, "LoggedIn");
                }
                else
                {
                    player.SendChatMessage("~r~Error~w~: Password is incorrect");
                }
            }

            
        }
    }
}

