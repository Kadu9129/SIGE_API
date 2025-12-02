using AutoMapper;
using SIGE.API.DTOs;
using SIGE.API.Models;

namespace SIGE.API.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Usuario mappings
            CreateMap<Usuario, UsuarioDto>();
            CreateMap<Usuario, UsuarioListDto>()
                .ForMember(dest => dest.TipoUsuario, opt => opt.MapFrom(src => src.TipoUsuario.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            
            CreateMap<CreateUsuarioDto, Usuario>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SenhaHash, opt => opt.Ignore())
                .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
                .ForMember(dest => dest.DataUltimaAtualizacao, opt => opt.Ignore())
                .ForMember(dest => dest.FotoPerfil, opt => opt.Ignore());
            
            CreateMap<UpdateUsuarioDto, Usuario>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SenhaHash, opt => opt.Ignore())
                .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
                .ForMember(dest => dest.DataUltimaAtualizacao, opt => opt.Ignore())
                .ForMember(dest => dest.FotoPerfil, opt => opt.Ignore());

            // Escola mappings
            CreateMap<Escola, EscolaDto>()
                .ForMember(dest => dest.NomeDiretor, opt => opt.MapFrom(src => src.Diretor != null ? src.Diretor.Nome : null));
            
            CreateMap<CreateEscolaDto, Escola>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
            CreateMap<UpdateEscolaDto, Escola>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Curso mappings
            CreateMap<Curso, CursoDto>()
                .ForMember(dest => dest.NomeEscola, opt => opt.MapFrom(src => src.Escola.Nome));
            
            CreateMap<CreateCursoDto, Curso>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
            CreateMap<UpdateCursoDto, Curso>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Aluno mappings (consolidado)
            CreateMap<Aluno, AlunoDto>()
                .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => src.Usuario.Nome))
                .ForMember(dest => dest.EmailUsuario, opt => opt.MapFrom(src => src.Usuario.Email))
                .ForMember(dest => dest.FotoPerfil, opt => opt.MapFrom(src => src.Usuario.FotoPerfil))
                .ForMember(dest => dest.NomeEscola, opt => opt.MapFrom(src => src.Escola.Nome))
                .ForMember(dest => dest.Matriculas, opt => opt.MapFrom(src => src.Matriculas));

            CreateMap<CreateAlunoDto, Aluno>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioId, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.Escola, opt => opt.Ignore())
                .ForMember(dest => dest.Matriculas, opt => opt.Ignore())
                .ForMember(dest => dest.DataMatricula, opt => opt.Ignore());

            CreateMap<UpdateAlunoDto, Aluno>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioId, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.Escola, opt => opt.Ignore())
                .ForMember(dest => dest.Matricula, opt => opt.Ignore())
                .ForMember(dest => dest.Matriculas, opt => opt.Ignore())
                .ForMember(dest => dest.DataMatricula, opt => opt.Ignore());

            // Disciplina mappings
            CreateMap<Disciplina, DisciplinaDto>()
                .ForMember(dest => dest.NomeCurso, opt => opt.MapFrom(src => src.Curso.Nome));
            
            CreateMap<CreateDisciplinaDto, Disciplina>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
            CreateMap<UpdateDisciplinaDto, Disciplina>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // (Removido bloco duplicado de aluno)

            // Professor mappings
            CreateMap<Professor, ProfessorDto>()
                .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => src.Usuario.Nome))
                .ForMember(dest => dest.EmailUsuario, opt => opt.MapFrom(src => src.Usuario.Email))
                .ForMember(dest => dest.NomeEscola, opt => opt.MapFrom(src => src.Escola.Nome));
            
            CreateMap<CreateProfessorDto, Professor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DataAdmissao, opt => opt.MapFrom(src => DateTime.UtcNow));
            
            CreateMap<UpdateProfessorDto, Professor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioId, opt => opt.Ignore())
                .ForMember(dest => dest.DataAdmissao, opt => opt.Ignore());

            // (Removidos mapeamentos de Responsavel inexistentes nos DTOs atuais)

            // Turma mappings
            CreateMap<Turma, TurmaDto>()
                .ForMember(dest => dest.NomeCurso, opt => opt.MapFrom(src => src.Curso.Nome))
                .ForMember(dest => dest.NomeProfessorCoordenador, opt => opt.MapFrom(src => src.ProfessorCoordenador != null ? src.ProfessorCoordenador.NomeCompleto : null))
                .ForMember(dest => dest.Alunos, opt => opt.MapFrom(src => src.Matriculas));
            
            CreateMap<CreateTurmaDto, Turma>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
            CreateMap<UpdateTurmaDto, Turma>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Matricula, TurmaAlunoResumoDto>()
                .ForMember(dest => dest.NomeAluno, opt => opt.MapFrom(src => src.Aluno.NomeCompleto));

            // Matricula mappings
            CreateMap<Matricula, MatriculaDto>()
                .ForMember(dest => dest.NomeAluno, opt => opt.MapFrom(src => src.Aluno.NomeCompleto))
                .ForMember(dest => dest.MatriculaAluno, opt => opt.MapFrom(src => src.Aluno.Matricula))
                .ForMember(dest => dest.NomeTurma, opt => opt.MapFrom(src => src.Turma.Nome))
                .ForMember(dest => dest.CodigoTurma, opt => opt.MapFrom(src => src.Turma.Codigo));
            
            CreateMap<CreateMatriculaDto, Matricula>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DataMatricula, opt => opt.MapFrom(src => DateTime.UtcNow));
            
            CreateMap<UpdateMatriculaDto, Matricula>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DataMatricula, opt => opt.Ignore());

            // Avaliacao mappings
            CreateMap<Avaliacao, AvaliacaoDto>()
                .ForMember(dest => dest.NomeDisciplina, opt => opt.MapFrom(src => src.Disciplina.Nome))
                .ForMember(dest => dest.NomeTurma, opt => opt.MapFrom(src => src.Turma.Nome))
                .ForMember(dest => dest.NomeProfessor, opt => opt.MapFrom(src => src.Professor.NomeCompleto));
            
            CreateMap<CreateAvaliacaoDto, Avaliacao>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
            CreateMap<UpdateAvaliacaoDto, Avaliacao>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Nota mappings
            CreateMap<Nota, NotaDto>()
                .ForMember(dest => dest.NomeAvaliacao, opt => opt.MapFrom(src => src.Avaliacao.Nome))
                .ForMember(dest => dest.NomeAluno, opt => opt.MapFrom(src => src.Aluno.NomeCompleto))
                .ForMember(dest => dest.MatriculaAluno, opt => opt.MapFrom(src => src.Aluno.Matricula))
                .ForMember(dest => dest.NomeProfessor, opt => opt.MapFrom(src => src.Professor.NomeCompleto));
            
            CreateMap<CreateNotaDto, Nota>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DataLancamento, opt => opt.MapFrom(src => DateTime.UtcNow));
            
            CreateMap<UpdateNotaDto, Nota>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DataLancamento, opt => opt.Ignore());
        }
    }
}