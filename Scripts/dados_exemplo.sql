-- Script de dados iniciais para o SIGE
-- Execute após criar o banco de dados

USE sige_db;

-- Limpar dados existentes (exceto usuário admin)
DELETE FROM frequencias;
DELETE FROM notas;
DELETE FROM avaliacoes;
DELETE FROM chamadas;
DELETE FROM boletins;
DELETE FROM horarios;
DELETE FROM matriculas;
DELETE FROM turmas;
DELETE FROM professor_disciplina;
DELETE FROM professores;
DELETE FROM aluno_responsavel;
DELETE FROM responsaveis;
DELETE FROM alunos;
DELETE FROM disciplinas;
DELETE FROM cursos;
DELETE FROM escolas;
DELETE FROM mensagens;
DELETE FROM comunicados;
DELETE FROM financeiro_aluno;
DELETE FROM planos_pagamento;
DELETE FROM sessoes;
DELETE FROM usuarios WHERE id > 1; -- Manter apenas o admin

-- Reset identity
DBCC CHECKIDENT('escolas', RESEED, 0);
DBCC CHECKIDENT('cursos', RESEED, 0);
DBCC CHECKIDENT('disciplinas', RESEED, 0);
DBCC CHECKIDENT('usuarios', RESEED, 1);
DBCC CHECKIDENT('alunos', RESEED, 0);
DBCC CHECKIDENT('professores', RESEED, 0);
DBCC CHECKIDENT('responsaveis', RESEED, 0);
DBCC CHECKIDENT('turmas', RESEED, 0);

-- Inserir dados de exemplo

-- 1. USUÁRIOS
INSERT INTO usuarios (nome, email, senha_hash, tipo_usuario, status, data_criacao, data_ultima_atualizacao, telefone, cpf) VALUES
('João Silva', 'joao.silva@escola.com', '$2a$11$rQQQqQqQqQqQqQqQqQqQqOZk7k7k7k7k7k7k7k7k7k7k7k7k7k7k7', 'Diretor', 'Ativo', GETDATE(), GETDATE(), '(11) 99999-1001', '123.456.789-01'),
('Maria Santos', 'maria.santos@escola.com', '$2a$11$rQQQqQqQqQqQqQqQqQqQqOZk7k7k7k7k7k7k7k7k7k7k7k7k7k7k7', 'Professor', 'Ativo', GETDATE(), GETDATE(), '(11) 99999-1002', '123.456.789-02'),
('Pedro Costa', 'pedro.costa@escola.com', '$2a$11$rQQQqQqQqQqQqQqQqQqQqOZk7k7k7k7k7k7k7k7k7k7k7k7k7k7k7', 'Professor', 'Ativo', GETDATE(), GETDATE(), '(11) 99999-1003', '123.456.789-03'),
('Ana Oliveira', 'ana.oliveira@escola.com', '$2a$11$rQQQqQqQqQqQqQqQqQqQqOZk7k7k7k7k7k7k7k7k7k7k7k7k7k7k7', 'Professor', 'Ativo', GETDATE(), GETDATE(), '(11) 99999-1004', '123.456.789-04'),
('Carlos Pereira', 'carlos.pereira@email.com', '$2a$11$rQQQqQqQqQqQqQqQqQqQqOZk7k7k7k7k7k7k7k7k7k7k7k7k7k7k7', 'Aluno', 'Ativo', GETDATE(), GETDATE(), '(11) 99999-1005', '123.456.789-05'),
('Lucas Rodrigues', 'lucas.rodrigues@email.com', '$2a$11$rQQQqQqQqQqQqQqQqQqQqOZk7k7k7k7k7k7k7k7k7k7k7k7k7k7k7', 'Aluno', 'Ativo', GETDATE(), GETDATE(), '(11) 99999-1006', '123.456.789-06'),
('Fernanda Lima', 'fernanda.lima@email.com', '$2a$11$rQQQqQqQqQqQqQqQqQqQqOZk7k7k7k7k7k7k7k7k7k7k7k7k7k7k7', 'Aluno', 'Ativo', GETDATE(), GETDATE(), '(11) 99999-1007', '123.456.789-07'),
('Roberto Pereira', 'roberto.pereira@email.com', '$2a$11$rQQQqQqQqQqQqQqQqQqQqOZk7k7k7k7k7k7k7k7k7k7k7k7k7k7k7', 'Responsavel', 'Ativo', GETDATE(), GETDATE(), '(11) 99999-1008', '123.456.789-08'),
('Márcia Rodrigues', 'marcia.rodrigues@email.com', '$2a$11$rQQQqQqQqQqQqQqQqQqQqOZk7k7k7k7k7k7k7k7k7k7k7k7k7k7k7', 'Responsavel', 'Ativo', GETDATE(), GETDATE(), '(11) 99999-1009', '123.456.789-09'),
('Sandra Lima', 'sandra.lima@email.com', '$2a$11$rQQQqQqQqQqQqQqQqQqQqOZk7k7k7k7k7k7k7k7k7k7k7k7k7k7k7', 'Responsavel', 'Ativo', GETDATE(), GETDATE(), '(11) 99999-1010', '123.456.789-10');

