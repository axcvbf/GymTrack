using GymTrack.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymTrack.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiStatsController : ControllerBase
    {
        private readonly IStatsService _statsService;

        public ApiStatsController(IStatsService statsService)
        {
            _statsService = statsService;
        }

        [HttpGet("{exerciseId}")]
        public async Task<IActionResult> GetExerciseStats(int exerciseId)
        {
            var stats = await _statsService.GetExerciseProgressAsync(exerciseId);

            var result = stats.Select(s => new
            {
                Date = s.Date.ToString("yyyy-MM-dd"),
                Weight = s.Weight
            });

            return Ok(result);
        }
    }
}
