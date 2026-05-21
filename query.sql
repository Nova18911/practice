USE LibraryDB;
GO

-- Выборка данных с фильтрацией, сортировкой
SELECT Title, ISBN, Price, PublishedAt
FROM books
WHERE Price > 400 AND InStock = 1
ORDER BY Price DESC;

-- Изменение данных
UPDATE books 
SET Price = Price * 1.1;

-- Удаление данных
DELETE FROM Readers
WHERE Id NOT IN (SELECT DISTINCT ReaderId FROM Loans);


-- Выборка с группировкой
SELECT r.FullName, COUNT(l.Id) AS LoansCount
FROM readers r
JOIN Loans l ON l.ReaderId = r.Id
GROUP BY r.FullName
ORDER BY LoansCount DESC;


--Выборка из нескольких связанных таблиц
--левое
SELECT b.Title, l.LoanedAt, l.IsReturned
FROM books b
LEFT JOIN Loans l ON l.BookId = b.Id;

-- правое
SELECT r.FullName, l.LoanedAt
FROM Loans l
RIGHT JOIN Readers r ON l.ReaderId = r.Id;

-- пересечение
SELECT r.FullName
FROM readers r 
JOIN Loans l ON l.ReaderId = r.Id
WHERE l.IsReturned = 1

INTERSECT

SELECT r.FullName
FROM readers r 
JOIN Loans l ON l.ReaderId = r.Id
WHERE l.IsReturned = 0;