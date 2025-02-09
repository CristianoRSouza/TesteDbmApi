                                                                                                                                                                            
# No caso para rodar a api, é necessário somente ter uma instancia do banco SqlServer no computador, o banco será criado dentro da instância que existir e as migrações serão inicializadas, 
quando o usuario iniciar o a API.
                                                                                                                                                                           
# Criando e Publicando minha Imagem Docker para uma Api ASP.NET Core

## 1. Criando a Imagem Docker

### 1.1. Criar um Dockerfile
Crie um arquivo chamado **Dockerfile** na raiz do seu projeto e adicione o seguinte conteúdo:

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080

CMD ["dotnet", "MinhaApi.dll"]

Execute o seguinte comando no terminal para criar a imagem Docker:
```sh
docker build -t <seu-usuario>/aspnetcore-api:latest .
```
Exemplo:
```sh
docker build -t yarusski/aspnetcore-api:latest .
```
Isso criará a imagem da sua API ASP.NET Core.

## 2. Fazer Push da Imagem para o Docker Hub

### 2.1. Fazer Login no Docker Hub
Se ainda não estiver autenticado, execute:
```sh
docker login
```
Informe seu nome de usuário e senha do Docker Hub.

### 2.2. Enviar a Imagem para o Docker Hub

Execute o comando abaixo para enviar a imagem:
```sh
docker push yarusski/aspnetcore-api:latest
```
Exemplo:
```sh
docker push yarusski/aspnetcore-api:latest
```
Agora, sua imagem está disponível no Docker Hub e pode ser utilizada em qualquer ambiente compatível com Docker.

## 3. Rodando a Aplicação com Docker

Qualquer pessoa pode executar a API localmente usando:

```sh
docker run -d -p 8080:8080 yarusski/aspnetcore-api:latest
```

Isso iniciará um contêiner com sua API ASP.NET Core.

### Parando o Contêiner
Para parar a API, identifique o ID do contêiner e pare-o:

```sh
docker ps  
docker stop <container-id>
```

## Conclusão 🚀

