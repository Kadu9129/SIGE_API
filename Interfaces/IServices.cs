using SIGE.API.DTOs;
using SIGE.API.Models;

namespace SIGE.API.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
        Task<bool> LogoutAsync(string token);
        Task<string?> RefreshTokenAsync(string token);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
        Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<UsuarioDto?> GetCurrentUserAsync(int userId);
        Task<bool> VerifyTokenAsync(string token);
    }

    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioListDto>> GetAllAsync(int page = 1, int pageSize = 15, string? search = null);
        Task<UsuarioDto?> GetByIdAsync(int id);
        Task<UsuarioDto?> CreateAsync(CreateUsuarioDto createUsuarioDto);
        Task<UsuarioDto?> UpdateAsync(int id, UpdateUsuarioDto updateUsuarioDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateStatusAsync(int id, StatusUsuario status);
        Task<bool> UploadPhotoAsync(int id, IFormFile photo);
        Task<IEnumerable<string>> GetUserTypesAsync();
    }

    public interface IEscolaService
    {
        Task<IEnumerable<EscolaDto>> GetAllAsync(int page = 1, int pageSize = 15, string? search = null);
        Task<EscolaDto?> GetByIdAsync(int id);
        Task<EscolaDto?> CreateAsync(CreateEscolaDto createEscolaDto);
        Task<EscolaDto?> UpdateAsync(int id, UpdateEscolaDto updateEscolaDto);
        Task<bool> DeleteAsync(int id);
        Task<object> GetEstatisticasAsync(int id);
    }

    public interface IAlunoService
    {
        Task<ApiResponse<PaginatedResponse<AlunoDto>>> GetAlunosAsync(int page = 1, int pageSize = 10, string? search = null, int? escolaId = null, StatusAluno? status = null);
        Task<ApiResponse<AlunoDto>> GetAlunoByIdAsync(int id);
        Task<ApiResponse<AlunoDto>> CreateAlunoAsync(CreateAlunoDto createAlunoDto);
        Task<ApiResponse<AlunoDto>> UpdateAlunoAsync(int id, UpdateAlunoDto updateAlunoDto);
        Task<ApiResponse<bool>> DeleteAlunoAsync(int id);
        Task<ApiResponse<List<AlunoDto>>> GetAlunosByTurmaAsync(int turmaId);
        Task<ApiResponse<bool>> ChangeStatusAsync(int id, StatusAluno status);
    }

    public interface IProfessorService
    {
        Task<ApiResponse<PaginatedResponse<ProfessorDto>>> GetProfessoresAsync(int page = 1, int pageSize = 10, string? search = null, int? escolaId = null, StatusProfessor? status = null);
        Task<ApiResponse<ProfessorDto>> GetProfessorByIdAsync(int id);
        Task<ApiResponse<ProfessorDto>> CreateProfessorAsync(CreateProfessorDto createProfessorDto);
        Task<ApiResponse<ProfessorDto>> UpdateProfessorAsync(int id, UpdateProfessorDto updateProfessorDto);
        Task<ApiResponse<bool>> DeleteProfessorAsync(int id);
        Task<ApiResponse<List<DisciplinaDto>>> GetDisciplinasProfessorAsync(int id, int? anoLetivo = null);
        Task<ApiResponse<List<TurmaDto>>> GetTurmasProfessorAsync(int id, int? anoLetivo = null);
        Task<ApiResponse<bool>> AtribuirDisciplinaAsync(int professorId, int disciplinaId, int anoLetivo);
        Task<ApiResponse<bool>> RemoverDisciplinaAsync(int professorId, int disciplinaId, int anoLetivo);
        Task<ApiResponse<bool>> ChangeStatusAsync(int id, StatusProfessor status);
    }

    public interface ICursoService
    {
        Task<IEnumerable<CursoDto>> GetAllAsync(int page = 1, int pageSize = 15, string? search = null);
        Task<CursoDto?> GetByIdAsync(int id);
        Task<CursoDto?> CreateAsync(CreateCursoDto createCursoDto);
        Task<CursoDto?> UpdateAsync(int id, UpdateCursoDto updateCursoDto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<DisciplinaDto>> GetDisciplinasAsync(int id);
        Task<IEnumerable<object>> GetTurmasAsync(int id);
    }

    public interface IDisciplinaService
    {
        Task<IEnumerable<DisciplinaDto>> GetAllAsync(int page = 1, int pageSize = 15, string? search = null);
        Task<DisciplinaDto?> GetByIdAsync(int id);
        Task<DisciplinaDto?> CreateAsync(CreateDisciplinaDto createDisciplinaDto);
        Task<DisciplinaDto?> UpdateAsync(int id, UpdateDisciplinaDto updateDisciplinaDto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<object>> GetProfessoresAsync(int id);
        Task<IEnumerable<object>> GetHorariosAsync(int id);
    }
}