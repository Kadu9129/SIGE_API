using System.ComponentModel.DataAnnotations;

namespace SIGE.API.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public object? Errors { get; set; }
        public int? StatusCode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static ApiResponse<T> Ok(T? data, string message = "", int? statusCode = null)
            => new ApiResponse<T> { Success = true, Data = data, Message = message, StatusCode = statusCode };

        public static ApiResponse<T> Error(string message, int? statusCode = null, object? errors = null)
            => new ApiResponse<T> { Success = false, Message = message, StatusCode = statusCode, Errors = errors };
    }

    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; } = new();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage => CurrentPage < TotalPages;
        public bool HasPreviousPage => CurrentPage > 1;
    }

    public class PaginationParameters
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page deve ser maior que 0")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "PageSize deve estar entre 1 e 100")]
        public int PageSize { get; set; } = 15;

        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; } = "asc"; // asc ou desc
    }

    public class ErrorDto
    {
        public string Message { get; set; } = string.Empty;
        public string? Detail { get; set; }
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? TraceId { get; set; }
    }

    public class ValidationErrorDto
    {
        public string Field { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class DashboardDto
    {
        public int TotalEscolas { get; set; }
        public int TotalAlunos { get; set; }
        public int TotalProfessores { get; set; }
        public int TotalTurmas { get; set; }
        public int TotalUsuarios { get; set; }
        public decimal PercentualFrequencia { get; set; }
        public decimal MediaGeralNotas { get; set; }
        public List<object> GraficoMatriculas { get; set; } = new();
        public List<object> GraficoFinanceiro { get; set; } = new();
        public List<object> AlertasRecentes { get; set; } = new();
    }

    public class EstatisticasDto
    {
        public string Categoria { get; set; } = string.Empty;
        public int Total { get; set; }
        public int Ativos { get; set; }
        public int Inativos { get; set; }
        public decimal Percentual { get; set; }
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
    }
}