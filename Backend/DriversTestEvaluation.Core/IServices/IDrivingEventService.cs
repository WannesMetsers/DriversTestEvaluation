using DriversTestEvaluation.Core.DTOs;
using DriversTestEvaluation.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriversTestEvaluation.Core.IServices
{
    public interface IDrivingEventService
    {
        Task<List<DrivingEventResponseDto>> GetEvents(Guid sessionId);

        Task CreateVisionEvent(VisionResult vision, Guid sessionId);

        Task CreateJsonEvent(JsonEntry entry, Guid sessionId);

    }
}
