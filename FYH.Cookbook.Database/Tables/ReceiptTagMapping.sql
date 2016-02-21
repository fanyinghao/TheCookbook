CREATE TABLE [dbo].[ReceiptTagMapping]
(
	[MappingId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ReceiptId] INT NOT NULL, 
    [TagId] INT NOT NULL, 
    CONSTRAINT [FK_ReceiptTagMapping_Receipt] FOREIGN KEY ([ReceiptId]) REFERENCES [Receipt]([ReceiptId]), 
    CONSTRAINT [FK_ReceiptTagMapping_Tag] FOREIGN KEY ([TagId]) REFERENCES [Tag]([TagId])
)
