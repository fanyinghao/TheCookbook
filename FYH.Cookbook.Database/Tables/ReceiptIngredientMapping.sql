CREATE TABLE [dbo].[ReceiptIngredientMapping]
(
	[MappingId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ReceiptId] INT NOT NULL, 
    [IngredientId] INT NOT NULL, 
    CONSTRAINT [FK_ReceiptIngredientMapping_Receipt] FOREIGN KEY ([ReceiptId]) REFERENCES [Receipt]([ReceiptId]), 
    CONSTRAINT [FK_ReceiptIngredientMapping_Ingredient] FOREIGN KEY ([IngredientId]) REFERENCES [Ingredient]([IngredientId])
)
