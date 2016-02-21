CREATE TABLE [dbo].[RecipeIngredientMapping]
(
	[MappingId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RecipeId] INT NOT NULL, 
    [IngredientId] INT NOT NULL, 
    [Unit] VARCHAR(50) NOT NULL, 
    [Quantity] DECIMAL(18, 3) NOT NULL,
    CONSTRAINT [FK_RecipeIngredientMapping_Recipe] FOREIGN KEY ([RecipeId]) REFERENCES [Recipe]([RecipeId]), 
    CONSTRAINT [FK_RecipeIngredientMapping_Ingredient] FOREIGN KEY ([IngredientId]) REFERENCES [Ingredient]([IngredientId])
)
