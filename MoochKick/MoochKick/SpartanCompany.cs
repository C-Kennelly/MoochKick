using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoochKick
{
    using Quartermaster;
    /// <summary>
    /// Represents a group of players.
    /// </summary>
    class SpartanCompany
    {
        public string name { get; set; }
        //public string leader { get; set; }
        public List<Player> activeMembers;
        public List<Player> inactiveMembers;

        public SpartanCompany(string spartanCompanyName)
        {
            name = spartanCompanyName;
            activeMembers = ConvertTagsToPlayers(Quartermaster.GetGamertagsForCompany( name ));
            inactiveMembers = new List<Player>();
        }

        public void SetMemberActiveDates()
        {

        }

        public void PopulateInactiveMembers(int inactivityThreshold)
        {
            foreach (Player player in activeMembers)
            {
                if (!player.isActive(inactivityThreshold))
                {
                    inactiveMembers.Add(player);
                }
            }

            foreach (Player player in inactiveMembers)
            {
                activeMembers.Remove(player);
            }
        }

        public void PrintActiveMembers()
        {
            foreach (Player player in activeMembers)
            {
                Console.WriteLine(player.gamertag);
            }
        }

        public void PrintInactiveMemebrs()
        {
            foreach(Player player in inactiveMembers)
            {
                Console.WriteLine(player.gamertag);
            }
        }

        private List<Player> ConvertTagsToPlayers(List<string> gamertagList)
        {
            List<Player> result = new List<Player>(gamertagList.Count());

            foreach(string gt in gamertagList)
            {
                result.Add(new Player(gt));
            }

            return result;
        }
    }
}
