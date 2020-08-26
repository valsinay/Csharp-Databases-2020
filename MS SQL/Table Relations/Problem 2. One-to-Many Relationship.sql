CREATE TABLE Manufacturers(
	ManufacturerID INT PRIMARY KEY,
	[Name] NVARCHAR(20) NOT NULL,
	EstablishedOn DateTime2 NOT NULL
)

CREATE TABLE Models(
	ModellID INT PRIMARY KEY,
	[Name] NVARCHAR(20) NOT NULL,
	ManufacturerID INT NOT NULL FOREIGN KEY REFERENCES Manufacturers(ManufacturerID)
)

INSERT INTO Manufacturers(ManufacturerID,[Name],EstablishedOn)
	VALUES
	(1,'BMW','1916-03-07'),
	(2,'Tesla','2003-03-07'),
	(3,'Lada','1966-03-07')


INSERT INTO Models(ModellID, [Name],ManufacturerID)
	VALUES
(101, 'X1',1),
(102, 'X2',1),
(103, 'Model S',2),
(104, 'Model X',2),
(105, 'Model 3',2),
(106, 'Nova',3)