CREATE TABLE [dbo].[OrderedSeat]
(
    [CartId] INT NOT NULL, 
    [SeatId] INT NOT NULL PRIMARY KEY, 
    CONSTRAINT [FK_OrderedSeat_Cart] FOREIGN KEY ([CartId]) REFERENCES [Cart]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_OrderedSeat_EventSeat] FOREIGN KEY ([SeatId]) REFERENCES [EventSeat]([Id])
)
