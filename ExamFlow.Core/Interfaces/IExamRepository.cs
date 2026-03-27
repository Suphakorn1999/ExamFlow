using ExamFlow.Core.Models;
using System.Diagnostics.Contracts;

namespace ExamFlow.Core.Interfaces;

public interface IExamRepository
{
    Task<IEnumerable<ExamQuestion>> GetAllAsync();
    Task<ExamQuestion?> GetByIdAsync(int id);
    Task<ExamQuestion> CreateAsync(ExamQuestion question);
    Task DeleteAsync(int id);
}