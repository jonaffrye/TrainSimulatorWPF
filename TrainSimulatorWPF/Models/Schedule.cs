using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainSimulator
{
    internal class Schedule
    {
        public string StationA{ get; set; }
        public string StationB{ get; set; }
        public DateTime StartTime{ get; set; }
        public int QuaiNbr{ get; set; }

        public Schedule(string stationAName, string stationBName, DateTime startTime, int quaiNbr)
        {
            StationA = stationAName;
            StationB = stationBName;
            StartTime = startTime;
            QuaiNbr = quaiNbr;
        }

        public override string ToString()
        {
            return ($"{StartTime:HH:mm}".PadRight(8) + StationB.PadRight(25) + QuaiNbr.ToString());
        }
    }
}
