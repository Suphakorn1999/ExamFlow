using ExamFlow.Core.DTOs;
using ExamFlow.Core.Interfaces;

namespace ExamFlow.Api.Endpoints;

public static class ExamEndpoints
{
    public static void MapExamEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/examquestions")
                       .WithTags("ExamQuestions")
                       .WithOpenApi();

        group.MapGet("/", GetAll);
        group.MapGet("/{id}", GetById);
        group.MapPost("/", Create);
        group.MapDelete("/{id}", Delete);
    }

    private static async Task<IResult> GetAll(IExamRepository repo)
    {
        var questions = await repo.GetAllAsync();
        return Results.Ok(questions);
    }

    private static async Task<IResult> GetById(int id, IExamRepository repo)
    {
        var question = await repo.GetByIdAsync(id);
        return question is null
            ? Results.NotFound(new { message = $"ไม่พบข้อสอบ id {id}" })
            : Results.Ok(question);
    }

    private static async Task<IResult> Create(
        CreateExamQuestionDto dto,
        IExamRepository repo)
    {
        if (string.IsNullOrWhiteSpace(dto.QuestionText))
            return Results.BadRequest(new { message = "กรุณากรอกคำถาม" });

        var created = await repo.CreateAsync(new()
        {
            QuestionText = dto.QuestionText,
            Choice1 = dto.Choice1,
            Choice2 = dto.Choice2,
            Choice3 = dto.Choice3,
            Choice4 = dto.Choice4,
        });

        return Results.Created($"/api/examquestions/{created.Id}", created);
    }

    private static async Task<IResult> Delete(int id, IExamRepository repo)
    {
        var question = await repo.GetByIdAsync(id);
        if (question is null)
            return Results.NotFound(new { message = $"ไม่พบข้อสอบ id {id}" });

        await repo.DeleteAsync(id);
        return Results.NoContent();
    }
}