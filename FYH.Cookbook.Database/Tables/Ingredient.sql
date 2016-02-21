CREATE TABLE [dbo].[Ingredient]
(
	[IngredientId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
    [Description] VARCHAR(MAX) NULL
)
