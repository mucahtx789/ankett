using ankett.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ankett.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnswerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AnswerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("submit")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> SubmitAnswers([FromBody] List<Answer> answers)
        {
            if (answers == null || answers.Count == 0)
            {
                return BadRequest("Yanıtlar boş olamaz.");
            }

            _context.Answers.AddRange(answers);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cevaplar başarıyla kaydedildi." });
        }


    }

    public class AnswerDto
    {
        public int EmployeeId { get; set; }
        public int OptionId { get; set; }
    }
}
