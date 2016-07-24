using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp.Model;

namespace MoochKick
{
    class UserInput
    {
        public string _spartanCompanyName { get; set; }
        public int _minGamesToPlay { get; set; }
        public int _daysToInactive { get; set; }
        public List<Enumeration.GameMode> activeGameModes;

        public UserInput()
        {
            AskCompanyName();
            AskMinGamesToPlay();
            AskDaysToInactivityThreshold();
            AskGameModes(); 
        }

        public UserInput(string spartanCompanyName,int minGamesToPlay,int DaystoInactive)
        {
            _spartanCompanyName = spartanCompanyName;
            _minGamesToPlay = minGamesToPlay;
            _daysToInactive = DaystoInactive;
            AskGameModes();
        }

        //TODO - Scrub user input

        private void AskCompanyName()
        {
            Console.WriteLine("Enter Spartan Company Name: ");
            _spartanCompanyName = Console.ReadLine();
        }

        private void AskMinGamesToPlay()
        {
            Console.WriteLine("Enter the Minimum Number of Games to Play: ");
            _minGamesToPlay = Convert.ToInt32(Console.ReadLine());
            //TODO - Don't let player add more than 25 until a refactor
        }

        private void AskDaysToInactivityThreshold()
        {
            Console.WriteLine("Enter the Days To Inactivity Thresholds: ");
            _daysToInactive = Convert.ToInt32(Console.ReadLine());
        }
        private void AskGameModes()
        {
            Console.WriteLine("Setting game modes to Arena and Warzone");
            activeGameModes = new List<Enumeration.GameMode>();
                activeGameModes.Add(Enumeration.GameMode.Arena);
                activeGameModes.Add(Enumeration.GameMode.Warzone);
        }
    }
}
