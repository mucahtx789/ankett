using ankett.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ankett.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SurveyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SurveyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // sadece admin yetkisi olanlar
        public async Task<IActionResult> CreateSurvey([FromBody] Survey survey)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            survey.CreatedAt = DateTime.UtcNow;
            _context.Surveys.Add(survey);
            await _context.SaveChangesAsync();

            return Ok(survey);
        }

        [HttpGet("{surveyId}")]
        public async Task<IActionResult> GetSurvey(int surveyId)
        {
            var survey = await _context.Surveys
                .Include(s => s.Questions)
                .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(s => s.Id == surveyId);

            if (survey == null) return NotFound();

            return Ok(survey);
        }
    }

}
