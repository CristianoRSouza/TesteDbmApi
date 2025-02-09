# Etapa 1: Build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia os arquivos do projeto e restaura as dependências
COPY *.sln ./
COPY ApiDbmTeste/ ApiDbmTeste/
RUN dotnet nuget locals all --clear
RUN dotnet restore ApiDbmTeste/ApiDbmTeste.csproj --force

# Publica a aplicação
RUN dotnet publish ApiDbmTeste/ApiDbmTeste.csproj -c Release -o /out

# Etapa 2: Criando a imagem final com PostgreSQL
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copia a aplicação publicada
COPY --from=build /out .

# Instala PostgreSQL
RUN apt-get update && apt-get install -y postgresql postgresql-contrib && \
    rm -rf /var/lib/apt/lists/*

# Configura e inicia o PostgreSQL
RUN service postgresql start && \
    su - postgres -c "psql -c \"CREATE DATABASE BancoMentoriaGordo;\"" && \
    su - postgres -c "psql -c \"CREATE USER yarusski WITH PASSWORD 'masha3112';\"" && \
    su - postgres -c "psql -c \"GRANT ALL PRIVILEGES ON DATABASE BancoMentoriaGordo TO yarusski;\""

# Expõe as portas
EXPOSE 8080 5432

# Inicia o PostgreSQL e a API simultaneamente
CMD service postgresql start && dotnet ApiDbmTeste.dll
