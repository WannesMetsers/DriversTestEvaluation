using System;
using System.Collections.Generic;
using System.Text;
using DriversTestEvaluation.Business.Mappers;
using DriversTestEvaluation.Core.DTOs;
using DriversTestEvaluation.Core.IServices;
using DriversTestEvaluation.Core.Models;

namespace DriversTestEvaluation.Business.Services
{
    public class ResultsService : IResultsService
    {
        private readonly IDrivingEventService _drivingEventService;
       
        public ResultsService(IDrivingEventService drivingEventService) 
        {
            _drivingEventService = drivingEventService;
            
        }

        public async Task<ResultsResponseDto> GetResults(DrivingSession session)
        {
            var results = new ResultsResponseDto();
            int score = await CalculateScoreAsync(session);
            results.Score = score;
            results.Passed = Passed(score);
            results.NumberOfEvents = session.Events.Count;
            results.Speeds = Speeds(session);
            results.DifferencesInPlace = DifferencesInPlace(session);
            results.Events = await _drivingEventService.GetEvents(session.Id);
            results.DrivingStraight = DrivingStraight(session);
            results.RegularSpeed = RegularSpeed(session);
            

            return results;



        }

        private async Task<int> CalculateScoreAsync(DrivingSession session)
        {
            int score = session.Score;
            List<DrivingEventResponseDto> events = await _drivingEventService.GetEvents(session.Id);
            
            foreach (var e in events)
            {
                if (score > 0)
                {
                    score -= e.Penalty;
                }
                
            }

           

            return score;
        }

        private static bool RegularSpeed(DrivingSession session)
        {
            double MaxDifference = 5;
            List<Entry> entries = session.Entries;
            double differenceCount = 0;
            double totalDifference = 0;
            if (entries == null || entries.Count == 0)
                return false;
            Entry lastEntry = entries[0];
            foreach (var entry in entries)
            {
                
                double SpeedDifference = Math.Abs(entry.Speed_kmh - lastEntry.Speed_kmh);
                totalDifference -= SpeedDifference;
                differenceCount++;

            }

            if ((totalDifference / differenceCount) > MaxDifference)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private static List<double> Speeds(DrivingSession session)
        {
            List<Entry> entries = session.Entries;
            List<double> speeds = new List<double>();
            foreach (var entry in entries)
            {

                speeds.Add(entry.Speed_kmh);

            }
            
            return speeds;
        }

        private static List<double> DifferencesInPlace(DrivingSession session)
        {
            List<Entry> entries = session.Entries;
            List<double> answer = new();

            if (entries == null || entries.Count < 2)
                return answer;

            Entry lastEntry = entries[0];

            foreach (var entry in entries)
            {
                if (entry.Position == null || entry.Position.Length < 2 ||
                    lastEntry.Position == null || lastEntry.Position.Length < 2)
                {
                    continue;
                }

                double xDifference = Math.Abs(entry.Position[0] - lastEntry.Position[0]);
                double yDifference = Math.Abs(entry.Position[1] - lastEntry.Position[1]);

                answer.Add(xDifference);
                answer.Add(yDifference);

                lastEntry = entry;
            }

            return answer;
        }

        private static bool DrivingStraight(DrivingSession session)
        {
            const double MaxDifference = 5;

            var entries = session.Entries;

            if (entries == null || entries.Count < 2)
                return true;

            double differenceCount = 0;
            double totalDifference = 0;

            Entry lastEntry = entries[0];

            for (int i = 1; i < entries.Count; i++)
            {
                var entry = entries[i];

                if (entry.Position == null || entry.Position.Length < 2 ||
                    lastEntry.Position == null || lastEntry.Position.Length < 2)
                {
                    continue;
                }

                double xDifference =
                    Math.Abs(entry.Position[0] - lastEntry.Position[0]);

                double yDifference =
                    Math.Abs(entry.Position[1] - lastEntry.Position[1]);

                totalDifference += xDifference;
                totalDifference -= yDifference;

                differenceCount += 2;

                lastEntry = entry;
            }

            if (differenceCount == 0)
                return true;

            double averageDifference = totalDifference / differenceCount;

            return averageDifference <= MaxDifference;
        }

        private static bool Passed(int score)
        {
            if (score < 70)
            {
                return false;
            }

            else return true;
        }
       

    }
}
