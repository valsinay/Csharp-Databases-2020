USE SoftUni

	SELECT FirstName, LastName FROM Employees 
	WHERE JobTitle NOT LIKE '%engineer%'

	SELECT * FROM Towns
	WHERE [Name] NOT LIKE ('[RBD]%')
	ORDER BY [Name]

	CREATE VIEW V_EmployeesHiredAfter2000 AS
	SELECT FirstName,LastName FROM Employees
	WHERE Year(HireDate) >2000;

	SELECT * FROM V_EmployeesHiredAfter2000

	SELECT FirstName,LastName FROM Employees 
	WHERE LEN(LastName)=5;

	SELECT * FROM( SELECT
	EmployeeId,FirstName,LastName,Salary,
	DENSE_RANK() over (PARTITION BY Salary order by EmployeeId)  as [Rank]
	 FROM Employees
	 WHERE Salary BETWEEN 10000 and 50000) AS [RankTable]
	 WHERE [Rank]=2
	 ORDER BY SAlary desc
	 

	 USE Geography

	 SELECT CountryName, ISOCode FROM Countries
	 WHERE CountryName LIKE 'a%a%a'  or CountryName LIKE '%a%a%a' OR CountryName LIKE 'a%a%a%'
	 ORDER BY IsoCode

 Use SoftUni

 SELECT TOP 50 e.EmployeeID,
  CONCAT(e.FirstName, ' ',e.LastName) AS [EmployeeName],
  CONCAT(m.FirstName ,' ', m.LastName) AS [ManagerName],
  d.[Name] as [DepartmentName]
  FROM Employees as e
  LEFT OUTER JOIN Employees as m
  ON e.ManagerID=m.EmployeeID
  RIGHT OUTER JOIN Departments as d
  ON e.DepartmentID=d.DepartmentID
  ORDER BY EmployeeID

  SELECT MIN(AverageSalary) FROM 
    (
		SELECT DepartmentId, AVG(Salary) as [AverageSalary] FROM Employees
		GROUP BY DepartmentID
	) as [AverageSalaryQuery]

USE [Geography]


SELECT c.CountryCode,m.MountainRange,p.PeakName,p.Elevation FROM Countries as c
INNER JOIN MountainsCountries as mc
ON c.CountryCode=mc.CountryCode
INNER JOIN Mountains as m
ON mc.MountainId=m.Id
INNER JOIN Peaks as p
 ON mc.MountainId=p.MountainId
 WHERE mc.CountryCode='BG' AND p.Elevation >2835
 ORDER BY p.Elevation DESC


 SELECT * FROM Peaks
	SELECT * FROM Countries 
	
	SELECT * FROM Mountains

	SELECT CountryCode, COUNT(MountainID) as [MountainRanges] FROM MountainsCountries
	WHERE CountryCode IN ('US','BG','RU')
	GROUP BY CountryCode
	
	SELECT * From Rivers
	SeLECT * FROM CountriesRivers
	SELeCT * FROM Countries

	SELECT TOP 5 CountryName,RiverName FROM Countries as c
	LEFT OUTER JOIN CountriesRivers as cr
	ON c.CountryCode=cr.CountryCode
	LEFT OUTER JOIN Rivers as r
	ON cr.RiverId=r.ID
	WHERE ContinentCode='AF'
	ORDER BY CountryName

	SELECT * FROM Continents			
	SELECT * FROM Currencies
	SELECT * FROM Countries

	SELECT ContinentCode, CUrrencyCode, CurrencyUsage FROM
		(	SELECT ContinentCode, 
			CurrencyCode,
			[CurrencyUsage],
			DENSE_RANK() OVER (PARTITION BY ContinentCode ORDER BY CurrencyUsage DESC) as [CurrencyRank]
			FROM 
			(
	         SELECT con.ContinentCode, cs.CurrencyCode  , 
			 COUNT(cs.ContinentCode) as [CurrencyUsage]
	         FROM Continents con
	         JOIN Countries as cs
	         ON con.ContinentCode=cs.ContinentCode
	         GROUP BY CurrencyCode, con.ContinentCode	
	) 	as [CurrencyCountQuery]
	WHERE CurrencyUsage > 1
		) as [ CurrencyRankingQuery]
		WHERE CurrencyRank=1
		ORDER BY ContinentCode
		

