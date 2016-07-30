using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoochKick
{
    class TalkingHead
    {
        public Stack<string> cleverLines = new Stack<string>(10);

        public TalkingHead()
        {
            BuildLines();
        }

        public string GetNextLine()
        {
            if(cleverLines.Count < 1)
            {
                BuildLines();
            }

            return cleverLines.Pop();
        }

        public static string GetEndLine()
        {
            return "Spartan Company data located!";
        }

        private void BuildLines()
        {
            List<string> lines = new List<string>(10);
            //Eventually could read from database
            lines.Add("Detecting Infinity file structures..."); //0
            lines.Add("Searching for additional open ports...");//10
            lines.Add("Negotiating firewalls...");//20
            lines.Add("Uploading network daemons...");//30
            lines.Add("Filtering data sectors...");//40
            lines.Add("Having drinks with Roland...");//50
            lines.Add("Discovered War Games records.  Scanning...");//60
            lines.Add("Encountered TASHI class sentinel AI.  Posting distraction thread on Waypoint...");//70
            lines.Add("Using Sarah Palmer's workout videos to destroy the enemy core...");//80
            lines.Add("Scanning SECTOR.BARVOPICS.DAT...");//90

            Shuffle(lines);

            foreach(string line in lines)
            {
                cleverLines.Push(line);
            }

        }

        private static Random rng = new Random();

        private static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while(n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }
}
