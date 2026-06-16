using DriversTestEvaluation.Business.Analyzers;
using DriversTestEvaluation.Business.Mappers;
using DriversTestEvaluation.Core.DTOs;
using DriversTestEvaluation.Core.IServices;
using DriversTestEvaluation.Core.Models;
using DriversTestEvaluation.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace DriversTestEvaluation.Business.Services
{
    public class SessionService : ISessionService
    {
        private readonly DriversTestEvaluationDbContext _context;
        private readonly IResultsService _resultsService;

        private readonly IDrivingEventService _eventService;

        private readonly IFrameBuffer _frameBuffer;

        //De jsons entry komen momenteel van deze vision klasse. 
        private readonly LlavaVisionAnalyzer vision;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly SimulatorAnalyzer _analyzer;
       



        public SessionService(DriversTestEvaluationDbContext context, IResultsService resultsService, IDrivingEventService eventService, IFrameBuffer frameBuffer, LlavaVisionAnalyzer gemini, IServiceScopeFactory serviceScopeFactory, SimulatorAnalyzer analyzer)
        {
            _context = context;
            _resultsService = resultsService;
            _eventService = eventService;
            _frameBuffer = frameBuffer;
            vision = gemini;
            _scopeFactory = serviceScopeFactory;
            _analyzer = analyzer;
        }

        public async Task<ActionResult<Guid>> StartTest()
        {
            Console.WriteLine("Test Started");
            GameWindowRequestDto gameWindowRequestDto = new GameWindowRequestDto { WindowTitle = "Game" };
            DrivingSession session = CreateSession(gameWindowRequestDto).Result;
            _ = Task.Run(() => AnalyzeJsonLoop(session));
            return session.Id;

        }

        public async Task StopTest(Guid sessionId)
        {
            var session = await _context.DrivingSession.FirstOrDefaultAsync(x => x.Id == sessionId);
            if (session == null)
                throw new ArgumentNullException("session is null");
            session.SessionActive = false;
            await _context.SaveChangesAsync();

        }

        public async Task<UpdateDto> GetUpdate(Guid sessionId)
        {
            UpdateDto update = new UpdateDto
            {
                Events = await GetCurrentEvents(sessionId),
                Coordinates = await GetCurrentCoordinates(sessionId),

            };


            return update;
        }

        public async Task<List<Entry>> GetEntries(Guid sessionId)
        {
            var entries = await _context.Entries
                .Where(e => e.SessionId == sessionId)
                .ToListAsync();
            return entries;
        }

        private async Task<List<CoordinatesResponseDto>> GetCurrentCoordinates(Guid sessionId)
        {
            var coordinates = await _context.Coordinates
                .Where(e => e.SessionId == sessionId)
                .ToListAsync();
            return coordinates.MapListToResponse();
        }

        private async Task<DrivingSession> CreateSession(GameWindowRequestDto window)
        {
            DrivingSession session = new DrivingSession();

            session.GameWindowName = window.WindowTitle;
            await _context.AddAsync(session);
            await _context.SaveChangesAsync();
            return session;
        }

       

        public async Task<ResultsResponseDto> GetResults(Guid sessionId) 
        {
            var session = await _context.DrivingSession
                                .Include(x => x.Entries)
                                .Include(x => x.Events)
                                .FirstOrDefaultAsync(x => x.Id == sessionId);
            if (session == null)
                throw new ArgumentNullException("session is null");
            ResultsResponseDto results = await _resultsService.GetResults(session);

            return results;
        }

        private async Task<List<DrivingEventResponseDto>> GetCurrentEvents(Guid sessionId)
        {
            var session = await _context.DrivingSession
                .FirstOrDefaultAsync(x => x.Id == sessionId);
            if (session == null)
                throw new ArgumentNullException("session is null");
            List<DrivingEventResponseDto> events = new List<DrivingEventResponseDto>();
            events = await _eventService.GetEvents(session.Id);

            return events;
        }

        //private async Task AnalyzeScreenLoop(DrivingSession session)
        //{
        //    while (true)
        //    {
        //        using var scope = _scopeFactory.CreateScope();
        //        var db = scope.ServiceProvider.GetRequiredService<DriversTestEvaluationDbContext>();

        //        var latestSession = await db.DrivingSession
        //            .AsNoTracking()
        //            .FirstOrDefaultAsync(x => x.Id == session.Id);

        //        if (latestSession == null || !latestSession.SessionActive)
        //            break;

        //        var service = scope.ServiceProvider.GetRequiredService<IDrivingEventService>();

        //        try
        //        {
        //            Console.WriteLine("Getting frame");

        //            //byte[] screenshot = _frameBuffer.GetLatest();

        //            var result = await vision.AnalyzeAsync(screenshot);

        //            Console.WriteLine("Analysis complete");

        //            await service.CreateEvent(result, session.Id);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"ERROR: {ex}");
        //        }

        //        await Task.Delay(2000);
        //    }
        //}

        //=========================================================================================================
        // Hier worden de jsons behandeld die door de ai aangemaakt worden. comment dit als je een simulator hebt
        //=========================================================================================================
        private async Task AnalyzeJsonLoop(DrivingSession session)
        {
            JsonEntry lastJsonEntry = null;
            while (true)
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<DriversTestEvaluationDbContext>();

                var latestSession = await db.DrivingSession
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == session.Id);

                if (latestSession == null || !latestSession.SessionActive)
                    break;

                var service = scope.ServiceProvider.GetRequiredService<IDrivingEventService>();

                try
                {
                    JsonEntry result = vision.AnalyzeJsonEntry(lastJsonEntry).Result; 
                    
                    lastJsonEntry = result;
                    await service.CreateJsonEvent(result, session.Id);
                    await CreateCoordinates(result, session.Id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex}");
                }

                await Task.Delay(2000);
            }
        }

        //=========================================================================================================
        // Hier worden de jsons behandeld die door de simulator aangemaakt worden. comment dit als je geen simulator hebt
        //=========================================================================================================

        //private async Task AnalyzeJsonLoop(DrivingSession session)
        //{
        //    while (true)
        //    {
        //        using var scope = _scopeFactory.CreateScope();
        //        var db = scope.ServiceProvider.GetRequiredService<DriversTestEvaluationDbContext>();

        //        var latestSession = await db.DrivingSession
        //            .AsNoTracking()
        //            .FirstOrDefaultAsync(x => x.Id == session.Id);

        //        if (latestSession == null || !latestSession.SessionActive)
        //            break;

        //        var service = scope.ServiceProvider.GetRequiredService<IDrivingEventService>();

        //        try
        //        {
        //            var result = await _analyzer.AnalyzeJsonEntry();

        //            await service.CreateJsonEvent(result, session.Id);
        //            await CreateCoordinates(result, session.Id);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"ERROR: {ex}");
        //        }

        //        await Task.Delay(2000);
        //    }
        //}

        private async Task CreateCoordinates(JsonEntry entry, Guid sessionId)
        {
            if (entry.Position != null)
            {
                Coordinates c = new Coordinates
                {
                    x = entry.Position[0],
                    y = entry.Position[1],
                    SessionId = sessionId
                };
                await _context.AddAsync(c);
                await _context.SaveChangesAsync();
            }
        }
    }




    
}