-- 2. ESCOLAS
INSERT INTO escolas (nome, cnpj, endereco, cidade, estado, cep, telefone, email, diretor_id, status) VALUES
('Escola Estadual José de Alencar', '12.345.678/0001-90', 'Rua das Flores, 123', 'São Paulo', 'SP', '01234-567', '(11) 3333-4444', 'contato@escolajosealencar.edu.br', 2, 'Ativa'),
('Colégio Santa Maria', '98.765.432/0001-10', 'Av. Principal, 456', 'São Paulo', 'SP', '09876-543', '(11) 5555-6666', 'secretaria@colegiosantamaria.edu.br', 2, 'Ativa');

-- 3. CURSOS
INSERT INTO cursos (escola_id, nome, codigo, descricao, duracao_anos, nivel_ensino, status) VALUES
(1, 'Ensino Fundamental I', 'EF1', 'Ensino Fundamental do 1º ao 5º ano', 5, 'Fundamental1', 1),
(1, 'Ensino Fundamental II', 'EF2', 'Ensino Fundamental do 6º ao 9º ano', 4, 'Fundamental2', 1),
(1, 'Ensino Médio', 'EM', 'Ensino Médio regular', 3, 'Medio', 1),
(2, 'Ensino Fundamental I', 'EF1-SM', 'Ensino Fundamental do 1º ao 5º ano', 5, 'Fundamental1', 1),
(2, 'Ensino Médio Técnico', 'EMT', 'Ensino Médio com curso técnico', 3, 'Tecnico', 1);

-- 4. DISCIPLINAS
INSERT INTO disciplinas (nome, codigo, descricao, carga_horaria_total, curso_id, status) VALUES
-- Disciplinas do Ensino Fundamental I
('Português', 'PORT-EF1', 'Língua Portuguesa para Ensino Fundamental I', 160, 1, 1),
('Matemática', 'MAT-EF1', 'Matemática para Ensino Fundamental I', 160, 1, 1),
('Ciências', 'CIE-EF1', 'Ciências para Ensino Fundamental I', 80, 1, 1),
('História', 'HIS-EF1', 'História para Ensino Fundamental I', 80, 1, 1),
('Geografia', 'GEO-EF1', 'Geografia para Ensino Fundamental I', 80, 1, 1),

-- Disciplinas do Ensino Fundamental II
('Português', 'PORT-EF2', 'Língua Portuguesa para Ensino Fundamental II', 160, 2, 1),
('Matemática', 'MAT-EF2', 'Matemática para Ensino Fundamental II', 160, 2, 1),
('Ciências', 'CIE-EF2', 'Ciências para Ensino Fundamental II', 120, 2, 1),
('História', 'HIS-EF2', 'História para Ensino Fundamental II', 80, 2, 1),
('Geografia', 'GEO-EF2', 'Geografia para Ensino Fundamental II', 80, 2, 1),
('Inglês', 'ING-EF2', 'Língua Inglesa', 80, 2, 1),

