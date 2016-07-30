using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoochKick
{
    /// <summary>
    /// Represents a Halo 5 player with gamertag and a set of recent .
    /// </summary>
    class Player
    {
        public string gamertag { get; set; }
        public DateTime lastActiveDate { get; set; }
        public Stack<DateTime> recentGameDates;


        //Constructor
        public Player(string username)
        {
            gamertag = username;
            recentGameDates = new Stack<DateTime>(25);
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

            if(playersTotalGames <1)
            {
                return false;
            }

            for(int i = 0; i < minimumNumberofGames && i < playersTotalGames; i++)
            {
                temp = recentGameDates.Pop();
            }

            TimeSpan difference = DateTime.UtcNow - temp;

            if(difference.Days < daysInactivityThreshold)       
            {
                return true;
            }
            return false;
        }

    }

}

