using DriversTestEvaluation.Core.DTOs;
using DriversTestEvaluation.Core.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace DriversTestEvaluation.Business.Mappers
{
    public static class DrivingEventMapper
    {
        public static DrivingEventResponseDto MapToResponse(this DrivingEvent drivingEvent)
        {
            DrivingEventResponseDto response = new DrivingEventResponseDto
            {
                Id= drivingEvent.Id,
                EventType = drivingEvent.EventType,
                Penalty = drivingEvent.Penalty,
            };

            return response;
        }

        public static List<DrivingEventResponseDto> MapListToResponse(this List<DrivingEvent> drivingEventList)
        {
            List<DrivingEventResponseDto> response = new List<DrivingEventResponseDto>();

            foreach(DrivingEvent d in drivingEventList)
            {
                response.Add(d.MapToResponse());
            }

            return response;
        }

        public static DrivingEvent MapToModel(this DrivingEventRequestDto dto) 
        {
            DrivingEvent model = new DrivingEvent
            {
                EventType = dto.EventType,
                Penalty = dto.Penalty,
            };

            return model;
        }
    }
}
