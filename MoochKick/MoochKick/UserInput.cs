using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp.Model;

namespace MoochKick
{
    using Quartermaster;
    /// <summary>
    /// Takes user input.  Asks questions upon object instatiation.
    /// </summary>
    class UserInput
    {
        public string _spartanCompanyName { get; set; }
        public int _minGamesToPlay { get; set; }
        public int _daysToInactive { get; set; }
        public List<Enumeration.GameMode> activeGameModes;

        /// <summary>
        /// Default constructor - Asks questions
        /// </summary>
        public UserInput()
        {
            FindCompanyName();
            Console.WriteLine();
            Console.WriteLine("MoochKick defines an inactive player as someone who hasn't played at least X games in Y days.");
            Console.WriteLine("For instance, we might call a player inactive who hasn't played at least 3 games in 21 days.");
            AskMinGamesToPlay();
            AskDaysToInactivityThreshold();
            AskGameModes(); 
        }

        /// <summary>
        /// Testing constructor - automatically answers questions
        /// </summary>
        /// <param name="spartanCompanyName"></param>
        /// <param name="minGamesToPlay"></param>
        /// <param name="DaystoInactive"></param>
        public UserInput(string spartanCompanyName,int minGamesToPlay,int DaystoInactive)
        {
            _spartanCompanyName = spartanCompanyName;
            _minGamesToPlay = minGamesToPlay;
            _daysToInactive = DaystoInactive;
            AskGameModes();
        }

        /// <summary>
        /// Asks user to enter either a Spartan Company name or a gamertag.
        /// Checks a Spartan Company name first.
        /// </summary>
        private void FindCompanyName()
        {
            string response = "";
            string name = "";
            Console.WriteLine("Start by entering a Spartan Company Name");

            while(_spartanCompanyName == null)
            {
                Console.Write("\t");
                response = Console.ReadLine();
                name = CheckName(response);

                if(name != "")
                {
                    _spartanCompanyName = name;
                }
                else
                {
                    Console.WriteLine("Couldn't find a valid Spartan Company with the name '{0}' on HaloWaypoint.com.", response);
                    Console.WriteLine("Please re-enter the Spartan Company name.");
                }
            }
        }

        /// <summary>
        /// Substitute method until Quartermaster.Exists() becomes a thing
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private string CheckName(string response)
        {
            string name;
            if(Quartermaster.GetGamertagsForCompany(response).Count > 1)
            {
                name = response;
            }
            else
            {
                name = "";
                //Quartermaster is bugged - returning "/nSpartanCompanies      ....."
                //name = Quartermaster.GetSpartanCompanyFromGamertag(response);
            }

            return name;
        }

        /// <summary>
        /// Asks user the minimum number of games parameter.
        /// </summary>
        private void AskMinGamesToPlay()
        {
            do
            {
                Console.WriteLine('\t' + "How many games would you like to use?  (Max of 25)");
                Console.Write('\t');
                try
                {
                    _minGamesToPlay = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    _minGamesToPlay = -1;
                }
            } while(!ValidateInt(_minGamesToPlay, 1, 25));


            //TODO - Don't let player add more than 25 until a refactor
        }

        /// <summary>
        /// Asks user the days to inactivity threshold.
        /// </summary>
        private void AskDaysToInactivityThreshold()
        {
            do
            {
                Console.WriteLine("\tHow many days would you like to use? (Max of 1000)");
                Console.Write('\t');
                try
                {
                    _daysToInactive = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    _daysToInactive = -1;
                }
            } while(!ValidateInt(_daysToInactive, 1, 1000));
        }

        /// <summary>
        /// Asks users if they want to exclude any game modes.
        /// Currently defaults to Arena and Warzone.
        /// </summary>
        private void AskGameModes()
        {
            Console.WriteLine();
            Console.WriteLine("Game modes set to Arena and Warzone.");
            Console.WriteLine();
            activeGameModes = new List<Enumeration.GameMode>();
                activeGameModes.Add(Enumeration.GameMode.Arena);
                activeGameModes.Add(Enumeration.GameMode.Warzone);
        }

        /// <summary>
        /// Returns true if int is greater than or equal to Min, AND less or equal to Max.  Returns false and prints error if not.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private bool ValidateInt(int input, int min, int max)
        {
            if(input >= min && input <= max)
            {
                return true;
            }

            Console.WriteLine('\t' + "Sorry, your response must be a number between {0} and {1}", min, max);
            return false;

        }
    }
}
