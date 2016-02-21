CREATE TABLE [dbo].[Receipt]
(
	[ReceiptId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
    [Description] VARCHAR(MAX) NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    [CreatedDate] DATETIME NOT NULL DEFAULT getdate()
)

GO

CREATE INDEX [IX_Receipt_Name] ON [dbo].[Receipt] ([Name])
