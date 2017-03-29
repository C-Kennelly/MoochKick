using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoochKick_WindowsClient.Classes
{
    /// <summary>
    /// Represents a Halo 5 player with gamertag and a set of recent .
    /// </summary>
    class Player
    {
        public string gamertag { get; set; }
        public Stack<DateTime> recentGameDates;
        public bool activeStatus;

        //Constructor
        public Player(string username)
        {
            gamertag = username;
            recentGameDates = new Stack<DateTime>(25);
            activeStatus = false;
        }

        /// <summary>
        /// Returns true if player has played y games in x days.  Defaults to 3 games in 14 days.
        /// </summary>
        /// <param name="daysInactivityThreshold"></param>
        /// <param name="minimumNumberofGames"></param>
        /// <returns></returns>

        public bool isActive(int daysInactivityThreshold = 14, int minimumNumberofGames = 3)
        {
            DateTime temp = DateTime.UtcNow; //UtcNow handles minimumNumberofGames <= 1

            int playersTotalGames = recentGameDates.Count;

            if (playersTotalGames < 1)
            {
                activeStatus = false;
                return activeStatus;
            }

            //TODO: This can only be done once before results get weird.
            for (int i = 0; i < minimumNumberofGames && i < playersTotalGames; i++)
            {
                temp = recentGameDates.Pop();
            }

            TimeSpan difference = DateTime.UtcNow - temp;

            if (difference.Days < daysInactivityThreshold)
            {
                activeStatus = true;
                return activeStatus;
            }
            activeStatus = false;
            return activeStatus;
        }

    }

}