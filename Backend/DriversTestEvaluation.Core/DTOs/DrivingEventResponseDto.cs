using System;
using System.Collections.Generic;
using System.Text;

namespace DriversTestEvaluation.Core.DTOs
{
    public class DrivingEventResponseDto
    {
        public Guid Id { get; set; }
        public string EventType { get; set; } = "";

        public int Penalty { get; set; }
    }
}
