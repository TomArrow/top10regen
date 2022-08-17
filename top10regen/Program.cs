using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace top10regen
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(".");
            Dictionary<string, int> times = new Dictionary<string, int>();
            foreach(string file in files)
            {
                string filename = Path.GetFileName(file);
                string playername = Path.GetFileNameWithoutExtension(file);
                string ext = Path.GetExtension(file);
                if(ext == ".txt" && filename != "{Top10}.txt" && filename != "{Top10}-new.txt")
                {
                    string fileContent = File.ReadAllText(file);
                    int time;
                    if(int.TryParse(fileContent, out time))
                    {
                        times[playername] = time;
                    }
                }
            }
            Dictionary<string, int> orderedTimes = times.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            int index = 0;
            int limit = 10;
            StringBuilder sb = new StringBuilder();
            foreach(KeyValuePair<string,int> time in orderedTimes)
            {
                sb.Append($"\\t{index}name\\{time.Key}\\t{index}time\\{time.Value}");
                if (++index >= limit) break;
            }
            while(index < limit)
            {
                sb.Append($"\\t{index}name\\NULL\\t{index}time\\16777216");
                index++;
            }
            Console.WriteLine(sb.ToString());
            if (!File.Exists("{Top10}-new.txt"))
            {
                File.WriteAllText("{Top10}-new.txt", sb.ToString());
            } else
            {
                Console.WriteLine("{Top10}-new.txt already exists. Please remove first.");
            }
            Console.ReadKey();
        }
    }
}
