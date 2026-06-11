using System;
using System.Collections.Generic;
using System.Text;

namespace DriversTestEvaluation.Core.Models
{
    public class DrivingSession
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime StartedAt { get; set; } = DateTime.Now;

        public DateTime? EndedAt { get; set; }

        public List<Entry> Entries { get; set; } = [];
        public List<Coordinates> Coordinates { get; set; } = [];
        public int Score { get; set; } = 100;

        public List<DrivingEvent> Events { get; set; } = [];

        public bool SessionActive { get; set; } = true;

        public string GameWindowName { get; set; } = string.Empty;
    }
}
