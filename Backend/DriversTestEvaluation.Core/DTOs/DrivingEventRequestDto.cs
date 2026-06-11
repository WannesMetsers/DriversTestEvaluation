using System;
using System.Collections.Generic;
using System.Text;

namespace DriversTestEvaluation.Core.DTOs
{
    public class DrivingEventRequestDto
    {

        public string EventType { get; set; } = "";

        public int Penalty { get; set; }
    }
}
