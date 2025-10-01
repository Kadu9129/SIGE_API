# 🚀 INSTALAÇÃO E CONFIGURAÇÃO DO PROJETO SIGE API

## ⚠️ PRÉ-REQUISITOS

### 1. Instalar .NET 8.0 SDK
1. Acesse: https://dotnet.microsoft.com/download/dotnet/8.0
2. Baixe e instale o **.NET 8.0 SDK** (não apenas o Runtime)
3. Reinicie o terminal após a instalação
4. Verifique a instalação: `dotnet --version`

### 2. Instalar SQL Server
Escolha uma das opções:

#### Opção A - SQL Server LocalDB (Recomendado para desenvolvimento)
1. Baixe SQL Server Express: https://www.microsoft.com/sql-server/sql-server-downloads
2. Durante a instalação, marque a opção "LocalDB"

#### Opção B - SQL Server Express completo
1. Baixe e instale SQL Server Express
2. Configure com autenticação mista
3. Anote a string de conexão

#### Opção C - SQL Server Developer (Gratuito)
1. Baixe SQL Server Developer Edition
2. Instale com configurações padrão

### 3. Instalar Visual Studio Code (Opcional)
1. Baixe: https://code.visualstudio.com/
2. Instale as extensões:
   - C# (Microsoft)
   - C# Extensions (jchannon)
   - REST Client (humao)

## 📥 CLONE E CONFIGURAÇÃO

### 1. Clone o repositório
```bash
git clone <url-do-repositorio>
cd SIGE_API/SIGE_API
```

### 2. Verificar instalação do .NET
```bash
dotnet --version
# Deve retornar algo como: 8.0.100
```

### 3. Restaurar dependências
```bash
dotnet restore
```

### 4. Configurar string de conexão
Edite o arquivo `appsettings.json`:

Para **LocalDB**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=sige_db;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

Para **SQL Server Express**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=sige_db;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

Para **SQL Server com usuário/senha**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=sige_db;User Id=sa;Password=SuaSenha123;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

## 🗄️ CONFIGURAÇÃO DO BANCO DE DADOS

### 1. Instalar ferramentas do Entity Framework
```bash
dotnet tool install --global dotnet-ef
```

### 2. Verificar instalação do EF
```bash
dotnet ef --version
```

### 3. Criar o banco de dados
```bash
dotnet ef database update
```

### 4. Popular com dados de exemplo (Opcional)
Se você tiver o SQL Server Management Studio ou Azure Data Studio:
1. Conecte ao banco `sige_db`
2. Execute o script `Scripts/dados_exemplo.sql`

Ou via linha de comando:
```bash
sqlcmd -S "(localdb)\mssqllocaldb" -d sige_db -i Scripts/dados_exemplo.sql
```

## 🏃‍♂️ EXECUTAR A APLICAÇÃO

### 1. Executar em modo desenvolvimento
```bash
dotnet run
```

### 2. Executar com watch (recompila automaticamente)
```bash
dotnet watch run
```

### 3. Acessar a aplicação
- **Swagger UI**: https://localhost:5001 ou http://localhost:5000
- **API Base URL**: https://localhost:5001/api

## 🔑 PRIMEIRO ACESSO

### Credenciais padrão:
- **Email**: admin@sige.edu.br
- **Senha**: 123456

### Teste de login via curl:
```bash
curl -X POST "https://localhost:5001/api/auth/login" \
-H "Content-Type: application/json" \
-d "{\"email\":\"admin@sige.edu.br\",\"senha\":\"123456\"}"
```

### Teste via Swagger:
1. Acesse https://localhost:5001
2. Vá para `POST /api/auth/login`
3. Clique em "Try it out"
4. Use as credenciais acima
5. Copie o token retornado
6. Clique em "Authorize" no topo da página
7. Digite: `Bearer {seu-token}`

## 🛠️ COMANDOS ÚTEIS

### Build
```bash
dotnet build
```

### Executar testes
```bash
dotnet test
```

### Criar nova migração
```bash
dotnet ef migrations add NomeDaMigracao
```

### Aplicar migrações
```bash
dotnet ef database update
```

### Resetar banco de dados
```bash
dotnet ef database drop --force
dotnet ef database update
```

## 🐛 SOLUÇÃO DE PROBLEMAS COMUNS

### 1. "dotnet não é reconhecido"
**Problema**: .NET SDK não instalado ou não está no PATH
**Solução**: 
- Reinstale o .NET 8.0 SDK
- Reinicie o terminal/VS Code
- Verifique variáveis de ambiente

### 2. "Cannot create the database"
**Problema**: SQL Server não está executando
**Solução**:
- Inicie o SQL Server Service
- Verifique se o LocalDB está instalado: `sqllocaldb info`
- Para LocalDB: `sqllocaldb start mssqllocaldb`

### 3. "Invalid column name"
**Problema**: Banco desatualizado
**Solução**:
```bash
dotnet ef database drop --force
dotnet ef database update
```

### 4. "Certificate error"
**Problema**: Certificado HTTPS não confiável
**Solução**:
```bash
dotnet dev-certs https --trust
```

### 5. "Port already in use"
**Problema**: Porta 5000 ou 5001 em uso
**Solução**: Altere as portas no `launchSettings.json` ou mate o processo:
```bash
netstat -ano | findstr :5001
taskkill /PID {PID} /F
```

## 📊 ESTRUTURA DO BANCO CRIADO

Após executar as migrações, você terá:
- **26 tabelas** principais
- **8 módulos** funcionais
- **1 usuário administrador** padrão
- **Dados de exemplo** (se executar o script)

### Módulos:
1. Usuários e Autenticação
2. Institucional (Escolas, Cursos, Disciplinas)
3. Alunos e Responsáveis
4. Professores
5. Acadêmico (Turmas, Matrículas, Horários)
6. Notas e Avaliações
7. Frequência
8. Comunicação
9. Financeiro
10. Sistema (Configurações, Logs)

## 🔍 VERIFICAR SE ESTÁ FUNCIONANDO

### 1. Verificar banco de dados
```sql
-- Conectar e verificar tabelas
USE sige_db;
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';

-- Verificar usuário admin
SELECT * FROM usuarios WHERE email = 'admin@sige.edu.br';
```

### 2. Verificar API
- Acesse: https://localhost:5001/swagger
- Teste o endpoint de login
- Explore os outros endpoints

### 3. Verificar logs
- Logs aparecem no console durante execução
- Arquivos de log em `logs/` (se configurado)

## 📞 SUPORTE

Se encontrar problemas:
1. Verifique se seguiu todos os pré-requisitos
2. Consulte a seção de solução de problemas
3. Verifique os logs de erro
4. Crie uma issue no GitHub com detalhes do erro

---

**✅ Após seguir todos os passos, você terá uma API completa de gestão escolar rodando localmente!**