-- Disciplinas do Ensino Médio
('Português', 'PORT-EM', 'Língua Portuguesa e Literatura', 120, 3, 1),
('Matemática', 'MAT-EM', 'Matemática para Ensino Médio', 120, 3, 1),
('Física', 'FIS-EM', 'Física', 80, 3, 1),
('Química', 'QUI-EM', 'Química', 80, 3, 1),
('Biologia', 'BIO-EM', 'Biologia', 80, 3, 1),
('História', 'HIS-EM', 'História', 80, 3, 1),
('Geografia', 'GEO-EM', 'Geografia', 80, 3, 1),
('Inglês', 'ING-EM', 'Língua Inglesa', 80, 3, 1);

-- 5. PROFESSORES
INSERT INTO professores (usuario_id, codigo_professor, nome_completo, cpf, rg, data_nascimento, formacao, especializacao, data_admissao, status, salario, carga_horaria_semanal, escola_id) VALUES
(3, 'PROF001', 'Maria Santos', '123.456.789-02', '12.345.678-9', '1985-03-15', 'Licenciatura em Matemática', 'Especialização em Educação Matemática', '2023-01-15', 'Ativo', 4500.00, 40, 1),
(4, 'PROF002', 'Pedro Costa', '123.456.789-03', '23.456.789-0', '1980-07-22', 'Licenciatura em Português', 'Mestrado em Literatura Brasileira', '2023-02-01', 'Ativo', 4800.00, 40, 1),
(5, 'PROF003', 'Ana Oliveira', '123.456.789-04', '34.567.890-1', '1988-11-10', 'Licenciatura em Ciências', 'Especialização em Ensino de Ciências', '2023-03-01', 'Ativo', 4200.00, 30, 1);

-- 6. PROFESSOR_DISCIPLINA (Relacionamento)
INSERT INTO professor_disciplina (professor_id, disciplina_id, ano_letivo, status) VALUES
-- Maria Santos (Matemática)
(1, 2, 2024, 1), -- Matemática EF1
(1, 7, 2024, 1), -- Matemática EF2
(1, 13, 2024, 1), -- Matemática EM

-- Pedro Costa (Português)
(2, 1, 2024, 1), -- Português EF1
(2, 6, 2024, 1), -- Português EF2
(2, 12, 2024, 1), -- Português EM

-- Ana Oliveira (Ciências)
(3, 3, 2024, 1), -- Ciências EF1
(3, 8, 2024, 1), -- Ciências EF2
(3, 15, 2024, 1); -- Biologia EM

-- 7. TURMAS
INSERT INTO turmas (codigo, nome, ano_letivo, serie, turno, capacidade_maxima, curso_id, professor_coordenador_id, sala, status) VALUES
('3A-2024', '3º Ano A', 2024, '3º Ano', 'Matutino', 30, 1, 1, 'Sala 101', 'Ativa'),
('7B-2024', '7º Ano B', 2024, '7º Ano', 'Matutino', 35, 2, 2, 'Sala 201', 'Ativa'),
('1EM-2024', '1º Ano EM', 2024, '1º Ano', 'Matutino', 40, 3, 3, 'Sala 301', 'Ativa');

-- 8. ALUNOS
INSERT INTO alunos (usuario_id, matricula, nome_completo, data_nascimento, sexo, rg, cpf, endereco, cidade, estado, cep, telefone_responsavel, email_responsavel, status, data_matricula, escola_id) VALUES
(6, 'ALU001', 'Carlos Pereira', '2015-04-10', 'M', '45.678.901-2', '123.456.789-05', 'Rua A, 100', 'São Paulo', 'SP', '12345-000', '(11) 99999-1008', 'roberto.pereira@email.com', 'Matriculado', '2024-01-15', 1),
(7, 'ALU002', 'Lucas Rodrigues', '2011-08-22', 'M', '56.789.012-3', '123.456.789-06', 'Rua B, 200', 'São Paulo', 'SP', '12345-001', '(11) 99999-1009', 'marcia.rodrigues@email.com', 'Matriculado', '2024-01-15', 1),
(8, 'ALU003', 'Fernanda Lima', '2008-12-05', 'F', '67.890.123-4', '123.456.789-07', 'Rua C, 300', 'São Paulo', 'SP', '12345-002', '(11) 99999-1010', 'sandra.lima@email.com', 'Matriculado', '2024-01-15', 1);

