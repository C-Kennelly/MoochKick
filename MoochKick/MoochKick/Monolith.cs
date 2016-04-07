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
    class Monolith
    {
        static void Main(string[] args)
        {
            MakeRequest();
            Console.WriteLine("Hit ENTER to exit");
            Console.ReadLine();
        }
               

        static async void MakeRequest()
        {
            List<Enumeration.GameMode> activeGameModes = new List<Enumeration.GameMode>();
                activeGameModes.Add(Enumeration.GameMode.Arena);
                activeGameModes.Add(Enumeration.GameMode.Warzone);


            var developerAccessProduct = new Product
            {
                SubscriptionKey = "",               //Key gets pasted here.
                RateLimit = new RateLimit
                {
                    RequestCount = 10,
                    TimeSpan = new TimeSpan(0, 0, 0, 10),
                    Timeout = TimeSpan.Zero
                }
            };

            var cacheSettings = new CacheSettings
            {
                MetadataCacheDuration = new TimeSpan(0, 0, 10, 0),
                ProfileCacheDuration = new TimeSpan(0, 0, 10, 0),
                StatsCacheDuration = null //Don't cache 'Stats' endpoints.
            };

            var client = new HaloClient(developerAccessProduct, cacheSettings);

            using(var session = client.StartSession())
            {
                var query = new GetMatches()                       
                .InGameModes(activeGameModes)
                //.InGameMode(Enumeration.GameMode.Arena)  
                .ForPlayer("Sn1p3r C");

                var matchSet = await session.Query(query);          //Did you paste your key in here?  Need to better handle exceptions.

                Console.WriteLine(matchSet.ResultCount);

                foreach(var result in matchSet.Results)
                {
                    TimeSpan difference = DateTime.Now - result.MatchCompletedDate.ISO8601Date;       //TimeSpan difference = [Timespan]GetElapsedDays(DateTime date);

                    Console.WriteLine("Match {0} completed {1} days ago", result.Id.MatchId, difference.Days);

                    
                }
            }

        }



    }
}
