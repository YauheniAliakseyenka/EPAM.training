﻿--- Venue
INSERT INTO dbo.Venue
VALUES ('Symphony Hall','Birmingham', '413 43 441 32 12', 'As public buildings, welcoming over 450,000 members of the public for performances each year at Town Hall and Symphony Hall', 'Mountain Standard Time'),
('Royal Albert Hall','London', '231 25 121 22 12', 'The Royal Albert Hall Box Office is open to everyone daily from 9am, with tours beginning at 9.30am and Verdi opening at 12 noon (Tuesday – Sunday)', 'Mountain Standard Time')
--- Layout
INSERT INTO dbo.Layout
VALUES (1, 'The Big hall, 1t floor'),
(1, 'The Small hall, 1t floor'),
(2, 'The Main Hall')

--- Area
INSERT INTO dbo.Area
VALUES (1, 'The area #1', 1, 1),
(1, 'The area #2', 1, 1),
(2, 'Main area', 4, 4),
(3, 'The first area', 7, 7),
(3, 'The second area', 7, 7)

--- Seat
INSERT INTO dbo.Seat
VALUES (1, 1, 1),
(1, 1, 2),
(1, 1, 3),
(1, 2, 1),
(1, 2, 2),
(1, 2, 3),

(2, 1, 1),
(2, 1, 2),
(2, 1, 3),
(2, 1, 4),
(2, 2, 1),
(2, 2, 2),

(3, 1, 1),
(3, 1, 2),
(3, 1, 3),
(3, 1, 4),
(3, 2, 1),
(3, 2, 2),
(3, 2, 3),

(4, 1, 1),
(4, 1, 2),
(4, 1, 3),
(4, 1, 4),
(4, 2, 1),
(4, 2, 2),
(4, 2, 3),

(5, 1, 1),
(5, 1, 2),
(5, 1, 3),
(5, 1, 4),
(5, 2, 1),
(5, 2, 2),
(5, 2, 3)


--- Role
INSERT INTO dbo.[Role]
VALUES ('User'),
('Event manager'),
('Venue manager')

--- event_manager (password: 1231231231)
INSERT INTO dbo.[User]
VALUES ('event_manager', 'AFm5ad5jz9VOpSkL6yUgSX8oVYZgzebM4oiK7s4jWKdADiGQv0l50xlZoAk1CG16hg==', 'manager@gmail.com', 'John', 'Smith', 'en', 'UTC', 250.25)

--- User roles for event manager
INSERT INTO dbo.[UserRole]
VALUES (1, 1),
(1, 2)

--- Event
EXEC AddEvent 'Parsifal', 'The highpoint of Andris Nelsons’ final season as the CBSO’s music director – a concert performance of Wagner’s final music drama of almost alarming maturity','http://localhost:61963/Content/images/default.jpg', 1, '12/8/2022 12:00', 1
UPDATE EventArea SET Price = '15.25'



