# SIGE API - Comandos Úteis

## Comandos .NET

### Restaurar pacotes
```bash
dotnet restore
```

### Executar aplicação
```bash
dotnet run
```

### Executar em modo watch (desenvolvimento)
```bash
dotnet watch run
```

### Build da aplicação
```bash
dotnet build
```

### Build de release
```bash
dotnet build -c Release
```

### Publicar aplicação
```bash
dotnet publish -c Release -o ./publish
```

## Entity Framework

### Adicionar nova migração
```bash
dotnet ef migrations add NomeDaMigracao
```

### Atualizar banco de dados
```bash
dotnet ef database update
```

### Remover última migração
```bash
dotnet ef migrations remove
```

### Gerar script SQL
```bash
dotnet ef migrations script
```

### Criar banco de dados
```bash
dotnet ef database create
```

### Deletar banco de dados
```bash
dotnet ef database drop
```

### Ver migrações
```bash
dotnet ef migrations list
```

### Reverter para migração específica
```bash
dotnet ef database update NomeDaMigracao
```

## Comandos de Desenvolvimento

### Instalar ferramentas do EF
```bash
dotnet tool install --global dotnet-ef
```

### Atualizar ferramentas do EF
```bash
dotnet tool update --global dotnet-ef
```

### Limpar build
```bash
dotnet clean
```

### Executar testes
```bash
dotnet test
```

### Executar testes com coverage
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Adicionar pacote NuGet
```bash
dotnet add package NomeDoPackage
```

### Remover pacote NuGet
```bash
dotnet remove package NomeDoPackage
```

### Listar pacotes instalados
```bash
dotnet list package
```

### Verificar pacotes desatualizados
```bash
dotnet list package --outdated
```

## URLs da Aplicação

### Desenvolvimento
- HTTP: http://localhost:5000
- HTTPS: https://localhost:5001
- Swagger: https://localhost:5001/swagger

### Endpoints Importantes
- Login: POST /api/auth/login
- Usuários: GET /api/usuarios
- Dashboard: GET /api/dashboard/geral

## Comandos SQL Server

### Conectar ao LocalDB
```bash
sqlcmd -S "(localdb)\mssqllocaldb"
```

### Backup do banco
```sql
BACKUP DATABASE sige_db TO DISK = 'C:\backup\sige_db.bak'
```

### Restaurar banco
```sql
RESTORE DATABASE sige_db FROM DISK = 'C:\backup\sige_db.bak'
```

### Ver bancos
```sql
SELECT name FROM sys.databases
```

### Usar banco
```sql
USE sige_db
```

### Ver tabelas
```sql
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'
```

## Docker (Opcional)

### Build da imagem
```bash
docker build -t sige-api .
```

### Executar container
```bash
docker run -p 8080:80 sige-api
```

### Executar com SQL Server
```bash
docker-compose up
```

## Comandos Git

### Inicializar repositório
```bash
git init
```

### Adicionar arquivos
```bash
git add .
```

### Commit
```bash
git commit -m "Initial commit"
```

### Adicionar remote
```bash
git remote add origin https://github.com/usuario/sige-api.git
```

### Push
```bash
git push -u origin main
```

## Variáveis de Ambiente

### Development
```bash
set ASPNETCORE_ENVIRONMENT=Development
set ConnectionStrings__DefaultConnection="Server=(localdb)\mssqllocaldb;Database=sige_db_dev;Trusted_Connection=true"
```

### Production
```bash
set ASPNETCORE_ENVIRONMENT=Production
set ConnectionStrings__DefaultConnection="Server=servidor;Database=sige_db;User Id=user;Password=pass"
```

## Logs

### Ver logs em tempo real
```bash
tail -f logs/sige-api.log
```

### Logs do sistema (Windows)
```bash
Get-EventLog -LogName Application -Source "SIGE.API"
```

## Performance

### Monitorar performance
```bash
dotnet counters monitor --process-name SIGE.API
```

### Dump de memória
```bash
dotnet dump collect --process-name SIGE.API
```

## Troubleshooting

### Limpar cache do NuGet
```bash
dotnet nuget locals all --clear
```

### Verificar versão do .NET
```bash
dotnet --version
```

### Verificar SDKs instalados
```bash
dotnet --list-sdks
```

### Verificar runtimes instalados
```bash
dotnet --list-runtimes
```

### Informações sobre o projeto
```bash
dotnet --info
```

## Scripts Úteis

### Script para reset completo do banco
```bash
dotnet ef database drop --force
dotnet ef migrations remove --force
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Script para popular banco com dados de exemplo
```bash
sqlcmd -S "(localdb)\mssqllocaldb" -d sige_db -i Scripts/dados_exemplo.sql
```