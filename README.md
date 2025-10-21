## 👤 Integrantes do Projeto
| Nome do Integrante       | RM       |
|--------------------------|----------|
| Alice Nunes              | 559052   |
| Guilherme Akira          | 556128   |
| Anne Rezendes            | 556779   |

---
# 🔒 SafeScribe API: Sistema de Notas Seguro com JWT

Este projeto é uma API RESTful simples para gerenciamento de notas, focada na implementação robusta de **Segurança baseada em JWT (JSON Web Token)** e Autorização por Papéis (Roles).

O desafio final implementa um sistema de **Blacklist de Tokens** para criar uma funcionalidade de "Logout" seguro em uma arquitetura *stateless*.

---

## 🚀 Tecnologias

* **Linguagem:** C# (.NET Core/ASP.NET Core)
* **Autenticação:** JWT Bearer (Implementação customizada com Claims e Roles)
* **Segurança:** BCrypt.Net (Hashing de Senhas)
* **Documentação:** Swagger/OpenAPI
* **Armazenamento:** Simulação em memória (Listas estáticas)


---

## 🔑 Instruções de Teste (Autenticação e Blacklist)



### 1. Usuário Padrão

O sistema já possui um usuário de teste inicial:
| Usuário | Senha | Role |
| :---: | :---: | :---: |
| **admin** | **123456** | **Admin** |

### 2. Fluxo de Teste de Logout

Execute os seguintes passos no Swagger/Postman:

#### A. Login (Obter Token)
* **Endpoint:** `POST /api/v1/auth/login`
* **Ação:** Use as credenciais do `admin` para obter o Token JWT.

#### B. Teste de Acesso (200 OK)
* **Autorização:** Clique em `Authorize` no Swagger e insira o token no formato `Bearer [SEU_TOKEN]`.
* **Endpoint:** `GET /api/v1/auth/dados-protegidos`
* **Resultado:** Deve retornar **200 OK** (Acesso concedido).

#### C. Logout (Revogar o Token)
* **Endpoint:** `POST /api/v1/auth/logout`
* **Ação:** Execute esta rota (ela usará o token que está na sessão autorizada).
* **Resultado:** Deve retornar **200 OK** (Token adicionado à Blacklist).

#### D. Teste de Revogação (401 Unauthorized)
* **Endpoint:** `GET /api/v1/auth/dados-protegidos`
* **Ação:** Tente executar o **mesmo GET** da Etapa B **sem alterar o token**.
* **Resultado Esperado:** Deve retornar **401 Unauthorized** com a mensagem "Token revogado (Logout realizado)...", provando que o `JwtBlacklistMiddleware` bloqueou o token, mesmo antes de expirar.
