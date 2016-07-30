using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp.Model;

namespace MoochKick
{
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
            AskCompanyName();
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
        /// Asks user to enter a Spartan Company name.
        /// </summary>
        private void AskCompanyName()
        {
            Console.WriteLine("Enter Spartan Company Name: ");
            _spartanCompanyName = Console.ReadLine();
        }

        /// <summary>
        /// Asks user the minimum number of games parameter.
        /// </summary>
        private void AskMinGamesToPlay()
        {
            Console.WriteLine("Enter the Minimum Number of Games to Play: ");
            _minGamesToPlay = Convert.ToInt32(Console.ReadLine());
            //TODO - Don't let player add more than 25 until a refactor
        }

        /// <summary>
        /// Asks user the days to inactivity threshold.
        /// </summary>
        private void AskDaysToInactivityThreshold()
        {
            Console.WriteLine("Enter the Days To Inactivity Thresholds: ");
            _daysToInactive = Convert.ToInt32(Console.ReadLine());
        }

        /// <summary>
        /// Asks users if they want to exclude any game modes.
        /// Currently defaults to Arena and Warzone.
        /// </summary>
        private void AskGameModes()
        {
            Console.WriteLine("Setting game modes to Arena and Warzone");
            activeGameModes = new List<Enumeration.GameMode>();
                activeGameModes.Add(Enumeration.GameMode.Arena);
                activeGameModes.Add(Enumeration.GameMode.Warzone);
        }
    }
}
