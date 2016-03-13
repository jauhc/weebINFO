using System;
using System.Timers;
using CSGSI;
using CSGSI.Nodes;

namespace animestate
{
    class animeinfo
    {
        static GameStateListener gsl;
        static void Main(string[] args)
        {
            gsl = new GameStateListener(1488);
            gsl.NewGameState += new NewGameStateHandler(OnNewGameState);
            if (!gsl.Start())
            {
                Environment.Exit(0);
            }
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("Listening...");
        }

        static void timerelapsed(object sender, ElapsedEventArgs e)
        {
            Console.Beep(600, 1);
        }
        
        static bool localplayer = false;

        static void OnNewGameState(GameState gs)
        {
            {
                if (gs.Player.SteamID == gs.Provider.SteamID)
                    localplayer = true;
                else
                    localplayer = false;
            }

            Console.ForegroundColor = ConsoleColor.Black;
            int iPlayerCurAmmo = gs.Player.Weapons.ActiveWeapon.AmmoClip;
            
            if (gs.Round.Bomb == BombState.Planted)
            {
                Console.BackgroundColor = ConsoleColor.Red;
            }
            if (gs.Round.Bomb == BombState.Undefined)
            {
                Console.BackgroundColor = ConsoleColor.White;
            }
            if (gs.Player.Weapons.ActiveWeapon.AmmoClip == 0 && localplayer)
            {
                Console.Beep(); //classic beep
                /* rofl this is a mess
                Timer t = new Timer();
                t.Interval = 1380; // ms delay
                t.AutoReset = true;
                t.Elapsed += new ElapsedEventHandler(timerelapsed);
                t.Start();
                */
            }

            if (gs.Player.State.Burning == 255 && localplayer)
            {
                Console.Beep(450, 1);
            }
            if (gs.Round.WinTeam == RoundWinTeam.CT)
            {
                Console.BackgroundColor = ConsoleColor.Cyan;
            }
            if (gs.Round.WinTeam == RoundWinTeam.T)
                    {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
            }
            if (gs.Round.WinTeam == RoundWinTeam.Undefined && gs.Round.Bomb != BombState.Planted)
                    {
                Console.BackgroundColor = ConsoleColor.White;
            }

            float kd = 0.00f;
            float kills = gs.Player.MatchStats.Kills;
            float deaths = gs.Player.MatchStats.Deaths;
            kd = kills / deaths;
            string kdr = kd.ToString("0.00");

            //todo: make coloured statuses, helmets etc
            //health could use a smooth transition from green to red depending on hp
            Console.Clear();
            Console.WriteLine(" ╔════CSGO Anime menu by edgar.money [beta]\n ║");
            Console.WriteLine(" ╠═══Playing " + gs.Map.Mode + " on " + gs.Map.Name);
            Console.WriteLine(" ╠═══Server time \t" + gs.Provider.TimeStamp);
            Console.WriteLine(" ╠═══Server ID \t\t" + gs.Provider.SteamID);
            Console.WriteLine(" ╠═══Players \t\t" + gs.AllPlayers.Count);
            Console.WriteLine(" ╠═══Currently " + gs.Map.Phase + " on round " + gs.Map.Round);
            Console.WriteLine(" ╠═══Game score: (CT - " + gs.Map.TeamCT.Score + ") / (T - " + gs.Map.TeamT.Score + ")" + "\n ║");
            Console.WriteLine(" ╟──Current player: [" + gs.Player.Clan + "] " + gs.Player.Name);
            Console.WriteLine(" ╟─" + gs.Player.Name + "'s STEAMID \t" + gs.Player.SteamID);
            Console.WriteLine(" ╟─" + gs.Player.Name + "'s health \t" + gs.Player.State.Health);
            Console.WriteLine(" ╟─" + gs.Player.Name + " has helmet \t" + gs.Player.State.Helmet);
            Console.WriteLine(" ╟─" + gs.Player.Name + " money\t \t" + gs.Player.State.Money);
            Console.WriteLine(" ╟─" + gs.Player.Name + "'s ammo \t" + gs.Player.Weapons.ActiveWeapon.AmmoClip + " out of " + gs.Player.Weapons.ActiveWeapon.AmmoClipMax + " (" + gs.Player.Weapons.ActiveWeapon.State + ")");
            Console.WriteLine(" ╟─" + gs.Player.Name + "'s weapon \t" + gs.Player.Weapons.ActiveWeapon.Name + " (" + gs.Player.Weapons.ActiveWeapon.Paintkit + ")" );
            Console.WriteLine(" ╟─" + gs.Player.Name + " money\t \t" + gs.Player.State.Money);
            Console.WriteLine(" ╟─" + gs.Player.Name + "'s KD ratio \t" + kdr + " = " + gs.Player.MatchStats.Kills + " / " + gs.Player.MatchStats.Deaths);
            Console.WriteLine(" ╟─Misc details\t");
            Console.WriteLine(" ╟─Flashed status \t" + gs.Player.State.Flashed);
            Console.WriteLine(" ╟─Smoked status \t" + gs.Player.State.Smoked);
            Console.WriteLine(" ╙─Burned status \t" + gs.Player.State.Burning);
            //Console.WriteLine("Winteam " + gs.Round.WinTeam);
            //Console.WriteLine("Bomb status " + gs.Round.Bomb);
        }
    }
}
