using DriversTestEvaluation.Business.Mappers;
using DriversTestEvaluation.Core.DTOs;
using DriversTestEvaluation.Core.IServices;
using DriversTestEvaluation.Core.Models;
using DriversTestEvaluation.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace DriversTestEvaluation.Business.Services
{
    public class DrivingEventService : IDrivingEventService
    {
        private readonly DriversTestEvaluationDbContext _context;
        public DrivingEventService(DriversTestEvaluationDbContext context) 
        {
            _context = context;
        }

        public async Task<List<DrivingEventResponseDto>> GetEvents(Guid sessionId)
        {
            var events = await _context.DrivingEvent
                .Where(e => e.SessionId == sessionId)
                .Where(e => !string.IsNullOrEmpty(e.EventType))
                .ToListAsync();

            

            return events.MapListToResponse();
        }

        //Variables for TrafficlightControl
        private string LastTrafficLight = "";
        private bool TrafficLightOnLastFrame = false;
        private double lastSpeed;

        public DrivingEvent CalculateDrivingEvent(string eventType)
        {
            DrivingEvent drivingEvent = new DrivingEvent
            {
               
            };

            if (eventType == "speeding")
            {
                drivingEvent.EventType = "speeding";
                drivingEvent.Penalty = 5;
            }

            if (eventType == "ToCloseToVehicleInFront")
            {
                drivingEvent.EventType = "To close to vehicle in front";
                drivingEvent.Penalty = 10;
            }

            return drivingEvent;

        }

        private DrivingEvent HandleTrafficLight(string state)
        {
            DrivingEvent drivingEvent = new DrivingEvent();

            if (state == "red")
            {
                drivingEvent.EventType = "Ran through red light";
                drivingEvent.Penalty = 10;
            }

            else if (state == "yellow")
            {
                drivingEvent.EventType = "Ran through yellow light";
                drivingEvent.Penalty = 5;
            }



            return drivingEvent;
        }


        public async Task CreateJsonEvent(JsonEntry entry, Guid sessionId)
        {
            Entry entry1 = new Entry
            {
                SessionId = sessionId,
                Position = entry.Position,
                SpeedLimit = entry.SpeedLimit,
                SpaceCarInFront = entry.SpaceCarInFront,
                Speed_kmh   = entry.Speed_kmh,
                InFrontOfTrafficLight = entry.InFrontOfTrafficLight,
                ColorTrafficLight = entry.ColorTrafficLight,

            };

            await _context.AddAsync(entry1);
            await _context.SaveChangesAsync();

            DrivingEvent drivingEvent = new DrivingEvent
            {
                SessionId = sessionId
            };

            if (entry.Speed_kmh > entry.SpeedLimit)
            {
                int difference = (int)((entry.Speed_kmh - entry.SpeedLimit) / 5);

                drivingEvent = CalculateDrivingEvent("speeding");

                drivingEvent.Penalty = drivingEvent.Penalty * difference;

            }

            if(entry.SpaceCarInFront < 10)
            {
                drivingEvent = CalculateDrivingEvent("ToCloseToVehicleInFront");
            }

            else if (!entry.InFrontOfTrafficLight)
            {
                if (TrafficLightOnLastFrame)
                {
                    drivingEvent = HandleTrafficLight(LastTrafficLight);
                    drivingEvent.SessionId = sessionId;
                }

                TrafficLightOnLastFrame = false;
            }
            else if (entry.InFrontOfTrafficLight)
            {
                TrafficLightOnLastFrame = true;
                LastTrafficLight = entry.ColorTrafficLight;
            }

            
            
            if (drivingEvent != null | drivingEvent.EventType != "")
            {
                drivingEvent.SessionId = sessionId;
                await _context.AddAsync(drivingEvent);
                await _context.SaveChangesAsync();

            }
            if (entry.Position != null)
            {
                Coordinates coordinates = new Coordinates
                {
                    SessionId = sessionId,
                    x = entry.Position[0],
                    y = entry.Position[1]
                };
                await _context.AddAsync(coordinates);
                await _context.SaveChangesAsync();
            }
        }

       
        public async Task CreateVisionEvent(VisionResult vision, Guid sessionId)
        {
            DrivingEvent drivingEvent = new DrivingEvent
            {
                SessionId = sessionId 
            };

            if (vision.CollisionRisk)
            {
                drivingEvent.EventType = "CollisionRisk";
                drivingEvent.Penalty = 50;
            }
            else if (vision.LaneDeparture)
            {
                drivingEvent.EventType = "LaneDeparture";
                drivingEvent.Penalty = 10;
            }
            else if (vision.Speeding)
            {
                drivingEvent.EventType = "Speeding";
                drivingEvent.Penalty = 5;
            }
            else if (vision.TrafficLight == "none")
            {
                if (TrafficLightOnLastFrame)
                {
                    drivingEvent = HandleTrafficLight(LastTrafficLight);
                    drivingEvent.SessionId = sessionId; 
                }

                TrafficLightOnLastFrame = false;
            }
            else if (vision.TrafficLight != "none")
            {
                TrafficLightOnLastFrame = true;
                LastTrafficLight = vision.TrafficLight;
            }
        

            if(drivingEvent != null | drivingEvent.EventType != "") 
            {
                await _context.AddAsync(drivingEvent);
                await _context.SaveChangesAsync();
            }


           
        }


       





    }
}
