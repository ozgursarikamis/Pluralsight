using System;

namespace GameConsole
{
    internal class Program
    {
        private static void Main()
        { 
            PlayerCharacter[] players = new PlayerCharacter[3]
            {
                new PlayerCharacter{ Name = "Sarah" }, 
                new PlayerCharacter{ }, 
                null
            };

            string p1 = players[0]?.Name;
            string p2 = players[1]?.Name;
            string p3 = players[2]?.Name;
            Console.ReadLine();
        }
    }

    public class PlayerCharacter
    {
        public PlayerCharacter()
        {
            DateOfBirth = null;
            DaysSinceLastLogin = null;
        }
        public string Name { get; set; }
        public int? DaysSinceLastLogin { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? IsNoob { get; set; }
    }

    public class PlayerDisplayer
    {
        public static void Write(PlayerCharacter player)
        {
            Console.WriteLine(player.Name);
            var days = player.DaysSinceLastLogin ?? -1;
            Console.WriteLine($"Days : {days}");
            if (!player.DaysSinceLastLogin.HasValue)
            {
                Console.WriteLine("No value for DaysSinceLastLogin");
            }
            else
            {
                Console.WriteLine($"HasValue: {player.DaysSinceLastLogin.Value}");
            }

            if (player.DateOfBirth == null)
            {
                Console.WriteLine("No value for DateOfBirth");
            }
            else
            {
                Console.WriteLine(player.DateOfBirth);
            }

            if (player.IsNoob == null)
            {
                Console.WriteLine("Player newbie status is unknown");
            } else if (player.IsNoob == true)
            {
                Console.WriteLine("Player is newbie");
            }
            else
            {
                Console.WriteLine("Player is experienced");
            }
        }
    }
}
