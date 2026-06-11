using System;
using System.Collections.Generic;
using System.Text;

namespace DriversTestEvaluation.Core.Models
{
    public class JsonEntry
    {
        public double[] Position { get; set; } = [];
        public double Speed_kmh { get; set; }
        public double SpaceCarInFront { get; set; }
        public string ColorTrafficLight { get; set; } = string.Empty;
        public bool InFrontOfTrafficLight {  get; set; }
        public double SpeedLimit { get; set; }

    }
}
