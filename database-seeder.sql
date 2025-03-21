--
-- PostgreSQL database dump
--

-- Dumped from database version 16.1
-- Dumped by pg_dump version 16.1

-- Started on 2025-03-21 14:41:38

--
-- TOC entry 4795 (class 0 OID 100594)
-- Dependencies: 217
-- Data for Name: probation_projects; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.probation_projects VALUES ('90b9d339-2e5f-4a5e-bbfe-3db6c9d9297f', 'Оптимизация базы данных', '2025-05-12', 'Рефакторинг и оптимизация структуры базы данных для повышения производительности.');
INSERT INTO public.probation_projects VALUES ('57e3996b-755b-46fe-b843-48c86bdc0412', 'Внедрение CI/CD', '2025-05-15', 'Настройка процессов непрерывной интеграции и доставки в разработке ПО.');
INSERT INTO public.probation_projects VALUES ('e8e11f41-ce8b-4593-8035-58ae7caf5dad', 'Интеграция платежных систем', '2025-05-20', 'Добавление и настройка платежных шлюзов для приема оплат в системе.');
INSERT INTO public.probation_projects VALUES ('7aa29191-1933-448e-97d2-9ee1b713ff5f', 'Создание микросервиса для личного кабинета', '2025-05-08', 'Микросервис должен содержать логику: ...');
INSERT INTO public.probation_projects VALUES ('e99cb5b2-3d24-44f4-9633-e1bb5d2e71ce', 'Создание системы мониторинга серверов', '2025-05-16', 'Разработка веб-интерфейса для контроля и диагностики работы серверов.');
INSERT INTO public.probation_projects VALUES ('122206a5-7552-4d31-85f5-654d9fbec93d', 'Создание системы аналитики', '2025-05-10', 'Разработка аналитического инструмента для сбора и обработки данных пользователей.');
INSERT INTO public.probation_projects VALUES ('6ca9b28a-993d-4a34-b925-99b17338a4b6', 'Разработка чат-бота поддержки', '2025-05-13', 'Создание интеллектуального чат-бота для автоматизированного ответа на вопросы пользователей.');
INSERT INTO public.probation_projects VALUES ('530d72b8-0877-415f-ac23-778cb5ca1615', 'Разработка системы хранения логов', '2025-05-18', 'Создание распределенной системы сбора и анализа логов.');
INSERT INTO public.probation_projects VALUES ('d8324339-c702-4608-af46-8f943e183308', 'Разработка системы учета клиентов', '2025-05-15', 'Создание CRM-системы для эффективного управления клиентской базой.');
INSERT INTO public.probation_projects VALUES ('85af6b14-e05d-46aa-8e83-76fe2c9a8b5e', 'Разработка веб-портала для сотрудников', '2025-05-20', 'Создание внутреннего портала для управления рабочими процессами.');
INSERT INTO public.probation_projects VALUES ('8d227519-b332-420b-a50f-9cc51fadab4a', 'Разработка мобильного приложения для клиентов', '2025-05-11', 'Создание кроссплатформенного мобильного приложения для пользователей сервиса.');
INSERT INTO public.probation_projects VALUES ('904c8b4e-e298-4a17-94c2-628a1598ae7c', 'Миграция на облачные технологии', '2025-05-21', 'Перенос инфраструктуры компании в облачное хранилище для повышения масштабируемости.');
INSERT INTO public.probation_projects VALUES ('bce2e1a9-5c11-4f96-96f7-6cf0e8c4d799', 'Разработка API для внешних партнеров', '2025-05-22', 'Создание REST API для интеграции с партнёрскими сервисами.');
INSERT INTO public.probation_projects VALUES ('c4e547bd-3837-43d9-90ca-7373efbf6b77', 'Автоматизация тестирования ПО', '2025-05-17', 'Разработка набора автоматических тестов для повышения качества продукта.');
INSERT INTO public.probation_projects VALUES ('925fa5fc-13d7-4569-ba7e-032a598ec0d7', 'Создание системы рекомендаций', '2025-05-19', 'Разработка алгоритма рекомендаций на основе машинного обучения.');
INSERT INTO public.probation_projects VALUES ('85222362-2d3c-4dc7-b62f-04b60368bbd8', 'Разработка микросервиса для личного кабинета', '2025-05-08', 'Микросервис должен выполнять функции: авторизация, управление пользователями, обработка платежей.');

