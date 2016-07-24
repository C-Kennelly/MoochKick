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

        //Constructor
        public Player(string username)
        {
            gamertag = username;
            lastActiveDate = DateTime.MinValue;     //TODO: This is set to Jan 1st 1901 right now.
        }
       
        /*
        if the latest match was within the threshold, the player is active.
        if object has not been initialized, will return false
        */
        public bool isActive(int daysInactivityThreshold)
        {
            TimeSpan difference = DateTime.UtcNow - lastActiveDate;

            if(difference.Days < daysInactivityThreshold)       
            {
                return true;
            }
            return false;
        }

    }

}

