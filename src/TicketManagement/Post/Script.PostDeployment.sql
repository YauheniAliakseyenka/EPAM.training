--- Venue
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
(1, 'The area #2', 1, 2),
(2, 'Main area', 4, 4),
(3, 'The first area', 3, 3),
(3, 'The second area', 3, 4)

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
VALUES ('event_manager', '0bEbr5YEmzY0tEy9afFbmHTlBNFkiRGHVF8Odx0XtOjRHV8vr20A7PsJJruF4IZnzXidZ/AyTZjBca64eTCUKWR7NF/xDbPj9wWkVSC/npCDPuRIF02uWZM2hJj2WlYMmadwNaqlU1DziSftHwg19R2n52xYTPnW5P9c61yHJ7A=', 'manager@gmail.com', 'John', 'Smith', 'en', 'UTC', 250.25, '5EN5eJkBir0MoB2fDbWkOOBSAJRmpt7wOrQH72+6jL5pxqIIdyWgCTTsDHcDcXpeRG2uUDgPJD/0XNxicUdJ1A==')

--- User roles for event manager
INSERT INTO dbo.[UserRole]
VALUES (1, 1),
(1, 2)

--- venue_manager (password: 1231231231)
INSERT INTO dbo.[User]
VALUES ('venue_manager', '0bEbr5YEmzY0tEy9afFbmHTlBNFkiRGHVF8Odx0XtOjRHV8vr20A7PsJJruF4IZnzXidZ/AyTZjBca64eTCUKWR7NF/xDbPj9wWkVSC/npCDPuRIF02uWZM2hJj2WlYMmadwNaqlU1DziSftHwg19R2n52xYTPnW5P9c61yHJ7A=', 'venue_manager@gmail.com', 'Sam', 'Vinchester', 'en', 'UTC', 0.25, '5EN5eJkBir0MoB2fDbWkOOBSAJRmpt7wOrQH72+6jL5pxqIIdyWgCTTsDHcDcXpeRG2uUDgPJD/0XNxicUdJ1A==')


--- User roles for event manager
INSERT INTO dbo.[UserRole]
VALUES (2, 1),
(2, 3)

--- Event
EXEC AddEvent 'Parsifal', 'The highpoint of Andris Nelsons’ final season as the CBSO’s music director – a concert performance of Wagner’s final music drama of almost alarming maturity','http://localhost:61963/Content/images/default.jpg', 1, '12/8/2022 12:00', 1
EXEC AddEvent 'Parsifal', 'The highpoint of Andris Nelsons’ final season as the CBSO’s music director – a concert performance of Wagner’s final music drama of almost alarming maturity','http://localhost:61963/Content/images/default.jpg', 3, '12/8/2020 11:00', 1
UPDATE EventArea SET Price = '10.25'




