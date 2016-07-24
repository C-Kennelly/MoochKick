using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp;
using HaloSharp.Model;
using HaloSharp.Query.Stats;
using HaloSharp.Extension;
using Quartermaster;

//Defining "inactive" as a player who has not played an Arena or Warzone in the last y days.
namespace MoochKick
{
    using Quartermaster;
    class Monolith
    {
      
        static void Main(string[] args)
        {
            //Collect info
            Console.WriteLine("Enter Spartan Company Name: ");
            string spartanCompanyName = Console.ReadLine();
            Console.WriteLine("Paste developer key:");
            string devKey = Console.ReadLine();

            //Do calculations
            SpartanCompany userCompany = new SpartanCompany(spartanCompanyName);

            int minGamesToPlay = 1;
            int daysToInactive = 14;
            List<Enumeration.GameMode> activeGameModes = new List<Enumeration.GameMode>();
                activeGameModes.Add(Enumeration.GameMode.Arena);
                activeGameModes.Add(Enumeration.GameMode.Warzone);

            userCompany.SetLastActiveDates(minGamesToPlay,daysToInactive, activeGameModes, devKey).Wait();

            //print results
            Console.WriteLine("Here are active members inside Main");
            userCompany.PrintActiveMembers();
            Console.WriteLine("Here are {0} inactive members", userCompany.inactiveMembers.Count());
            userCompany.PrintInactiveMemebrs();
            Console.WriteLine("Finished!");
            Console.ReadLine();
        }

        
    }
}
