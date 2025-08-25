# Challenge Backend Sr

Este proyecto implementa un **Web API en .NET 8** para gestionar permisos de usuarios (**Permissions**) con persistencia en **SQL Server**, indexación en **Elasticsearch** y publicación de mensajes en **Kafka**.  


## Tecnologías utilizadas
- .NET 8 (Web API, EF Core)
- SQL Server 2022 (Docker)
- Elasticsearch 8.x (Docker)
- Apache Kafka (Docker + Zookeeper)
- Serilog (logs estructurados)
- Docker Compose


## Estructura del proyecto
ApiChallenge/ -> Proyecto Web API (controllers, startup)
Application/ -> Lógica de negocio, DTOs, interfaces
Domain/ -> Entidades del dominio
Infraestructure/ -> Repositorios, DbContext, migraciones, servicios (Elastic, Kafka)
## Levantar infraestructura con Docker(Elastic, Kafka,Sql Server)
docker-compose up -d
## Base de datos
dotnet ef migrations add InitDb --project .\Infraestructure\Infraestructure.csproj --startup-project .\ApiChallenge\ApiChallenge.csproj
dotnet ef database update --project .\Infraestructure\Infraestructure.csproj --startup-project .\ApiChallenge\ApiChallenge.csproj