-- 9. RESPONSÁVEIS
INSERT INTO responsaveis (usuario_id, nome_completo, cpf, rg, telefone, email, endereco, parentesco, principal) VALUES
(9, 'Roberto Pereira', '123.456.789-08', '78.901.234-5', '(11) 99999-1008', 'roberto.pereira@email.com', 'Rua A, 100', 'Pai', 1),
(10, 'Márcia Rodrigues', '123.456.789-09', '89.012.345-6', '(11) 99999-1009', 'marcia.rodrigues@email.com', 'Rua B, 200', 'Mae', 1),
(11, 'Sandra Lima', '123.456.789-10', '90.123.456-7', '(11) 99999-1010', 'sandra.lima@email.com', 'Rua C, 300', 'Mae', 1);

-- 10. ALUNO_RESPONSAVEL (Relacionamentos)
INSERT INTO aluno_responsavel (aluno_id, responsavel_id, data_vinculo) VALUES
(1, 1, GETDATE()),
(2, 2, GETDATE()),
(3, 3, GETDATE());

-- 11. MATRÍCULAS
INSERT INTO matriculas (numero_matricula, aluno_id, turma_id, ano_letivo, data_matricula, status, observacoes) VALUES
('MAT001-2024', 1, 1, 2024, '2024-01-15', 'Ativa', 'Matrícula regular'),
('MAT002-2024', 2, 2, 2024, '2024-01-15', 'Ativa', 'Matrícula regular'),
('MAT003-2024', 3, 3, 2024, '2024-01-15', 'Ativa', 'Matrícula regular');

-- 12. HORÁRIOS
INSERT INTO horarios (turma_id, disciplina_id, professor_id, dia_semana, hora_inicio, hora_fim, sala) VALUES
-- 3º Ano A (Turma 1)
(1, 2, 1, 'Segunda', '08:00', '08:50', 'Sala 101'), -- Matemática
(1, 1, 2, 'Segunda', '09:00', '09:50', 'Sala 101'), -- Português
(1, 3, 3, 'Terca', '08:00', '08:50', 'Sala 101'),   -- Ciências

-- 7º Ano B (Turma 2)
(2, 7, 1, 'Segunda', '10:00', '10:50', 'Sala 201'), -- Matemática
(2, 6, 2, 'Segunda', '11:00', '11:50', 'Sala 201'), -- Português
(2, 8, 3, 'Terca', '10:00', '10:50', 'Sala 201'),   -- Ciências

-- 1º EM (Turma 3)
(3, 13, 1, 'Quarta', '08:00', '08:50', 'Sala 301'), -- Matemática
(3, 12, 2, 'Quarta', '09:00', '09:50', 'Sala 301'), -- Português
(3, 15, 3, 'Quinta', '08:00', '08:50', 'Sala 301'); -- Biologia

-- 13. AVALIAÇÕES
INSERT INTO avaliacoes (disciplina_id, turma_id, professor_id, nome, tipo, data_aplicacao, valor_maximo, peso, bimestre, status) VALUES
-- 1º Bimestre
(2, 1, 1, 'Prova de Matemática - 1º Bim', 'Prova', '2024-03-15', 10.0, 3.0, 1, 1),
(1, 1, 2, 'Prova de Português - 1º Bim', 'Prova', '2024-03-20', 10.0, 3.0, 1, 1),
(7, 2, 1, 'Prova de Matemática - 1º Bim', 'Prova', '2024-03-18', 10.0, 3.0, 1, 1),
(13, 3, 1, 'Prova de Matemática - 1º Bim', 'Prova', '2024-03-22', 10.0, 3.0, 1, 1);

