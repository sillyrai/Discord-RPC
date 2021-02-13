using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordRPC;
using Newtonsoft.Json.Linq;

namespace Discord_RPC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Discord-RPC";
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Attempting to start Discord-RPC, if something errors after this message, you've got an error in your config");
            try
            {
                dynamic JSON = JObject.Parse(File.ReadAllText("config.json"));
                var Context = new DiscordRpcClient(JSON.AppID.ToString());
                RichPresence RP = new RichPresence();

                Context.Initialize();

                RP.WithState(JSON.State.ToString());
                RP.WithDetails(JSON.Details.ToString());

                string StartTime = JSON.StartTime.ToString();
                if (!string.IsNullOrEmpty(StartTime))
                {
                    Context.UpdateStartTime(DateTime.Parse(StartTime));
                }

                Assets _Assets = new Assets()
                {
                    LargeImageKey = JSON.LargeAsset.Key.ToString(),
                    LargeImageText = JSON.LargeAsset.Tooltip.ToString(),
                    SmallImageKey = JSON.SmallAsset.Key.ToString(),
                    SmallImageText = JSON.SmallAsset.Tooltip.ToString()
                };

                RP.WithAssets(_Assets);
                // Add Buttons

                List<Button> UserButtons = new List<Button>();

                string Btn1Label = JSON.Buttons.Button1.Text.ToString();
                string Btn2Label = JSON.Buttons.Button2.Text.ToString();

                string Btn1URL = JSON.Buttons.Button1.URL.ToString();
                string Btn2URL = JSON.Buttons.Button2.URL.ToString();

                Button Button1 = new Button();
                Button Button2 = new Button();

                if (Btn1Label != "" && Btn1URL != "")
                {
                    Button1.Label = Btn1Label;
                    Button1.Url = Btn1URL;
                    UserButtons.Add(Button1);
                }
                if (Btn2Label != "" && Btn2URL != "")
                {
                    Button2.Label = Btn2Label;
                    Button2.Url = Btn2URL;
                    UserButtons.Add(Button2);
                }

                RP.Buttons = UserButtons.ToArray();
                Context.SetPresence(RP);

                System.Threading.Thread.Sleep(2500);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Title = "Discord-RPC @ Connected";
                Console.WriteLine($"Sucesfully updated {Context.CurrentUser.Username}#{Context.CurrentUser.Discriminator} presence");
                Console.ForegroundColor = ConsoleColor.White;

                System.Threading.Thread.Sleep(-1);
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadLine();
            }
        }
    }
}
