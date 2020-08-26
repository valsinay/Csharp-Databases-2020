USE SoftUni

SELECT TOP 5 EmployeeID, JobTitle,e.AddressID, AddressText FROM Employees AS e
INNER JOIN Addresses as a
ON a.AddressID=e.AddressID 
ORDER BY AddressID

SELECT TOP 50 FirstName, LastName,t.[Name], AddressText FROM Employees as e
INNER JOIN Addresses as a
ON a.AddressID=e.AddressID
INNER JOIN Towns as t
ON a.TownID=t.TownID
ORDER BY FirstName, LastName


SELECT EmployeeID, FirstName, LastName,d.[Name]  FROM Employees as e
INNER JOIN Departments as d
ON e.DepartmentID=d.DepartmentID
WHERE d.[Name] ='Sales'
ORDER BY EmployeeID

SELECT TOP 5 EmployeeID, FirstName, Salary,d.[Name] as[DepartmentName] FROM Employees as e
INNER JOIN Departments as d
ON e.DepartmentID=d.DepartmentID
WHERE Salary>=15000
ORDER BY e.DepartmentID

SELECT TOP 3 e.EmployeeID, FirstName FROM Employees as e
LEFT OUTER JOIN EmployeesProjects as p
ON e.EmployeeID=p.EmployeeID 
WHERE p.ProjectID IS NULL

SELECT FirstName,LastName,HireDate,d.[Name] as [DeptName] FROM Employees as e
INNER JOIN Departments as d
ON e.DepartmentID= d.DepartmentID
WHERE HireDate > '1.1.1999' AND
d.[Name] IN ('Sales','Finance')
ORDER BY HireDate

SELECT TOP 5 e.EmployeeId, e.FirstName, p.[Name] as [ProjectName] FROM Employees as e
INNER JOIN EmployeesProjects as ep
ON e.EmployeeID=ep.EmployeeID
INNER JOIN Projects as p
ON ep.ProjectID=p.ProjectID
WHERE p.StartDate > '08.13.2002' and p.EndDate IS NULL
ORDER BY EmployeeID

SELECT e.EmployeeID,FirstName,
CASE
WHEN DATEPART(YEAR,p.StartDate) >=2005 THEN NULL
ELSE p.[Name] 
END AS [ProjectName]
FROM Employees as e
INNER JOIN EmployeesProjects as ep
ON e.EmployeeID=ep.EmployeeID
INNER JOIN Projects as p
ON ep.ProjectID=p.ProjectID
WHERE e.EmployeeID =24

SELECT e1.EmployeeID,e1.FirstName, e2.EmployeeID as [ManagerID], e2.FirstName as [ManagerName]
FROM Employees as e1
JOIN Employees as e2
ON e1.ManagerID=e2.EmployeeID
WHERE e1.ManagerID IN (3,7)
ORDER BY e1.EmployeeID	

SELECT * FROM Employees

