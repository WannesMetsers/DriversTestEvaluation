using DriversTestEvaluation.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriversTestEvaluation.Core.DTOs
{
    public class ResultsResponseDto
    {
        public bool Passed { get; set; }

        public int Score { get; set; }

        public int NumberOfEvents { get; set; }

        public List<DrivingEventResponseDto> Events { get; set; } = [];

        public bool DrivingStraight { get; set; }

        public List<double> DifferencesInPlace { get; set; } = [];


        public bool RegularSpeed { get; set; }

        public List<double> Speeds { get; set; } = [];
    }
}
