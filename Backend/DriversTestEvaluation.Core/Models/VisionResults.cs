using System;
using System.Collections.Generic;
using System.Text;

namespace DriversTestEvaluation.Core.Models
{
    public class VisionResult
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string TrafficLight { get; set; } = "none";

        public bool Speeding { get; set; }

        public bool LaneDeparture { get; set; }

        public bool CollisionRisk { get; set; }

        public int VisibleCars { get; set; }
    }
}