-- 14. NOTAS
INSERT INTO notas (avaliacao_id, aluno_id, nota, observacoes, data_lancamento, professor_id) VALUES
-- Notas do Carlos (Aluno 1)
(1, 1, 8.5, 'Bom desempenho', GETDATE(), 1),
(2, 1, 7.0, 'Pode melhorar', GETDATE(), 2),

-- Notas do Lucas (Aluno 2)
(3, 2, 9.0, 'Excelente', GETDATE(), 1),

-- Notas da Fernanda (Aluno 3)
(4, 3, 8.0, 'Muito bom', GETDATE(), 1);

-- 15. COMUNICADOS
INSERT INTO comunicados (titulo, conteudo, autor_id, data_publicacao, data_expiracao, publico_alvo, prioridade, status, anexos) VALUES
('Reunião de Pais - 1º Bimestre', 'Comunicamos que a reunião de pais e mestres do 1º bimestre será realizada no dia 15/04/2024, às 19h00. Contamos com a presença de todos.', 2, GETDATE(), '2024-04-15', 'Responsaveis', 'Alta', 'Publicado', NULL),
('Semana da Consciência Negra', 'Durante a semana de 20 a 24/11/2024, realizaremos atividades especiais em comemoração ao Dia da Consciência Negra. Participem!', 2, GETDATE(), '2024-11-24', 'Todos', 'Media', 'Publicado', NULL);

-- 16. PLANOS DE PAGAMENTO
INSERT INTO planos_pagamento (nome, valor_mensalidade, descricao, curso_id, ativo) VALUES
('Plano Básico EF1', 350.00, 'Mensalidade para Ensino Fundamental I', 1, 1),
('Plano Básico EF2', 400.00, 'Mensalidade para Ensino Fundamental II', 2, 1),
('Plano Básico EM', 450.00, 'Mensalidade para Ensino Médio', 3, 1);

-- 17. FINANCEIRO ALUNO
INSERT INTO financeiro_aluno (aluno_id, plano_id, mes_referencia, valor_devido, valor_pago, data_vencimento, data_pagamento, status, observacoes) VALUES
-- Carlos Pereira (3º Ano - EF1)
(1, 1, '2024-01-01', 350.00, 350.00, '2024-01-10', '2024-01-08', 'Pago', 'Pagamento em dia'),
(1, 1, '2024-02-01', 350.00, 350.00, '2024-02-10', '2024-02-10', 'Pago', 'Pagamento em dia'),
(1, 1, '2024-03-01', 350.00, NULL, '2024-03-10', NULL, 'Pendente', 'Aguardando pagamento'),

-- Lucas Rodrigues (7º Ano - EF2)
(2, 2, '2024-01-01', 400.00, 400.00, '2024-01-10', '2024-01-09', 'Pago', 'Pagamento em dia'),
(2, 2, '2024-02-01', 400.00, 400.00, '2024-02-10', '2024-02-11', 'Pago', 'Pagamento 1 dia de atraso'),
(2, 2, '2024-03-01', 400.00, NULL, '2024-03-10', NULL, 'Pendente', 'Aguardando pagamento'),

-- Fernanda Lima (1º EM)
(3, 3, '2024-01-01', 450.00, 450.00, '2024-01-10', '2024-01-07', 'Pago', 'Pagamento antecipado'),
(3, 3, '2024-02-01', 450.00, 450.00, '2024-02-10', '2024-02-09', 'Pago', 'Pagamento em dia'),
(3, 3, '2024-03-01', 450.00, NULL, '2024-03-10', NULL, 'Pendente', 'Aguardando pagamento');

-- 18. FREQUÊNCIAS (Últimos 30 dias de aula)
DECLARE @DataInicio DATE = DATEADD(DAY, -30, GETDATE());
DECLARE @DataAtual DATE = @DataInicio;

