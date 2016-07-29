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
            UserInput input = new UserInput();
            //UserInput input= new UserInput("The Cartographers", 3, 14);
            Console.WriteLine("Paste developer key:");
            string devKey = Console.ReadLine();

            //Do calculations
            SpartanCompany userCompany = new SpartanCompany(input._spartanCompanyName);

            userCompany.SetLastActiveDates(input, devKey).Wait();
            userCompany.UpdateMemberActivityLists(input._daysToInactive, input._minGamesToPlay);

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
