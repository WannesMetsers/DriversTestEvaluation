using DriversTestEvaluation.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriversTestEvaluation.Core.DTOs
{
    public class UpdateDto
    {
        public List<DrivingEventResponseDto> Events { get; set; } = [];

        public List<CoordinatesResponseDto> Coordinates { get; set; } = [];
        
    }
}
