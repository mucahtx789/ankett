using ankett.Models;
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

        [HttpPost]
        public async Task<IActionResult> SubmitAnswer([FromBody] AnswerDto answerDto)
        {
            var employee = await _context.Users.FirstOrDefaultAsync(u => u.Id == answerDto.EmployeeId && u.Role == UserRole.Employee);
            if (employee == null) return Unauthorized();

            var answer = new Answer
            {
                EmployeeId = answerDto.EmployeeId,
                OptionId = answerDto.OptionId
            };

            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();

            return Ok(answer);
        }

        [HttpGet("my-answers/{employeeId}")]
        public async Task<IActionResult> GetAnswers(int employeeId)
        {
            var answers = await _context.Answers
                .Include(a => a.Option)
                .ThenInclude(o => o.Question)
                .Where(a => a.EmployeeId == employeeId)
                .ToListAsync();

            return Ok(answers);
        }
    }

    public class AnswerDto
    {
        public int EmployeeId { get; set; }
        public int OptionId { get; set; }
    }
}
