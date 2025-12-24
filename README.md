# 💰 Pacovallet - Sistema de Gestão Financeira

Pacovallet é uma aplicação completa para gestão financeira pessoal, permitindo o controle de receitas, despesas, categorias e relatórios financeiros de forma simples e eficiente.

## 🎯 Finalidade

Este projeto tem como objetivo fornecer uma ferramenta intuitiva para:

- **Controle de Transações**: Registrar e gerenciar receitas e despesas
- **Categorização**: Organizar transações por categorias personalizadas
- **Gestão de Pessoas**: Controle de usuários e perfis
- **Autenticação Segura**: Sistema de login e autorização com JWT

## 🛠️ Tecnologias Utilizadas

### Backend (.NET Core)
- **ASP.NET Core** - Framework web
- **Entity Framework Core** - ORM para acesso a dados
- **ASP.NET Identity** - Sistema de autenticação e autorização
- **JWT (JSON Web Tokens)** - Autenticação stateless
- **Swagger/OpenAPI** - Documentação da API
- **PostgreSql** - Banco de dados
- **Docker** - Containerização

### Frontend (React)
- **React** - Biblioteca para interfaces de usuário
- **TypeScript** - Superset tipado do JavaScript
- **CSS3** - Estilização
- **React Router** - Roteamento client-side

## 🚀 Como Executar

### Pré-requisitos

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js 22+](https://nodejs.org/)
- [PostgreSql](https://www.postgresql.org/)
- [Docker](https://www.docker.com/) (opcional)

### Executando a API (.NET Core)

1. Navegue até o diretório da API:
```bash
cd Api/Pacovallet.Api
```

2. Restaure as dependências:
```bash
dotnet restore
```

3. Configure a string de conexão no arquivo `appsettings.json`

4. Execute as migrações do banco de dados:
```bash
dotnet ef database update
```

5. Execute a aplicação:
```bash
dotnet run
```

A API estará disponível em `https://localhost:7220` (HTTPS) ou `http://localhost:5064` (HTTP).
A documentação Swagger estará acessível em `https://localhost:7220/swagger`.

### Executando o Frontend (React)

1. Navegue até o diretório do frontend:
```bash
cd Front
```

2. Instale as dependências:
```bash
npm install
```

3. Execute a aplicação:
```bash
npm start
```

O frontend estará disponível em `http://localhost:3000`.

### Executando com Docker

1. Na raiz do projeto, construa e execute os containers:
```bash
docker-compose up --build
```

## 🤝 Contribuição

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença especificada no arquivo [LICENSE](LICENSE).

---

Desenvolvido com ❤️ para facilitar sua gestão financeira.