--
-- TOC entry 4794 (class 0 OID 100587)
-- Dependencies: 216
-- Data for Name: probation_courses; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.probation_courses VALUES ('dfe647ae-ccc0-4bde-8a7f-18053c258364', 'C# разработчик', 'Обязанности:
Работа с существующим кодом
Разработка новых продуктов под руководством опытных разработчиков');
INSERT INTO public.probation_courses VALUES ('169993ab-2c2a-4cbb-8bb3-f1f31921f4f6', 'Аналитик', 'Обязанности:
- Разработка документации по проектам
- Участие в подготовке коммерческих предложений
- Участие в проектировании интерфейсов
- Выяснение потребностей представителей заказчика
- Участие в тестировании ПО
- Участие во внедрении продуктов и обучении пользователей');
INSERT INTO public.probation_courses VALUES ('16f5eb6d-bda5-446d-86dd-3633cdd95d1a', 'PHP разработчик', 'Обязанности:
- Разрабатывать backend-часть сайтов / сервисов
- Разрабатывать CRUD админку на Yii2
- Разрабатывать RESTful API на Yii2');
INSERT INTO public.probation_courses VALUES ('1aa18791-9e2c-4e45-8a0e-8cd2ab396219', 'Frontend-разработчик', 'Обязанности:
Разработка пользовательского интерфейса
Работа с HTML, CSS, JavaScript, React/Vue');
INSERT INTO public.probation_courses VALUES ('d77c24bb-3cb8-44e8-944f-e02c5ad8346b', 'Python-разработчик', 'Обязанности:
Разработка серверных решений
Работа с Django, Flask, SQLAlchemy');
INSERT INTO public.probation_courses VALUES ('9897c5b3-890d-4782-8c1e-242620c83d57', 'Тестировщик ПО', 'Обязанности:
Проведение тестирования программного обеспечения
Разработка тест-кейсов и баг-репортов');
INSERT INTO public.probation_courses VALUES ('7ecf99de-4d26-4bc2-8750-fe8ab4afd664', 'Data Scientist', 'Обязанности:
Анализ и обработка данных
Разработка моделей машинного обучения');
INSERT INTO public.probation_courses VALUES ('cff7b5d0-d21e-4dcf-93d9-d34d07a1b102', 'UI/UX дизайнер', 'Обязанности:
Проектирование интерфейсов
Создание прототипов и макетов');
INSERT INTO public.probation_courses VALUES ('a43be1a9-1675-4e45-8fcf-d7c1c56b7bef', 'DevOps инженер', 'Обязанности:
Настройка CI/CD
Мониторинг и поддержка инфраструктуры');
INSERT INTO public.probation_courses VALUES ('f6b7f98d-0c58-4a51-bb46-b304f3c804c0', 'Бизнес-аналитик', 'Обязанности:
Сбор и анализ требований
Взаимодействие с заказчиками и командой разработки');
INSERT INTO public.probation_courses VALUES ('4d69cf19-9d36-4fa4-a32c-6a90ad70c562', 'Маркетолог', 'Обязанности:
Анализ рынка
Разработка рекламных кампаний');
INSERT INTO public.probation_courses VALUES ('bc35a48d-fde6-4753-8c50-2ca25af3b078', 'SMM-специалист', 'Обязанности:
Создание контента
Ведение социальных сетей');
INSERT INTO public.probation_courses VALUES ('79c2bf5e-77fc-495b-987c-ff96550b011b', 'Копирайтер', 'Обязанности:
Написание текстов
Разработка рекламных и PR-материалов');
INSERT INTO public.probation_courses VALUES ('cfb183a5-c737-4366-9387-05bd3768dd16', 'HR-специалист', 'Обязанности:
Поиск и отбор кандидатов
Проведение собеседований');
INSERT INTO public.probation_courses VALUES ('68fd3e03-81d8-4d49-8b62-2d5176779edb', 'Project Manager', 'Обязанности:
Управление проектами
Координация работы команды');

--
-- TOC entry 4796 (class 0 OID 100601)
-- Dependencies: 218
-- Data for Name: interns; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.interns VALUES ('99c49dc1-70a1-49dc-a8a1-02b6ad718ad9', 'Максим', 'Трофимов', true, 'maxim.trofimov@example.com', '+79010987654', '2001-04-19', '9897c5b3-890d-4782-8c1e-242620c83d57', '85af6b14-e05d-46aa-8e83-76fe2c9a8b5e');
INSERT INTO public.interns VALUES ('9a912793-91cc-46fe-b025-b6e655c7c8c6', 'Наталья', 'Громова', false, 'natalia.gromova@example.com', '+79087654321', '2002-09-15', 'cff7b5d0-d21e-4dcf-93d9-d34d07a1b102', '8d227519-b332-420b-a50f-9cc51fadab4a');
INSERT INTO public.interns VALUES ('75b99dee-584e-44a4-9c24-6aff9e819cb2', 'Андрей', 'Андреев', false, 'example@mail.ru', NULL, '2025-03-19', 'a43be1a9-1675-4e45-8fcf-d7c1c56b7bef', '8d227519-b332-420b-a50f-9cc51fadab4a');
INSERT INTO public.interns VALUES ('d56e1f50-3339-4610-9eb6-9a27836822bc', 'Игорь', 'Лебедев', true, 'igor.lebedev@example.com', '+79076543210', '2001-02-15', 'f6b7f98d-0c58-4a51-bb46-b304f3c804c0', '904c8b4e-e298-4a17-94c2-628a1598ae7c');
INSERT INTO public.interns VALUES ('2a709c8d-912d-4983-8299-615a95c7b172', 'Сергей', 'Мартынов', true, 'sergey.martynov@example.com', '+79054321098', '2001-08-03', 'f6b7f98d-0c58-4a51-bb46-b304f3c804c0', '904c8b4e-e298-4a17-94c2-628a1598ae7c');
INSERT INTO public.interns VALUES ('f70d9311-56af-4984-810a-d5979bb5090b', 'Алина', 'Родионова', false, 'alina.rodionova@example.com', '+79021098765', '2002-06-12', '4d69cf19-9d36-4fa4-a32c-6a90ad70c562', 'bce2e1a9-5c11-4f96-96f7-6cf0e8c4d799');
INSERT INTO public.interns VALUES ('096498b4-2331-45fc-b1ea-afa8282c835f', 'Григорий', 'Соловьев', true, 'grigory.soloviev@example.com', '+79032109876', '2001-01-02', '4d69cf19-9d36-4fa4-a32c-6a90ad70c562', 'c4e547bd-3837-43d9-90ca-7373efbf6b77');
INSERT INTO public.interns VALUES ('7f44f302-04c5-4a8a-abb6-79d0da3c459c', 'Алексей', 'Иванов', true, 'aleksei.ivanov@example.com', '+79001234567', '2000-03-14', '7ecf99de-4d26-4bc2-8750-fe8ab4afd664', '90b9d339-2e5f-4a5e-bbfe-3db6c9d9297f');
INSERT INTO public.interns VALUES ('613b4a33-10b3-4c29-aee7-a07bb9ccb71f', 'Светлана', 'Беляева', false, 'svetlana.belyaeva@example.com', '+79009876543', '2002-04-15', '9897c5b3-890d-4782-8c1e-242620c83d57', '85af6b14-e05d-46aa-8e83-76fe2c9a8b5e');
INSERT INTO public.interns VALUES ('77d303e3-7465-46de-b493-92945224df5f', 'Елизавета', 'Захарова', false, 'elizaveta.zakharova@example.com', '+79043210987', '2001-12-20', '79c2bf5e-77fc-495b-987c-ff96550b011b', '925fa5fc-13d7-4569-ba7e-032a598ec0d7');
INSERT INTO public.interns VALUES ('97647fd5-9688-4980-9c1d-63e9ca147c48', 'Дмитрий', 'Попов', true, 'popov@example.com', '+79333333333', '2004-03-19', '7ecf99de-4d26-4bc2-8750-fe8ab4afd664', '85222362-2d3c-4dc7-b62f-04b60368bbd8');
INSERT INTO public.interns VALUES ('5552b873-fedc-43ca-ac76-7572d1ceda03', 'Олег', 'Олегов', true, 'olegov@example.com', '+79111111111', '2004-03-19', '7ecf99de-4d26-4bc2-8750-fe8ab4afd664', '7aa29191-1933-448e-97d2-9ee1b713ff5f');
INSERT INTO public.interns VALUES ('6a93f2c7-b9b8-46ac-9869-9856b2147239', 'Ольга', 'Федорова', false, 'olga.fedorova@example.com', '+79056789012', '2000-02-13', 'd77c24bb-3cb8-44e8-944f-e02c5ad8346b', 'e99cb5b2-3d24-44f4-9633-e1bb5d2e71ce');
INSERT INTO public.interns VALUES ('c43c4ec7-c8a4-421e-9750-95bf9f0f2512', 'Анна', 'Смирнова', false, 'anna.smirnova@example.com', '+79034852793', '2002-05-09', 'd77c24bb-3cb8-44e8-944f-e02c5ad8346b', 'e99cb5b2-3d24-44f4-9633-e1bb5d2e71ce');
INSERT INTO public.interns VALUES ('80200bf9-eafa-4fd7-a1fc-4dd5878c9b19', 'Василий', 'Козлов', false, 'vasily.kozlov@example.com', '+79047287659', '1998-08-26', 'd77c24bb-3cb8-44e8-944f-e02c5ad8346b', 'e99cb5b2-3d24-44f4-9633-e1bb5d2e71ce');
INSERT INTO public.interns VALUES ('f845881f-859c-4c50-9e1a-dafa8a3d86df', 'Евгений', 'Морозов', true, 'evgeny.morozov@example.com', '+79069569428', '1999-06-29', 'd77c24bb-3cb8-44e8-944f-e02c5ad8346b', '122206a5-7552-4d31-85f5-654d9fbec93d');
INSERT INTO public.interns VALUES ('85f2ddea-8519-49fc-a59c-0f0a7f576168', 'Васильева', 'Екатерина', false, 'ekaterina.vasilyeva@example.com', '+79078901234', '2001-11-18', 'd77c24bb-3cb8-44e8-944f-e02c5ad8346b', '122206a5-7552-4d31-85f5-654d9fbec93d');
INSERT INTO public.interns VALUES ('1d8417eb-4b40-444a-b385-3dbc23c143a6', 'Игорь', 'Смирнов', true, 'smirnov@mail.ru', NULL, '2005-05-06', 'a43be1a9-1675-4e45-8fcf-d7c1c56b7bef', '6ca9b28a-993d-4a34-b925-99b17338a4b6');
INSERT INTO public.interns VALUES ('c79a34a0-13cb-4f88-b726-79f79658bc99', 'Вера', 'Киселева', false, 'vera.kiseleva@example.com', '+79065432109', '2002-01-04', 'f6b7f98d-0c58-4a51-bb46-b304f3c804c0', '530d72b8-0877-415f-ac23-778cb5ca1615');
INSERT INTO public.interns VALUES ('6aa177d3-29e7-459b-a756-03fbac995427', 'Константин', 'Орлов', true, 'konstantin.orlov@example.com', '+79098765432', '2002-03-04', 'bc35a48d-fde6-4753-8c50-2ca25af3b078', '530d72b8-0877-415f-ac23-778cb5ca1615');
INSERT INTO public.interns VALUES ('f8a2e246-a2d8-4154-867a-53a850282748', 'Иван', 'Иванов', true, 'ivanov@example.com', '+79222222222', '2025-03-20', 'dfe647ae-ccc0-4bde-8a7f-18053c258364', '85222362-2d3c-4dc7-b62f-04b60368bbd8');
INSERT INTO public.interns VALUES ('e87f7d2b-0784-4298-b9ed-55012ab9f4a9', 'Мария', 'Петрова', false, 'maria.petrova@example.com', '+79012345677', '1999-07-21', 'dfe647ae-ccc0-4bde-8a7f-18053c258364', '85222362-2d3c-4dc7-b62f-04b60368bbd8');
INSERT INTO public.interns VALUES ('0b988bae-7782-4085-b1a4-89f1a19631f2', 'Дмитрий', 'Сидоров', true, 'dmitry.sidorov@example.com', '+79023458897', '2001-12-02', 'dfe647ae-ccc0-4bde-8a7f-18053c258364', '85222362-2d3c-4dc7-b62f-04b60368bbd8');

-- Completed on 2025-03-21 14:41:38

--
-- PostgreSQL database dump complete
--

