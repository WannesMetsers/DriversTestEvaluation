using DriversTestEvaluation.Core.DTOs;
using DriversTestEvaluation.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriversTestEvaluation.Core.IServices
{
    public interface ISessionService
    {
        Task<ActionResult<Guid>> StartTest();

        Task StopTest(Guid sessionId);

        Task<UpdateDto> GetUpdate(Guid sessionId);

        Task<ResultsResponseDto> GetResults(Guid sessionId);



    }
}
