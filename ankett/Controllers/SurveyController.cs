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

    // Anket oluştur (Sadece Admin yetkisi olanlar)
    [HttpPost("create")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateSurvey([FromBody] Survey survey)
    {
        if (survey == null || string.IsNullOrWhiteSpace(survey.Title))
        {
            return BadRequest("Anket başlığı boş olamaz.");
        }

        survey.CreatedAt = DateTime.UtcNow;
        _context.Surveys.Add(survey);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Anket başarıyla oluşturuldu.", survey.Id });
    }

    // Tüm anketleri getir
    [HttpGet]
    public async Task<IActionResult> GetAllSurveys()
    {
        var surveys = await _context.Surveys.ToListAsync();
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

}
