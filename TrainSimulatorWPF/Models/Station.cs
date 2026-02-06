using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainSimulator
{
    internal class Station
    {
        public string StationName { get; private set; }
        string StationLocation;
        int QuaiNbr;
        public List<Schedule> ScheduleList
        {
            get;
            private set; 
        }
        public Station(string stationName, string stationLocation, int quainNbr, List<Schedule> schedules) {
            StationName = stationName; 
            StationLocation = stationLocation; 
            QuaiNbr = quainNbr;
            ScheduleList = schedules;
        }  


        public override string ToString()
        {
            return "----Bienvenue à la gare de "+StationName+"----\r\n";
        }

        

        // TODO create function that read a file of schedules and create schedule of given station
    }


}