WHILE @DataAtual <= GETDATE()
BEGIN
    -- Verificar se é dia útil (segunda a sexta)
    IF DATEPART(WEEKDAY, @DataAtual) BETWEEN 2 AND 6
    BEGIN
        -- Frequências para Carlos (Aluno 1)
        INSERT INTO frequencias (aluno_id, disciplina_id, data_aula, presente, justificativa, professor_id, horario_id)
        VALUES 
        (1, 2, @DataAtual, CASE WHEN RAND() > 0.1 THEN 1 ELSE 0 END, NULL, 1, 1),
        (1, 1, @DataAtual, CASE WHEN RAND() > 0.05 THEN 1 ELSE 0 END, NULL, 2, 2);
        
        -- Frequências para Lucas (Aluno 2)
        INSERT INTO frequencias (aluno_id, disciplina_id, data_aula, presente, justificativa, professor_id, horario_id)
        VALUES 
        (2, 7, @DataAtual, CASE WHEN RAND() > 0.08 THEN 1 ELSE 0 END, NULL, 1, 4),
        (2, 6, @DataAtual, CASE WHEN RAND() > 0.12 THEN 1 ELSE 0 END, NULL, 2, 5);
        
        -- Frequências para Fernanda (Aluno 3)
        INSERT INTO frequencias (aluno_id, disciplina_id, data_aula, presente, justificativa, professor_id, horario_id)
        VALUES 
        (3, 13, @DataAtual, CASE WHEN RAND() > 0.06 THEN 1 ELSE 0 END, NULL, 1, 7),
        (3, 12, @DataAtual, CASE WHEN RAND() > 0.04 THEN 1 ELSE 0 END, NULL, 2, 8);
    END
    
    SET @DataAtual = DATEADD(DAY, 1, @DataAtual);
END;

-- 19. CONFIGURAÇÕES SISTEMA (Adicionar mais configurações)
INSERT INTO configuracoes_sistema (chave, valor, descricao, categoria, data_alteracao, usuario_alteracao_id) VALUES
('ANO_LETIVO_ATUAL', '2024', 'Ano letivo atual do sistema', 'Academico', GETDATE(), 1),
('BIMESTRE_ATUAL', '1', 'Bimestre atual do sistema', 'Academico', GETDATE(), 1),
('NOTA_MINIMA_APROVACAO', '6.0', 'Nota mínima para aprovação', 'Avaliacao', GETDATE(), 1),
('FREQUENCIA_MINIMA', '75', 'Frequência mínima obrigatória (%)', 'Frequencia', GETDATE(), 1),
('DIAS_AVISO_VENCIMENTO', '5', 'Dias antes do vencimento para enviar aviso', 'Financeiro', GETDATE(), 1),
('EMAIL_SISTEMA', 'naoresponda@sige.edu.br', 'Email do sistema para envios automáticos', 'Email', GETDATE(), 1),
('BACKUP_AUTOMATICO', 'true', 'Ativar backup automático', 'Sistema', GETDATE(), 1),
('HORARIO_BACKUP', '02:00', 'Horário do backup automático', 'Sistema', GETDATE(), 1);

PRINT 'Dados de exemplo inseridos com sucesso!';
PRINT 'Total de escolas: ' + CAST((SELECT COUNT(*) FROM escolas) AS VARCHAR);
PRINT 'Total de usuários: ' + CAST((SELECT COUNT(*) FROM usuarios) AS VARCHAR);
PRINT 'Total de alunos: ' + CAST((SELECT COUNT(*) FROM alunos) AS VARCHAR);
PRINT 'Total de professores: ' + CAST((SELECT COUNT(*) FROM professores) AS VARCHAR);
PRINT 'Total de turmas: ' + CAST((SELECT COUNT(*) FROM turmas) AS VARCHAR);
PRINT 'Total de matrículas: ' + CAST((SELECT COUNT(*) FROM matriculas) AS VARCHAR);

-- Mostrar dados de login
PRINT '';
PRINT '=== DADOS DE LOGIN ===';
PRINT 'Admin: admin@sige.edu.br / 123456';
PRINT 'Diretor: joao.silva@escola.com / 123456';
PRINT 'Professor: maria.santos@escola.com / 123456';
PRINT 'Aluno: carlos.pereira@email.com / 123456';
PRINT 'Responsável: roberto.pereira@email.com / 123456';