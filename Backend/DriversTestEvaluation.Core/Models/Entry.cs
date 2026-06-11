using System;
using System.Collections.Generic;
using System.Text;

namespace DriversTestEvaluation.Core.Models
{
    public class Entry
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid SessionId { get; set; }

        public DrivingSession Session { get; set; }
        public double[] Position { get; set; } = [];
        public double Speed_kmh { get; set; }
        public double SpaceCarInFront { get; set; }
        public string ColorTrafficLight { get; set; } = string.Empty;
        public bool InFrontOfTrafficLight { get; set; }
        public double SpeedLimit { get; set; }
    }
}
