CREATE TABLE [dbo].[RecipeTagMapping]
(
	[MappingId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RecipeId] INT NOT NULL, 
    [TagId] INT NOT NULL, 
    CONSTRAINT [FK_RecipeTagMapping_Recipe] FOREIGN KEY ([RecipeId]) REFERENCES [Recipe]([RecipeId]), 
    CONSTRAINT [FK_RecipeTagMapping_Tag] FOREIGN KEY ([TagId]) REFERENCES [Tag]([TagId])
)
