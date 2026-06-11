using DriversTestEvaluation.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriversTestEvaluation.Core.DTOs
{
    public class CoordinatesResponseDto
    {
        public Guid Id { get; set; } 

        public double x { get; set; }

        public double y { get; set; }
    }
}
