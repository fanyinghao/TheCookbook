CREATE TABLE [dbo].[Recipe]
(
	[RecipeId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
    [Description] VARCHAR(MAX) NULL, 
    [Directions] VARCHAR(MAX) NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    [CreatedDate] DATETIME NOT NULL DEFAULT getdate()
)

GO

CREATE INDEX [IX_Recipe_Name] ON [dbo].[Recipe] ([Name])
