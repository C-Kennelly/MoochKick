using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp;
using HaloSharp.Model;
using HaloSharp.Query.Stats;
using HaloSharp.Extension;

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

        public async Task SetLastActiveDates(UserInput input, string devKey)
        {
            //setup product
            var developerAccessProduct = new Product
            {
                SubscriptionKey = devKey,               //Key taken from command line
                RateLimit = new RateLimit
                {
                    RequestCount = 10,
                    TimeSpan = new TimeSpan(0, 0, 0, 10),
                    Timeout = new TimeSpan(0, 0, 0, 10)
                }
            };
            //set cache settings
            var cacheSettings = new CacheSettings
            {
                MetadataCacheDuration = new TimeSpan(0, 0, 10, 0),
                ProfileCacheDuration = new TimeSpan(0, 0, 10, 0),
                StatsCacheDuration = null //Don't cache 'Stats' endpoints.
            };

            //create client, start session
            var client = new HaloClient(developerAccessProduct, cacheSettings);
            using(var session = client.StartSession())
            {
                /*
                Iterate through each gamertag in the list, querying the most recent match, 
                and reporting inactivity if it's out of the the range defined at the top
                top of the method.
                */
                foreach(Player player in this.activeMembers)
                {
                    //build the query
                    var query = new GetMatches()
                    .Take(input._minGamesToPlay)
                    .InGameModes(input.activeGameModes)
                    .ForPlayer(player.gamertag);

                    //run the query
                    try
                    {
                        var matchSet = await session.Query(query);

                        //set last activedate for each player
                        foreach(var result in matchSet.Results)
                        {
                            player.lastActiveDate = result.MatchCompletedDate.ISO8601Date;
                        }
                    }
                    //if the call fails, the player is invalid and must be removed... permanently...
                    catch(HaloSharp.Exception.HaloApiException e)
                    {
                        //Console.WriteLine("Halo API call failed! GT: {0}", player.gamertag);
                        //Console.WriteLine(e);
                        //playersToRemove.Add(player);       //can't just remove, or foreach will die up above
                    }
                }
            } //end session

            UpdateMemberActivityLists(input._daysToInactive);
        }

        private void UpdateMemberActivityLists(int inactivityThreshold)
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
