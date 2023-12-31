USE [WaCollaborative]

-- USER

SELECT * FROM [dbo].[AspNetUsers]
SELECT * FROM [dbo].[AspNetRoles]

-- MASTER

SELECT * FROM [dbo].[Categories]
SELECT * FROM [dbo].[Cities]
SELECT * FROM [dbo].[Countries]
SELECT * FROM [dbo].[DemandTypes]
SELECT * FROM [dbo].[DistributionChannels]
SELECT * FROM [dbo].[EventTypes]
SELECT * FROM [dbo].[MeasurementUnits]
SELECT * FROM [dbo].[Segments]
SELECT * FROM [dbo].[States]
SELECT * FROM [dbo].[Status]
SELECT * FROM [dbo].[StatusType]

-- QUERYS

SELECT * FROM [dbo].[Countries] AS C INNER JOIN
[dbo].[States] AS S ON C.Id = S.CountryId INNER JOIN
[dbo].[Cities] AS Ci ON S.Id = Ci.StateId
WHERE C.Id = 37 AND S.Id = 619 AND Ci.Id = 20624