SELECT COUNT(*) as [Count] FROM Countries as c
LEFT JOIN MountainsCountries as mc
ON c.CountryCode=mc.CountryCode
WHERE mc.CountryCode IS NULL

USE Geography

SELECT TOP (5) c.CountryName,
		MAX(p.Elevation) AS [HighestPeakElevation],
		MAX(r.Length) AS [LongestRiverLength]
FROM Countries  AS c
LEFT  JOIN CountriesRivers AS cr
ON c.CountryCode=cr.CountryCode
LEFT  JOIN Rivers AS r
ON cr.RiverId=r.Id 
LEFT  JOIN MountainsCountries AS mc
ON mc.CountryCode=c.CountryCode
LEFT  JOIN Mountains AS m
ON mc.MountainId=m.Id	
LEFT  JOIN Peaks AS p
ON p.MountainId = m.Id
GROUP BY c.CountryName
ORDER BY [HighestPeakElevation] DESC, [LongestRiverLength] DESC, CountryName


    SELECT  TOP 5 Country,
 ISNULL(PeakName,'(no highest peak)') as [HighestPeakName],
 ISNULL(Elevation,0) as [HighestPeakElevation],
 ISNULL(MountainRange,'(no mountain)') as [Mountain]
 FROM 
	 	(	
	 	  SELECT *,
	 	  DENSE_RANK() OVER (PARTITION BY [Country] ORDER BY [Elevation] DESC) AS [PeakRank]
	 	  FROM (
	 	SELECT CountryName as [Country],
	 	p.PeakName as [PeakName],
	 	p.Elevation as [Elevation] ,
	 	m.MountainRange as [MountainRange]
	 	FROM Countries as c
	 	LEFT JOIN MountainsCountries as mc
	 	ON c.CountryCode=mc.CountryCode
	 	LEFT JOIN Mountains as m
	 	ON mc.MountainId=m.Id
	 	LEFT JOIN Peaks as p
	 	ON m.Id=p.MountainId
	 	) AS [FullInfoQuery]
					

			) as [PeakRankQuery]
				WHERE [PeakRank]=1
				ORDER BY Country, [HighestPeakName]

USE Service

CREATE DATABASE Service 

CREATE TABLE Users(
	Id INT PRIMARY KEY IDENTITY,
	Username VARCHAR(30) NOT NULL UNIQUE,
	[Password] VARCHAR(50) NOT NULL,
	[Name] VARCHAR(50),
	Birthdate DATETIME2,
	Age INT CHECK(Age Between 14 AND 110),
	Email VARCHAR(50) NOT NULL
)

CREATE TABLE Departments(
	Id INT PRIMARY KEY IDENTITY,
	Name VARCHAR(50) NOT NULL
)

CREATE TABLE Employees(
	Id INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(25),
	LastName VARCHAR(25),
	Birthdate DATETIME2,
	Age INT CHECK(Age BETWEEN 18 and 110),
	DepartmentID INT FOREIGN KEY REFERENCES Departments(Id)
)

CREATE TABLE Categories(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	DepartmentId INT NOT NULL FOREIGN KEY REFERENCES Departments(Id)
)

CREATE TABLE [Status](
	Id INT PRIMARY KEY IDENTITY,
	[Label] VARCHAR(30) NOT NULL
)

