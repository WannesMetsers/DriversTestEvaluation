using DriversTestEvaluation.Core.DTOs;
using DriversTestEvaluation.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriversTestEvaluation.Core.IServices
{
    public interface IResultsService
    {

        Task<ResultsResponseDto> GetResults(DrivingSession session);



    }
}
