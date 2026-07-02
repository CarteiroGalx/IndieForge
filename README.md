# 🔨 IndieForge

![Status](https://img.shields.io/badge/status-em%20desenvolvimento-yellow) ![.NET](https://img.shields.io/badge/.NET-8-blue) ![License](https://img.shields.io/badge/license-MIT-green)

O IndieForge é uma plataforma de arrecadação voltada para desenvolvedores independentes de jogos digitais. Através dela, criadores podem apresentar projetos ainda em desenvolvimento para a comunidade, arrecadando recursos financeiros por meio de um sistema de apoio e pré-venda.
Em troca, os apoiadores podem receber recompensas definidas pelos próprios desenvolvedores, incentivando a participação da comunidade e ajudando a financiar a conclusão dos projetos.

## 🤔 Como ajuda no mercado de jogos?

O IndieForge busca incentivar o crescimento da indústria nacional de jogos independentes, oferecendo uma alternativa para desenvolvedores solo e pequenos estúdios que possuem boas ideias, mas enfrentam dificuldades para obter financiamento.
A plataforma funciona como uma ponte entre criadores e jogadores, permitindo que projetos promissores encontrem apoio antes do lançamento oficial, reduzindo o risco de abandono por falta de recursos e contribuindo para o fortalecimento do mercado independente.

![DER do projeto](image.png)

> ## Aviso
>
> Este projeto possui finalidade exclusivamente educacional e de portfólio.
> Todas as contribuições financeiras são **simuladas** e nenhuma transação monetária real é realizada.

## ✨ Funcionalidades

### 👤 Usuário

- Pesquisar projetos publicados.
- Visualizar informações detalhadas de um projeto.
- Contribuir financeiramente para projetos.
- Consultar seu histórico de contribuições feitas por ele mesmo.

### 🎮 Criador

- Publicar novos projetos.
- Editar informações dos próprios projetos.
- Cancelar projetos publicados.
- Acompanhar o progresso da arrecadação.
- Visualizar a lista de apoiadores.

### 🛡️ Administrador

- Visualizar estatísticas gerais da plataforma.
- Bloquear/banir usuários.
- Ocultar projetos da plataforma.
- Consultar o histórico de projetos e contribuições para fins de auditoria.

## Roadmap

> ### Legendas
>
> 🟩 - Concluído |
> 🟨 - Em andamento |
> 🟥 - Não concluído |
> 🚀 - Em breve

### Back-end

<details>
<summary>Ver andamento do Back-end</summary>

### Infraestrutura

- 🟩 Configuração do Entity Framework Core
- 🟩 Modelagem das entidades
- 🟩 Configuração do Context
- 🟩 Autenticação JWT
- 🟥 Configuração do CORS
- 🚀 Tratamento global de exceções
- 🚀 Docker
- 🚀 Deploy na Azure

### Arquitetura

- 🟨 Implementação dos Services
- 🟨 Criação dos DTOs

### Projetos

- 🟩 Listar projetos
- 🟨 Buscar projeto por Id
- 🟨 Criar projeto
- 🟥 Editar projeto
- 🟥 Cancelar projeto

### Contribuições

- 🟨 Contribuir para um projeto
- 🟥 Histórico de contribuições
- 🟥 Lista de apoiadores

### Administração

- 🟥 Auditoria universal de contribuições
- 🟥 Média de transações por minuto (valor e quantos por vez)
- 🟥 Ocultar projeto
- 🟥 Bloquear usuário
- 🚀 Revogar tokens de acesso
- 🚀 Refresh tokens

</details>

---

<details>
<summary>Ver andamento do Front-end</summary>

- Preenchimento em breve...
</details>

# Tecnologias usadas
<details>
<summary><strong>Back-end</strong></summary>

- C#
- ASP.NET Core 8
- Minimal APIs
- Entity Framework Core
- JWT
- PasswordHasher
- SQLite

</details>
<details>

<summary><strong>Front-end</strong></summary>

- Em breve...

</details>
<details>

<summary><strong>Outros</strong></summary>

- Swagger/OpenAPI
- Git
- Github
- Visual Studio

</details>

## Estrutura do projeto

<details>
<summary>Estruturação do projeto</summary>

```bash
📦IndieForge
 ┣ 📂Context
 ┃ ┗ 📜AppDbContext.cs
 ┣ 📂DTOs
 ┣ 📂Migrations
 ┣ 📂Models
 ┃ ┣ 📜Contribuicao.cs
 ┃ ┣ 📜Projeto.cs
 ┃ ┗ 📜User.cs
 ┣ 📂Services
 ┃ ┣ 📜AuthService.cs
 ┃ ┗ 📜ProjectService.cs
 ┣ 📜IndieForge.slnx
 ┗ 📜Program.cs
 ```

</details>

## Como testar o projeto

- Primeiro de tudo, use `git clone <URL do projeto>` em um diretório desejável e certifique-se que tenha o **.NET 8** instalado em sua máquina.
- Depois, navegue é a pasta raiz do projeto
- Digite `dotnet run` no terminal que aponta para a raiz do projeto e aguarde a aplicação inicializar.
- Após isso, poderá navegar até `http://localhost:5259/swagger/index.html` e testar a API através do Swagger.

### Registros Seeds

- Em breve...
