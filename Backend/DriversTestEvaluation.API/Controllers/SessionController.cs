using DriversTestEvaluation.Core.DTOs;
using DriversTestEvaluation.Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace DriversTestEvaluation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _service;


        public SessionController(ISessionService service)
        {
            _service = service;

        }

        [HttpPost("start")]

        public async Task<ActionResult<Guid>> StartTest()
        {
            return await _service.StartTest();
        }

        [HttpPost("{id}/stop")]

        public async Task StopTest(Guid id)
        {
            await _service.StopTest(id);
        }

        [HttpGet("{id}/getUpdate")]

        public async Task<ActionResult<UpdateDto>> GetUpdate(Guid id)
        {

            return await _service.GetUpdate(id);
        }

        [HttpGet("{id}/results")]

        public async Task<ActionResult<ResultsResponseDto>> getResults(Guid id)
        {
            return await _service.GetResults(id);
        }


       
    }
}