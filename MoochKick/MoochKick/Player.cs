using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoochKick
{
    /// <summary>
    /// Represents a Halo 5 player.
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
            lastActiveDate = DateTime.MinValue;     //TODO: This is set to Jan 1st 1901 right now.
            recentGameDates = new Stack<DateTime>(25);
        }
       
        /*
        if the latest match was within the threshold, the player is active.
        if object has not been initialized, will return false
        */
        public bool isActive(int daysInactivityThreshold, int minimumNumberofGames)
        {
            DateTime temp = DateTime.MinValue;

            int playersTotalGames = recentGameDates.Count;
            //can we get rid of sentinel value?
            // what happens when there are less than 25 recent games?
            //what happens if min number of game is passed?

            for(int i = 0; i < minimumNumberofGames && i < playersTotalGames; i++)
            {
                temp = recentGameDates.Pop();
                //Console.WriteLine("Player {0} popped game date of {1}", gamertag, temp.ToShortDateString());
            }

            TimeSpan difference = DateTime.UtcNow - temp;
            //Console.WriteLine("Comparing game date {0} for {1}", temp.ToShortDateString(), gamertag);

            if(difference.Days < daysInactivityThreshold)       
            {
                return true;
            }
            return false;
        }

    }

}

