# Сервис для учета стажеров
Тестовое задание на практику в 66БИТ.

Сервис состоит из 3 контейнеров: WebAPI (ASP.NET), Front-end (Blazor) и PostgreSQL 16.

WebAPI построено на многослойной onion архитектуре, где слой = файл .csproj:
- InternRegister - исполняемый проект, само API;
- Infrastructure - слой взаимодействия с БД;
- Application - слой бизнес логики, которой почти нет в данном проекте;
- Domain - доменная область, содержащая только используемые ключевые сущности;
- DtoModels - отдельный проект для dto моделей, чтобы на него можно было ссылаться из проекта с Blazor.

Запуск стандартный:
```bash
docker-compose up --build
```
### WebAPI (ASP.NET)
- Стандартный http порт: `8080`
- Подключен Swagger (независимо dev/prod): `localhost:8080/swagger`
### Front-end (Blazor Server)
- Стандартный http порт: `7200`
- Для создания интерфейса использован Bootstrap и select2 с jQuery для создания выпадающих списков.
### PostgreSQL
- Доступен извне на порте: `5434` 
- Пользователь `postgres:postgres`
- База данных: `internsRegister`
- Для тестирования был создан sql скрипт, добавляющий через INSERT тестовые данные в базу данных: `database-seeder.sql`
#### Из библиотек использованы:
- **Entity Framework** - для ORM взаимодействия с БД;
- **Newtonsoft Json** - для сериализации объектов и десериализации JSON;
- **Polly** - для обработки временных отказов соединения front-end'а с back-end'ом;
- **Swashbuckle** - для Swagger;
- **Serilog** - для логирования.
#### Примечание
С точки зрения UI/UX сервис может быть не самым совершенным, так как больше я специализируюсь на back-end'е, однако постарался реализовать наиболее приятный и удобный интерфейс.
Некоторые компоненты на front-end'е были взяты из моих других проектов, но были в значительной степени переписаны под этот проект.

