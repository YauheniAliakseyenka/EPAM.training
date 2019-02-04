CREATE TABLE [dbo].[PurchasedSeat]
(
    [OrderId] INT NOT NULL, 
    [SeatId] INT NOT NULL PRIMARY KEY, 
    [Price] DECIMAL(18, 2) NOT NULL, 
    CONSTRAINT [FK_PurchasedSeat_Order] FOREIGN KEY ([OrderId]) REFERENCES [Order]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_PurchasedSeat_EventSeat] FOREIGN KEY (SeatId) REFERENCES [EventSeat]([Id])
)
