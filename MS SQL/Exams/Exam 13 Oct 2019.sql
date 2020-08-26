
USE Bitbucket
CREATE DATABASE Bitbucket

CREATE TABLE Users(
	Id INT PRIMARY KEY IDENTITY,
	Username VARCHAR(30) NOT NULL,
	[Password] VARCHAR(30) NOT NULL,
	Email VARCHAR(50) NOT NULL
)

CREATE TABLE Repositories(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL
)

CREATE TABLE RepositoriesContributors(
	RepositoryId INT NOT NULL FOREIGN KEY (RepositoryId) REFERENCES Repositories(Id),
	ContributorId INT NOT NULL  FOREIGN KEY (ContributorId) REFERENCES  Users(Id),
	PRIMARY KEY(RepositoryId, ContributorId)
)

CREATE TABLE Issues(
	Id INT PRIMARY KEY IDENTITY,
	Title VARCHAR(255) NOT NULL,
	IssueStatus NCHAR(6) NOT NULL,
	RepositoryId INT NOT NULL FOREIGN KEY REFERENCES Repositories(Id),
	AssigneeId INT NOT NULL FOREIGN KEY REFERENCES Users(Id)
)

CREATE TABLE Commits(
	Id INT PRIMARY KEY IDENTITY,
	[Message] VARCHAR(255) NOT NULL,
	IssueId INT FOREIGN KEY REFERENCES Issues(Id),
    RepositoryId INT  NOT NULL FOREIGN KEY REFERENCES Repositories(Id),
	ContributorId INT NOT NULL FOREIGN KEY REFERENCES Users(Id)
)

CREATE TABLE Files(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(100) NOT NULL,
	Size DECIMAL(15,2) NOT NULL,
	ParentId INT FOREIGN KEY REFERENCES Files(Id),
	CommitId INT NOT NULL FOREIGN KEY REFERENCES Commits(Id)
)

INSERT INTO Files([Name],Size,ParentId,CommitId)
VALUES
('Trade.idk',2598.0,1,1),
('menu.net',9238.31,2,2),
('Administrate.soshy',1246.93,3,3),
('Controller.php',7353.15,4,4),
('Find.java',9957.86,5,5),
('Controller.json',14034.87,3,6),
('Operate.xix',7662.92,7,7)

INSERT INTO Issues(Title,IssueStatus,RepositoryId,AssigneeId)
VALUES
('Critical Problem with HomeController.cs file'	,'open',1,4),	
('Typo fix in Judge.html','open',4,3),
('Implement documentation for UsersService.cs'	,'closed',8,2),
('Unreachable code in Index.cs'	,'open',9,8)


UPDATE Issues
SET IssueStatus='closed'
WHERE AssigneeId=6;



DELETE FROM Issues
WHERE RepositoryId = ( SELECT Id FROM Repositories WHERE [Name]='Softuni-Teamwork')

DELETE FROM RepositoriesContributors
WHERE RepositoryId= ( SELECT Id FROM Repositories WHERE [Name]='Softuni-Teamwork')


-- 5. Commits

Select Id, [Message],RepositoryId,ContributorId FROM Commits
ORDER BY Id, [Message],RepositoryId,ContributorId

--6.HeavyHtml
SELECT Id,[Name],Size FROM Files
WHERE Size >1000 AND [Name] LIKE '%html%'
ORDER BY Size DESC,Id,[Name]

--7.Issues and Users

SELECT i.Id,
	  CONCAT(u.Username, ' : ', i.Title) as [IssueAssignee]
FROM Issues i
INNER JOIN Users u
ON i.AssigneeId=u.Id

ORDER BY i.Id DESC,IssueAssignee 

--8.Non-Directory Files

SELECT f.Id,f.[Name], CONCAT(f.Size,'KB') as [Size] FROM Files f
LEFT JOIN Files fChild
ON f.Id=fChild.ParentId
WHERE fChild.Id IS NULL
ORDER BY f.Id,fChild.[Name],f.Size DESc

--09.Most Contributed Repositories
SELECT r.Id, r.[Name], COUNT() FROM RepositoriesContributors rc
LEFT OUTER JOIN Repositories r
ON rc.RepositoryId=r.Id
GROUP BY rc.RepositoryId,rc.ContributorId


SELECT TOP 5 r.Id,r.[Name] as [Name],
	COUNT(c.Id) as [Commits] 
	FROM RepositoriesContributors rc
LEFT OUTER JOIN Repositories r
ON rc.RepositoryId=r.Id
LEFT OUTER JOIN Commits c
ON c.RepositoryId=rc.RepositoryId
GROUP BY r.Id,r.[Name]
ORDER BY [Commits] DESC, r.Id, r.[Name]
