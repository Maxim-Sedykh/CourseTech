﻿INSERT INTO Questions(LessonId, Number, Type, DisplayQuestion, Notation, CorrectQueryAnswer, Discriminator) VALUES
(1,1,0,'Примеры команд DQL это','v_1'),
(1,2,0,'Хранимые процедуры - это','v_3'),
(1,3,0,'Как расшифровывается первая буква в языке T-SQL?','v_1'),
(1,4,1,'Интегрированная среда разработки для работы с СУБД MS SQL Server называется (напишите аббревиатуру):','ssms'),
(1,5,1,'CAST, CONVERT, PARSE это всё функции чего?','преобразование'),
(1,5,1,'CAST, CONVERT, PARSE это всё функции чего?','преобразования'),
(1,6,1,'Как называется (сокращённо) язык запросов, который используется в MS SQL Server?','t-sql'),
(1,6,1,'Как называется (сокращённо) язык запросов, который используется в MS SQL Server?','tsql'),
(2,1,0,'SSMS - это','v_3'),
(2,2,0,'Для создания хранимой процедуры применяется команда:','v_2'),
(2,3,0,'Во что будет оптимально инкапсулировать набор из нескольких действий?','v_3'),
(2,4,1,'Что является главным инструментом SSMS?','object explorer'),
(2,5,1,'Какая команда устанавливает текущую базу данных ','use'),
(2,6,1,'Назовите первое ключевое слово блока отделения тела процедуры от остальной части скрипта','begin'),
(2,7,2,'Вывести список названий всех фильмов, отсортированных по названию в обратном алфавитном порядке:',
'SELECT Name FROM Films ORDER BY Name DESC;'),
(2,8,2,'Вывести список всех билетов на определённый фильм (а именно столбцы Tickets.Id, Tickets.Cost, Screenings.FilmId)
где цена билета больше 160 рублей и FilmId в Screening = 3','SELECT Tickets.Id, Tickets.Cost, Screenings.FilmId 
FROM Tickets 
JOIN Screenings ON Tickets.ScreeningId = Screenings.Id 
WHERE Screenings.FilmId = 3 AND Tickets.Cost > 160 
ORDER BY Screenings.Time ASC; '),
(3,1,0,'Для фильтрации данных по критерию используется ключевое слово:','v_1'),
(3,2,0,'Какая команда позволяет вставить запись в таблицу','v_1'),
(3,3,0,'JOIN используется для:','v_2'),
(3,4,1,'Для выполнения выборки используется команда:','select'),
(3,5,1,'Какая используется команда для сортировки данных?','order by'),
(3,5,1,'Какая используется команда для сортировки данных?','order'),
(3,6,1,'Какое ключевое слово используется для удаления записей из таблицы','delete'),
(3,7,2,'Вывести список всех фильмов (а именно столбцы Films.Name, Screenings.Time), которые предшествуют дате 2021-01-08 на одну неделю: 
(Используйте функцию t-sql DATEADD)',
'SELECT Films.Name, Screenings.Time 
FROM Films 
JOIN Screenings ON Films.Id = Screenings.FilmId 
WHERE Screenings.Time < DATEADD(WEEK, -1, ''2021-01-08'')'),
(3,8,2,' Вывести список всех билетов (а именно столбцы Tickets.Id, Tickets.Cost)
где номер ряда содержит цифру 5 (Используйте функцию TSQL - CHARINDEX) сортируя Tickets.Id в обратном порядке','SELECT Tickets.Id, Tickets.Cost
FROM Tickets 
WHERE CHARINDEX(''5'', CAST(Tickets.COST AS VARCHAR)) > 0 
ORDER BY Tickets.Id DESC;'),
(4,1,0,'Какие типы транзакций есть в MS SQL Server?','v_3'),
(4,2,0,'Какими словами заканчиваются транзакции?','v_2'),
(4,3,0,'Можно ли делать параллельное выполнение транзакций?','v_1'),
(4,4,1,'Совокупность операций, которые переводят согласованное состояние базы данных в несогласованное - это','транзакция'),
(4,4,1,'Совокупность операций, которые переводят согласованное состояние базы данных в несогласованное - это','трансакция'),
(4,5,1,'Какие 2 ключевых слова начинают выполнение транзакции?','begin transaction'),
(4,6,1,'Оператор ___ завершает текущую транзакцию и делает все изменения, выполненные в транзакции, постоянными.','commit'),
(4,7,2,'Вывести список всех фильмов (а именно столбцы Films.Name, Screenings.Id, Screenings.Time), 
которые в Screenings.Time в дне равно 01 (Используйте функцию t-sql DATEADD): 
(Используйте функцию t-sql DAY)',
'SELECT Films.Name, Screenings.Id, Screenings.Time
FROM Films 
JOIN Screenings ON Films.Id = Screenings.FilmId 
WHERE DAY ( Screenings.Time ) = 01'),
(4,8,2,' Вывести список всех фильмов (а именно столбцы Name, Description)
где длина поля Description больше 150 символов (Используйте функцию TSQL - LEN)','SELECT Name, Description
FROM Films
WHERE LEN(Description) > 150'),
(5,1,0,'Как кратко называется средство администрирования для MS SQL Server?','v_2'),
(5,2,0,'Какая системная база данных используется в качестве шаблона для всех баз данных, которые будут создаваться в экземпляре SQL Server?','v_3'),
(5,3,0,'Какая из этих команд выполняет процедуру или скалярную функцию?','v_1'),
(5,4,0,'Какая из этих функций возвращает строку, возникающую в результате объединения двух или более строковых значений в сквозной форме?','v_1'),
(5,5,0,'Продолжите фразу: транзакция — это группа инструкций одной или нескольких баз данных, которые ...........','v_1'),
(5,6,0,'Как можно создать резервную копию базы данных? ','v_2'),
(5,7,1,'С помощью каких двух ключевых слов создается база данных?','create database'),
(5,8,1,'С помощью какого ключевого слова объявляется переменная?','declare'),
(5,9,1,'Оператор ___ завершает текущую транзакцию и делает все изменения, выполненные в транзакции, постоянными.','commit'),
(5,10,1,'Вставьте пропущенное слово: в операторе IF Необязательное ключевое слово ..... позволяет указать альтернативную инструкцию Transact-SQL,
выполняемую в случае, если значение выражения Boolean_expression равно FALSE или NULL.','else'),
(5,11,1,'С помощью каких двух ключевых слов создается триггер','create trigger'),
(5,12,1,'Какими двумя ключевыми словами можно создать хранимую процедуру, помимо CREATE PROCEDURE?','create proc'),
(5,13,2,'Вывести список фильма, который имеет название ''65'' и среднюю стоимость билета на него 
(Вывести следующие столбцы Films.Name, AverageCost), (Используйте GROUP BY и HAVING)',
'SELECT Films.Name, AVG(Tickets.Cost) AS AverageCost 
FROM Films 
JOIN Screenings ON Films.Id = Screenings.FilmId 
JOIN Tickets ON Screenings.Id = Tickets.ScreeningId 
GROUP BY Films.Name
HAVING Films.Name = ''65'';'),
(5,14,2,'Найти все сеансы, прошедшие раньше текущего времени. (Используйте функцию GETDATE() и выведите все столбцы в таблице с помощью *)',
'SELECT *
FROM Screenings
WHERE Time < GETDATE();')