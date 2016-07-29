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
                    //.Take(input._minGamesToPlay)
                    .InGameModes(input.activeGameModes)
                    .ForPlayer(player.gamertag);

                    //run the query
                    try
                    {
                        var matchSet = await session.Query(query);

                        if(matchSet.Count < 1)
                        {
                            inactiveMembers.Add(player);
                        }
                        else
                        {
                            //set last activedate for each player
                            Stack<DateTime> reversalStack = new Stack<DateTime>(25);

                            foreach(var result in matchSet.Results)
                            {

                                reversalStack.Push(result.MatchCompletedDate.ISO8601Date);
                            }

                            //TODO Cleanup
                            int count = reversalStack.Count;
                            //Console.WriteLine("Count is {0}", count);
                            //int counter = 0;
                            for(int i = 0; i < count; i++)
                            {
                                //counter++;
                                //DateTime temp = reversalStack.Pop();
                                //player.recentGameDates.Push(temp);
                                player.recentGameDates.Push(reversalStack.Pop());

                                //Console.WriteLine("Result {0}: Date is {1}", counter, temp.ToShortDateString());

                            }
                        }
                    }
                    //if the call fails, the player is invalid and must be removed... permanently...
                    //TODO - now handling 0 match case up above... this is no longer a blanket case and could be handled differently (players to retry? Unknown players?)
                    catch(HaloSharp.Exception.HaloApiException e)
                    {
                        //Console.WriteLine("Halo API call failed! GT: {0}", player.gamertag);
                        //Console.WriteLine(e);
                        inactiveMembers.Add(player);       //can't just remove from activeMembers, or foreach will die up above
                    }
                }

                foreach (Player player in inactiveMembers)
                {
                    activeMembers.Remove(player);  //clean out the no result players we found last time.
                }
            } //end session
        }

        public void UpdateMemberActivityLists(int inactivityThreshold, int minNumberofGames)
        {
            foreach (Player player in activeMembers)
            {
                if (!player.isActive(inactivityThreshold, minNumberofGames))
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
