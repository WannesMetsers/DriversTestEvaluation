using System;
using System.Collections.Generic;
using System.Text;

namespace DriversTestEvaluation.Core.Models
{
    public class Coordinates
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid SessionId { get; set; }

        public DrivingSession Session { get; set; }
        public double x {  get; set; }

        public double y { get; set; }

        
    }
}
