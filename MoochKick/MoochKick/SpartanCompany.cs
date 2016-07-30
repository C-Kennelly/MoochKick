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
        public List<Player> activeMembers;
        public List<Player> inactiveMembers;

        /// <summary>
        /// Constructor - Creates a Spartan Company object and scrapes HaloWaypoint.com to populate the activeMembers list.
        /// </summary>
        /// <param name="spartanCompanyName"></param>
        public SpartanCompany(string spartanCompanyName)
        {
            name = spartanCompanyName;
            activeMembers = ConvertTagsToPlayers(Quartermaster.GetGamertagsForCompany( name ));
            inactiveMembers = new List<Player>();
        }

        /// <summary>
        /// Query the Halo 5 API and populate all players' recentGames stack in this company's activeMembers.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="devKey"></param>
        /// <returns></returns>
        public async Task PopulateActiveMemberRecentGames(UserInput input, string devKey)
        {
            //setup product
            var developerAccessProduct = new Product
            {
                SubscriptionKey = devKey,               //Key taken from main
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
                TalkingHead talkingHead = new TalkingHead();
                int counter = 0;

                Console.WriteLine("Establishing contact with UNSC Infinity...");
                foreach(Player player in activeMembers)
                {
                    //build the query
                    var query = new GetMatches()
                    .InGameModes(input.activeGameModes)
                    .ForPlayer(player.gamertag);
                    //.Take(input._minGamesToPlay)

                    //run the query
                    try
                    {
                        if(counter %10 == 0)
                        {
                            Console.WriteLine();
                            Console.Write(talkingHead.GetNextLine());
                        }
                        else
                        {
                            Console.Write("."); 
                        }
                        counter++;

                        var matchSet = await session.Query(query);

                        if(matchSet.Count < 1)
                        {
                            inactiveMembers.Add(player);
                        }
                        else  //populate each player's recent games
                        {                 
                            Stack<DateTime> reversalStack = new Stack<DateTime>(25);

                            foreach(var result in matchSet.Results)
                            {
                                reversalStack.Push(result.MatchCompletedDate.ISO8601Date);
                            }
                            
                            while(reversalStack.Count > 0)
                            {
                                player.recentGameDates.Push(reversalStack.Pop());
                            }
                        }
                    }
                    catch(HaloSharp.Exception.HaloApiException e)
                    {
                        //Call failed, assuming player is inactive.
                        inactiveMembers.Add(player);
                    }
                }

                foreach (Player player in inactiveMembers)
                {
                    activeMembers.Remove(player);
                }
            } //end session
            Console.WriteLine("\nSpartan Company data found!");
        }

        /// <summary>
        /// Moves players from activeMembers to inactiveMembers based on the contents of their RecentGames stack.
        /// </summary>
        /// <param name="inactivityThreshold"></param>
        /// <param name="minNumberofGames"></param>
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

        /// <summary>
        /// Prints all members of activeMembers.
        /// </summary>
        public void PrintActiveMembers()
        {
            foreach (Player player in activeMembers)
            {
                Console.WriteLine(player.gamertag);
            }
        }

        /// <summary>
        /// Prints all members of inactiveMemebers.
        /// </summary>
        public void PrintInactiveMemebrs()
        {
            foreach(Player player in inactiveMembers)
            {
                Console.WriteLine(player.gamertag);
            }
        }

        /// <summary>
        /// Converts a list of gamertags in string format to a list of Player objects.
        /// </summary>
        /// <param name="gamertagList"></param>
        /// <returns></returns>
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
