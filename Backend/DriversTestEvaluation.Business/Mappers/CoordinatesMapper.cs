using DriversTestEvaluation.Core.DTOs;
using DriversTestEvaluation.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriversTestEvaluation.Business.Mappers
{
    public static class CoordinatesMapper
    {
        public static CoordinatesResponseDto MapToResponse(this Coordinates coordinates)
        {
            CoordinatesResponseDto response = new CoordinatesResponseDto
            {
                Id = coordinates.Id,
                x = coordinates.x,
                y = coordinates.y,
            };

            return response;
        }

        public static List<CoordinatesResponseDto> MapListToResponse(this List<Coordinates> CoordinatesList)
        {
            List<CoordinatesResponseDto> response = new List<CoordinatesResponseDto>();

            foreach (Coordinates d in CoordinatesList)
            {
                response.Add(d.MapToResponse());
            }

            return response;
        }
    }
}
