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
                SubscriptionKey = "",
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
                  .ForPlayer("Sn1p3r C");

                var matchSet = await session.Query(query);

                foreach(var result in matchSet.Results)
                {
                    Console.WriteLine("MatchID: {0} completed on {1}", result.Id.MatchId, result.MatchCompletedDate);
                    //Console.WriteLine(result.MatchCompletedDate);
                }
            }

        }



    }
}
