using ankett.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/survey")]
[ApiController]
public class SurveyController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SurveyController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateSurvey([FromBody] SurveyCreateRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Title) || !request.Questions.Any())
        {
            return BadRequest("Geçersiz anket verisi.");
        }

        var survey = new Survey
        {
            Title = request.Title,
            CreatedAt = DateTime.Now
        };

        _context.Surveys.Add(survey);
        await _context.SaveChangesAsync();

        foreach (var questionRequest in request.Questions)
        {
            var question = new Question
            {
                Text = questionRequest.Text,
                SurveyId = survey.Id
            };

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            foreach (var optionRequest in questionRequest.Options)
            {
                var option = new Option
                {
                    Text = optionRequest.Text,
                    QuestionId = question.Id
                };

                _context.Options.Add(option);
                await _context.SaveChangesAsync();
            }
        }

        return Ok(new { Message = "Anket başarıyla oluşturuldu." });
    }


    // Tüm anketleri getir (Soruları ve seçenekleri dahil ederek)
    [HttpGet]
    public async Task<IActionResult> GetAllSurveys()
    {
        var surveys = await _context.Surveys
            .Include(s => s.Questions)
            .ThenInclude(q => q.Options)
            .Select(s => new
            {
                s.Id,
                s.Title,
                s.CreatedAt // CreatedAt'ı da döndürüyoruz
            })
            .ToListAsync();

        return Ok(surveys);
    }

    // Belirtilen anketin detaylarını getir
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSurveyById(int id)
    {
        var survey = await _context.Surveys
            .Include(s => s.Questions)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (survey == null)
        {
            return NotFound("Anket bulunamadı.");
        }

        return Ok(survey);
    }

    // Employee için çözülen anketleri kontrol et
    [HttpGet("completed/{employeeId}")]
    public async Task<IActionResult> GetCompletedSurveys(int employeeId)
    {
        var completedSurveys = await _context.Answers
            .Where(a => a.EmployeeId == employeeId)
            .Select(a => a.Option.Question.SurveyId)
            .Distinct()
            .ToListAsync();

        return Ok(completedSurveys);
    }
}
public class SurveyCreateRequest
{
    public string Title { get; set; }
    public List<QuestionCreateRequest> Questions { get; set; }
}

public class QuestionCreateRequest
{
    public string Text { get; set; }
    public List<OptionCreateRequest> Options { get; set; }
}

public class OptionCreateRequest
{
    public string Text { get; set; }
}