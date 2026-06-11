using DriversTestEvaluation.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriversTestEvaluation.Core.Models
{
    public class Results
    {
        public Guid Id { get; set; }
        public string GameName { get; set; } = string.Empty;

        public bool Passed { get; set; }

        public int Score { get; set; }

        public int NumberOfEvents { get; set; }

        public List<DrivingEvent> Events { get; set; } = [];



    }
}
