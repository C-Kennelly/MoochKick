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
    using System.IO;
    using System.Windows.Forms;

    
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //Paste your development key here before building, and comment out the following three lines.
            //string devKey = "";  
            Console.WriteLine("Paste your dev key to get started.");
            Console.WriteLine("This can be added to the first line of Program.Main() if you will be building multiple times.");
            string devKey = Console.ReadLine();

            bool runAnotherSession = true;

            Console.WriteLine("Welcome to MoochKick!  Let's find some inactive players.");
            while(runAnotherSession)
            {
                runAnotherSession = false;
                RunMoochKickSession(devKey);
                Console.WriteLine("\nClear screen and run another session? (y/N)");

                if(Console.ReadLine().ToLower().Contains("y"))
                {
                    runAnotherSession = true;
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
            List<string> output = new List<string>(userCompany.inactiveMembers.Count + 1);

            string header = ("Found " +
                                userCompany.inactiveMembers.Count +
                                " of " +
                                (userCompany.activeMembers.Count + userCompany.inactiveMembers.Count) +
                                " members who have not played at least " +
                                input._minGamesToPlay +
                                " games in " +
                                input.printableGameModes +
                                " in the last " +
                                input._daysToInactive +
                                " days.");
            output.Add(header);

            Console.WriteLine(header);
            foreach(Player player in userCompany.inactiveMembers)
            {
                output.Add(player.gamertag);
                Console.WriteLine(player.gamertag);
            }

            Console.WriteLine("\nSave output to file? (y/N)");
            if(Console.ReadLine().ToLower().Contains("y"))
            {
                SaveListToFile(output);
            }

        }

        private static void SaveListToFile(List<string> contents)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    using(TextWriter tw = new StreamWriter(myStream))
                    {
                        foreach(string s in contents)
                        {
                            tw.WriteLine(s);
                        }
                    }

                    myStream.Close();
                }
            }
        }


    }
}
