using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoochKick
{
    /// <summary>
    /// Represents a group of players.
    /// </summary>
    class SpartanCompany
    {
        public string name { get; set; }
        public string leader { get; set; }
        public List<Player> activeMembers;
        public List<Player> inactiveMembers; 

    }
}
