using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp;
using HaloSharp.Model;
using HaloSharp.Query.Stats;
using HaloSharp.Extension;
using Quartermaster;

//Defining "inactive" as a player who has not played an Arena or Warzone in the last y days.
namespace MoochKick
{
    using Quartermaster;
    class Monolith
    {
      
        static void Main(string[] args)
        {
            Console.WriteLine("Enter Spartan Company Name: ");
            string spartanCompanyName = Console.ReadLine();
            Console.WriteLine("Paste developer key:");
            string devKey = Console.ReadLine();
            MakeRequest(spartanCompanyName, devKey);
            Console.ReadLine();
        }       

        static async void MakeRequest(string spartanCompanyName, string devKey)
        {
            //string spartanCompanyName;
            const int minGamesToPlay = 1;
            const int daysToInactive = 14;

            List<Enumeration.GameMode> activeGameModes = new List<Enumeration.GameMode>();
                activeGameModes.Add(Enumeration.GameMode.Arena);
                activeGameModes.Add(Enumeration.GameMode.Warzone);

            //TODO: spartan company I/O needs to be in main. how to get string working outside MakeRequest? 
            

            List<string> gamertagList = Quartermaster.GetGamertagsForCompany(spartanCompanyName);
            List<Player> playersToRemove = new List<Player>();

            List<Player> players = new List<Player>();

            //Turn list of gametags in Player objects
            foreach(string gt in gamertagList)
            {
                players.Add(new Player(gt));
            }


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
                foreach(Player player in players)
                {
                    //build the query
                    var query = new GetMatches()
                    .Take(minGamesToPlay)
                    .InGameModes(activeGameModes)
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
                        playersToRemove.Add(player);       //can't just remove, or foreach will die up above
                    }
                }
            } //end session

            //Prune players that had a call failure occur.
            foreach (Player badplayer in playersToRemove)
            {
                players.Remove(badplayer);
            }


            int i = 0;
            foreach (Player player in players)
            {
                if(!player.isActive(daysToInactive))
                {
                    if(player.lastActiveDate == DateTime.MinValue)      //TODO: if player class changes, this hack needs to swap
                    {
                        Console.WriteLine("{0} does not seem to have an Arena or Warzone game on record and is probably inactive.", player.gamertag);
                    }
                    else
                    {
                        Console.WriteLine("{0} has not played a game since {1} and is inactive.", player.gamertag, player.lastActiveDate.ToShortDateString());
                    }

                    i++;
                }
            }
            Console.WriteLine("Scan complete - found {0} inactive players of {1} valid members", i, players.Count);

        }
        
    }
}