CREATE TABLE Reports(
	Id INT PRIMARY KEY IDENTITY,
	CategoryId INT NOT NULL FOREIGN KEY REFERENCES Categories(Id),
	StatusId  INT NOT NULL FOREIGN KEY REFERENCES [Status](Id),
	OpenDate DATETIME2 NOT NULL,
	CloseDate DATETIME2,
	[Description] VARCHAR(200) NOT NULL,
	UserId INT NOT NULL FOREIGN KEY REFERENCES Users(Id),
	EmployeeId INT  FOREIGN KEY REFERENCES Employees(Id)
)

INSERT INTO Employees(FirstName,LastName,Birthdate,DepartmentID)
VALUES
('Marlo','O''Malley','1958-9-21',1),
('Niki','Stanaghan','1969-11-26',4),
('Ayrton','Senna','1960-03-21',9),
('Ronnie','Peterson','1944-02-14',9),
('Giovanna','Amati','1959-07-20',5)

INSERT INTO Reports(CategoryId,StatusId,OpenDate,CloseDate,[Description],UserId,EmployeeId)
VALUES
(1,1,'2017-04-13', NULL,'Stuck Road on Str.133',6,2),
(6,3,'2015-09-05','2015-12-06','Charity trail running',3,5),
(14,2,'2015-09-07', NULL,'Falling bricks on Str.58',5,2),
(4,3,'2017-07-03','2017-07-06','Cut off streetlight on Str.11',1,1)

UPDATE Reports
SET CloseDate= GETDATE()
WHERE CloseDate IS NULL

DELETE FROM Reports WHERE StatusId=4

SELECT * FROM Reports
USE Service

SELECT Description, Format(OpenDate,'dd-MM-yyyy') FROM Reports
WHERE EmployeeId IS NULL
ORDER BY OpenDate, [Description]

SELECT r.[Description],c.[Name] FROM Reports as r
INNER JOIN Categories as c
ON r.CategoryId=c.Id
ORDER BY r.[Description],c.[Name]



SELECT TOP 5 c.[Name] as [CategoryName], COUNT(*) as [ReportsNumber] FROM Reports as r
INNER JOIN Categories as c
ON r.CategoryId=c.Id
GROUP BY c.[Name]
ORDER BY [ReportsNumber] DESC, [CategoryName]


SELECT u.Username,c.[Name] as [CategoryName] FROM Reports as r
INNER JOIN Categories as c
ON r.CategoryId=c.Id
INNER JOIN Users as u
ON r.UserId = u.Id
WHERE  DATEPART(MONTH, u.Birthdate) = DATEPART(Month,r.OpenDate)
AND DATEPART(DAY, u.Birthdate) = DATEPART(DAY,r.OpenDate)
ORDER BY Username, [CategoryName]

SELECT * FROM Users
SELECT * FROM Reports
SELECT * FROM Employees

 SELECT e.FirstName as [FullName], COUNT(*) as [UsersCount] FROM Employees as e
INNER JOIN Reports as r
ON r.EmployeeId=e.Id 
GROUP BY r.Id
ORDER BY [UsersCount] DESC, [FullName]

USE Service

SELECT 
 ISNULL((e.FirstName +' ' + e.LastName) ,'None') as [Employee],
 ISNULL(d.[Name] , 'None') as [Department],
 c.[Name] as [Category],
 r.[Description] as [Description],
 FORMAT(r.OpenDate,'dd.MM.yyyy') as [OpenDate],
 s.[Label] as [Status],	
 u.[Name] as [User]
FROM Reports as r
LEFT OUTER JOIN Employees as e
ON r.EmployeeId=e.Id
LEFT JOIN Departments as d
ON e.DepartmentID=d.Id
LEFT JOIN Categories as c
ON r.CategoryId=c.Id
LEFT JOIN Status as s
ON r.StatusId=s.Id
LEFT JOIN Users as u
ON r.UserId=u.Id
ORDER BY e.FirstName DESC,e.LastName DESC,Department,Category,[Description],OpenDate,[Status], [User]