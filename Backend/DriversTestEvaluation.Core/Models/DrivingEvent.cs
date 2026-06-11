using System;
using System.Collections.Generic;
using System.Text;

namespace DriversTestEvaluation.Core.Models
{
    public class DrivingEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime Timestamp { get; set; } = DateTime.Now;

        public Guid SessionId { get; set; }

        public DrivingSession Session { get; set; } 
        public string EventType { get; set; } = "";

        public int Penalty { get; set; }
    }
}
