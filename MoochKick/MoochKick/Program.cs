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
    class Program
    {
      
        static void Main(string[] args)
        {
            bool stopBelievin = false;
            string devKey = "";  //Paste your development key here before building
                Console.WriteLine("Paste your dev key to get started.");
                devKey = Console.ReadLine();

            Console.WriteLine("Welcome to MoochKick!  Let's find some inactive players.");
            while(!stopBelievin)
            {
                stopBelievin = true;
                RunMoochKickSession(devKey);
                Console.WriteLine("Clear screen and run another session? (y/N)");

                if(Console.ReadLine().ToLower().Contains("y"))
                {
                    stopBelievin = false;
                    Console.Clear();
                }
            }
        }

        static void RunMoochKickSession(string devKey)
        {
            //Collect info
            UserInput input = new UserInput();

            //Do calculations
            SpartanCompany userCompany = new SpartanCompany(input._spartanCompanyName);
            userCompany.PopulateActiveMemberRecentGames(input, devKey).Wait();
            userCompany.UpdateMemberActivityLists(input._daysToInactive, input._minGamesToPlay);

            //print results
            Console.WriteLine();
            Console.WriteLine("Found {0} of {1} members who have not played at least {2} games in {3} days.", 
                userCompany.inactiveMembers.Count, (userCompany.activeMembers.Count + userCompany.inactiveMembers.Count), 
                input._minGamesToPlay, input._daysToInactive);
            userCompany.PrintInactiveMemebrs();
        }


    }
}
