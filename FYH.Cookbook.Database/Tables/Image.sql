CREATE TABLE [dbo].[Image]
(
	[ImageId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Url] VARCHAR(MAX) NOT NULL, 
    [Description] VARCHAR(MAX) NULL, 
    [RecipeId] INT NOT NULL, 
    [CreatedDate] DATETIME NOT NULL DEFAULT getdate(), 
    CONSTRAINT [FK_Image_Recipe] FOREIGN KEY ([RecipeId]) REFERENCES [Recipe]([RecipeId])
)
