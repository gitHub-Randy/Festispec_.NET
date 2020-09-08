using FestiSpec.Entity;
using Mapsui.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestiSpec.Maps
{
    public class RouteData
    {
        public Layer MapLayer { get; private set; }
        public Employee Employee { get; private set; }
        public double DistanceKM { get; private set; }
        public double TravelTimeMins { get; private set; }
        public string TravelTimeFormatted { get; private set; }

        public RouteData(Layer layer, Employee employee, double distance, double time)
        {
            MapLayer = layer;
            Employee = employee;
            DistanceKM = Math.Round(distance, 1);
            TravelTimeMins = time;
            int hrs = (int) Math.Floor(TravelTimeMins / 60);
            int mins = (int)(time - hrs * 60);
            TravelTimeFormatted = $"{hrs}:{mins.ToString("00")}";
        }

        public RouteData(Employee employee, double distance, double time)
        {
            Employee = employee;
            DistanceKM = Math.Round(distance, 1);
            TravelTimeMins = time;
            int hrs = (int)Math.Floor(TravelTimeMins / 60);
            int mins = (int)(time - hrs * 60);
            TravelTimeFormatted = $"{hrs}:{mins.ToString("00")}";
        }
    }
}
