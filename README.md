# SIGE API - Sistema Integrado de Gestão Escolar

![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-green.svg)
![JWT](https://img.shields.io/badge/JWT-Authentication-orange.svg)
![Swagger](https://img.shields.io/badge/Swagger-Documentation-brightgreen.svg)

## 📌 IMPORTANTE - PRIMEIRA INSTALAÇÃO

**⚠️ ANTES DE COMEÇAR, LEIA O ARQUIVO [INSTALACAO.md](./INSTALACAO.md) PARA CONFIGURAÇÃO COMPLETA!**

Este projeto requer:
- **.NET 8.0 SDK** instalado
- **SQL Server** (LocalDB, Express ou Developer)
- **Entity Framework Tools** globais

## 🚀 Início Rápido

```bash
# 1. Instale .NET 8.0 SDK (se não tiver)
# Download: https://dotnet.microsoft.com/download/dotnet/8.0

# 2. Clone e configure
git clone <repositorio>
cd SIGE_API/SIGE_API

# 3. Restaurar dependências
dotnet restore

# 4. Instalar EF Tools
dotnet tool install --global dotnet-ef

# 5. Criar banco de dados
dotnet ef database update

# 6. Executar aplicação
dotnet run

# 7. Acessar Swagger
# https://localhost:5001
```

### Login Padrão
- **Email**: admin@sige.edu.br
- **Senha**: 123456

## Descrição

A SIGE API é uma API REST completa desenvolvida em .NET 8.0 com Entity Framework Core para gestão escolar. O sistema oferece funcionalidades completas para administração de escolas, incluindo gestão de usuários, alunos, professores, cursos, turmas, notas, frequência, comunicação e financeiro.

## 🚀 Características

- **Arquitetura**: Clean Architecture com Repository Pattern
- **Autenticação**: JWT Bearer Token
- **Banco de Dados**: SQL Server com Entity Framework Core
- **Documentação**: Swagger/OpenAPI
- **Mapeamento**: AutoMapper
- **Validação**: FluentValidation + Data Annotations
- **Logging**: Serilog
- **CORS**: Configurado para aplicações Angular/React
- **Upload**: Sistema de upload de arquivos
- **Criptografia**: BCrypt para senhas

## 📋 Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) ou SQL Server LocalDB
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

## 🔧 Instalação

### 1. Clone o repositório
```bash
git clone https://github.com/seu-usuario/sige-api.git
cd sige-api
```

### 2. Restaure os pacotes NuGet
```bash
dotnet restore
```

### 3. Configure a conexão com o banco
Edite o arquivo `appsettings.json` com sua string de conexão:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=sige_db;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

### 4. Execute as migrações do banco
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Execute a aplicação
```bash
dotnet run
```

A API estará disponível em:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger**: `https://localhost:5001` (página inicial)

## 📊 Estrutura do Banco de Dados

O sistema possui 26 tabelas organizadas em 8 módulos:

### Módulos
1. **Usuários e Autenticação** (3 tabelas)
2. **Institucional** (3 tabelas)
3. **Alunos** (3 tabelas)
4. **Professores** (2 tabelas)
5. **Acadêmico** (3 tabelas)
6. **Notas e Avaliações** (3 tabelas)
7. **Frequência** (2 tabelas)
8. **Comunicação** (2 tabelas)
9. **Financeiro** (2 tabelas)
10. **Sistema** (3 tabelas)

## 🔑 Autenticação

### Login Padrão
```json
{
  "email": "admin@sige.edu.br",
  "senha": "123456"
}
```

### Endpoint de Login
```
POST /api/auth/login
```

### Headers para Rotas Protegidas
```
Authorization: Bearer {seu-jwt-token}
Content-Type: application/json
```

## 📚 Principais Endpoints

### Autenticação
```
POST   /api/auth/login              - Login
POST   /api/auth/logout             - Logout
POST   /api/auth/refresh            - Renovar token
POST   /api/auth/change-password    - Alterar senha
GET    /api/auth/me                 - Dados do usuário logado
```

### Usuários
```
GET    /api/usuarios                - Listar usuários
GET    /api/usuarios/{id}           - Obter usuário
POST   /api/usuarios                - Criar usuário
PUT    /api/usuarios/{id}           - Atualizar usuário
DELETE /api/usuarios/{id}           - Deletar usuário
PATCH  /api/usuarios/{id}/status    - Alterar status
POST   /api/usuarios/{id}/foto      - Upload foto
```

### Escolas
```
GET    /api/escolas                 - Listar escolas
GET    /api/escolas/{id}            - Obter escola
POST   /api/escolas                 - Criar escola
PUT    /api/escolas/{id}            - Atualizar escola
DELETE /api/escolas/{id}            - Deletar escola
GET    /api/escolas/{id}/estatisticas - Estatísticas
```

### Cursos
```
GET    /api/cursos                  - Listar cursos
GET    /api/cursos/{id}             - Obter curso
POST   /api/cursos                  - Criar curso
PUT    /api/cursos/{id}             - Atualizar curso
DELETE /api/cursos/{id}             - Deletar curso
```

## 📝 Exemplos de Uso

### Login
```bash
curl -X POST "https://localhost:5001/api/auth/login" \
-H "Content-Type: application/json" \
-d '{
  "email": "admin@sige.edu.br",
  "senha": "123456"
}'
```

### Criar Usuário
```bash
curl -X POST "https://localhost:5001/api/usuarios" \
-H "Authorization: Bearer {token}" \
-H "Content-Type: application/json" \
-d '{
  "nome": "João Silva",
  "email": "joao@escola.com",
  "senha": "123456",
  "tipoUsuario": "Professor",
  "telefone": "(11) 99999-9999",
  "cpf": "123.456.789-00"
}'
```

### Listar Usuários com Filtro
```bash
curl -X GET "https://localhost:5001/api/usuarios?page=1&pageSize=10&search=João" \
-H "Authorization: Bearer {token}"
```

## 🗂️ Estrutura do Projeto

```
SIGE.API/
├── Controllers/          # Controladores da API
├── Models/              # Entidades do banco de dados
├── DTOs/                # Data Transfer Objects
├── Services/            # Lógica de negócio
├── Interfaces/          # Contratos dos serviços
├── Data/                # DbContext e configurações
├── Mappings/            # Perfis do AutoMapper
├── Migrations/          # Migrações do Entity Framework
└── wwwroot/            # Arquivos estáticos (uploads)
```

## 🛠️ Tecnologias Utilizadas

### Backend
- **.NET 8.0** - Framework principal
- **ASP.NET Core** - Web API
- **Entity Framework Core** - ORM
- **SQL Server** - Banco de dados
- **AutoMapper** - Mapeamento de objetos
- **JWT** - Autenticação
- **BCrypt** - Criptografia de senhas
- **Swagger** - Documentação da API
- **Serilog** - Logging

### Pacotes NuGet
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.8" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.8" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.8" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
```

## 🔒 Segurança

- **JWT Authentication**: Tokens com expiração de 8 horas
- **Password Hashing**: BCrypt com salt automático
- **CORS**: Configurado para origins específicos
- **Input Validation**: Data Annotations + FluentValidation
- **SQL Injection**: Proteção via Entity Framework
- **Authorization**: Role-based access control

## 📊 Módulos do Sistema

### 1. Usuários e Autenticação
- Gestão de usuários (Admin, Diretor, Professor, Aluno, Responsável)
- Sistema de login/logout
- Perfis de acesso e permissões
- Controle de sessões

### 2. Institucional
- Cadastro de escolas
- Gestão de cursos
- Disciplinas por curso
- Hierarquia institucional

### 3. Gestão de Alunos
- Cadastro completo de alunos
- Vínculos com responsáveis
- Histórico escolar
- Status acadêmico

### 4. Gestão de Professores
- Cadastro de professores
- Formação e especializações
- Disciplinas lecionadas
- Carga horária

### 5. Módulo Acadêmico
- Criação de turmas
- Sistema de matrículas
- Grade de horários
- Coordenação de turmas

### 6. Notas e Avaliações
- Tipos de avaliação
- Lançamento de notas
- Boletins automáticos
- Cálculo de médias

### 7. Controle de Frequência
- Chamadas eletrônicas
- Registro de presenças/faltas
- Justificativas
- Relatórios de frequência

### 8. Comunicação
- Sistema de comunicados
- Mensagens internas
- Notificações
- Anexos de arquivos

### 9. Gestão Financeira
- Planos de pagamento
- Controle de mensalidades
- Status de pagamentos
- Relatórios financeiros

### 10. Relatórios e Configurações
- Geração de relatórios
- Configurações do sistema
- Logs de auditoria
- Backup de dados

## 🚀 Deploy

### Desenvolvimento
```bash
dotnet run --environment Development
```

### Produção
```bash
dotnet publish -c Release -o ./publish
```

### Docker (Opcional)
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SIGE.API.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SIGE.API.dll"]
```

## 🧪 Testes

### Executar testes
```bash
dotnet test
```

### Coverage
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## 📈 Performance

- **Paginação**: Implementada em todas as listagens
- **Lazy Loading**: Configurado no Entity Framework
- **Caching**: Preparado para implementação
- **Índices**: Otimizados para consultas frequentes

## 🔄 Versionamento

A API segue o padrão Semantic Versioning (SemVer):
- **MAJOR**: Mudanças incompatíveis
- **MINOR**: Funcionalidades compatíveis
- **PATCH**: Correções de bugs

Versão atual: **1.0.0**

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch (`git checkout -b feature/nova-funcionalidade`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova funcionalidade'`)
4. Push para a branch (`git push origin feature/nova-funcionalidade`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para detalhes.

## 👥 Equipe

- **Backend**: .NET Core API
- **Frontend**: Angular (repositório separado)
- **Database**: SQL Server
- **DevOps**: Azure/AWS (em planejamento)

## 📞 Suporte

Para suporte técnico:
- **Email**: admin@sige.edu.br
- **Issues**: GitHub Issues
- **Wiki**: Documentação completa

## 🗓️ Roadmap

### Versão 1.1.0
- [ ] Sistema de relatórios avançados
- [ ] Notificações em tempo real
- [ ] Mobile API
- [ ] Integração com sistemas externos

### Versão 1.2.0
- [ ] Dashboard analytics
- [ ] Sistema de backup automático
- [ ] Multi-tenancy
- [ ] API Gateway

### Versão 2.0.0
- [ ] Microserviços
- [ ] Event Sourcing
- [ ] Machine Learning para predições
- [ ] Mobile App nativo

---

**Desenvolvido com ❤️ para a educação brasileira**