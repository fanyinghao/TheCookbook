CREATE TABLE [dbo].[Tag]
(
	[TagId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(MAX) NOT NULL, 
    [Description] VARCHAR(MAX) NULL
)
