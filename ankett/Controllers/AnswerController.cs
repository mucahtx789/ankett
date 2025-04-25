using ankett.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ankett.Models.Dto;
namespace ankett.Controllers
{
    [ApiController]
    [Route("api/answer")] //or [Route("api/[controller]")]  [controller]=answer

    public class AnswerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AnswerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitAnswers([FromBody] List<AnswerSubmitRequest> answers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (answers == null || !answers.Any())
            {
                return BadRequest(new { Success = false, Message = "Answers cannot be empty." });
            }

            foreach (var answer in answers)
            {
                var answerEntity = new Answer
                {
                    EmployeeId = answer.EmployeeId,
                    OptionId = answer.OptionId
                };

                _context.Answers.Add(answerEntity);
            }

            await _context.SaveChangesAsync();
            return Ok(new { Success = true, Message = "Answers saved successfully." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSurvey(int id)
        {
            var survey = await _context.Surveys
                .Where(s => s.Id == id)
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    Questions = s.Questions.Select(q => new
                    {
                        q.Id,
                        q.Text,
                        Options = q.Options.Select(o => new
                        {
                            o.Id,
                            o.Text
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (survey == null)
            {
                throw new KeyNotFoundException("No survey found.");
            }

            return Ok(survey);
        }
    }



}



