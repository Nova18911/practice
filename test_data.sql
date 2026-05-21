USE LibraryDB;
GO

INSERT INTO genres (Id, Name) VALUES
(NEWID(), N'Фантастика'),
(NEWID(), N'Детектив'),
(NEWID(), N'Роман');

DECLARE @AuthorId1 UNIQUEIDENTIFIER = NEWID();
DECLARE @AuthorId2 UNIQUEIDENTIFIER = NEWID();

INSERT INTO authors (Id, FullName, BirthDate, Country, IsALive) VALUES
(@AuthorId1, N'Фёдор Достоевский', '18211111', N'Россия', 0),
(@AuthorId2, N'Стивен Кинг', '19470921', N'США', 1);

DECLARE @BookId1 UNIQUEIDENTIFIER = NEWID();
DECLARE @BookId2 UNIQUEIDENTIFIER = NEWID();

INSERT INTO books (Id, Title, ISBN, Price, PublishedAt, InStock) VALUES
(@BookId1, N'Преступление и наказание', '978-5-17-090000-1', 450.00, '18660101', 1),
(@BookId2, N'Оно', '978-5-17-090000-2', 699.99, '19860915', 1);

INSERT INTO bookAuthors VALUES (@BookId1, @AuthorId1);
INSERT INTO bookAuthors VALUES (@BookId2, @AuthorId2);

DECLARE @ReaderId1 UNIQUEIDENTIFIER = NEWID();
INSERT INTO readers (Id, FullName, Email) VALUES
(@ReaderId1, N'Иван Иванов', 'ivan@mail.ru');

INSERT INTO Loans (Id, BookId, ReaderId, LoanedAt) VALUES
(NEWID(), @BookId1, @ReaderId1, GETDATE());