--- Venue
INSERT INTO dbo.Venue
VALUES ('Symphony Hall','Birmingham', '413 43 441 32 12', 'As public buildings, welcoming over 450,000 members of the public for performances each year at Town Hall and Symphony Hall'),
('Royal Albert Hall','London', '231 25 121 22 12', 'The Royal Albert Hall Box Office is open to everyone daily from 9am, with tours beginning at 9.30am and Verdi opening at 12 noon (Tuesday – Sunday)')
--- Layout
INSERT INTo dbo.Layout
VALUES (1, 'The Big hall, 1t floor'),
(1, 'The Small hall, 1t floor'),
(2, 'The Main Hall')

--- Area
INSERT INTo dbo.Area
VALUES (1, 'The area #1', 1, 1),
(1, 'The area #2', 1, 1),
(2, 'Main area', 4, 4),
(3, 'The first area', 7, 7),
(3, 'The second area', 7, 7)

--- Seat
INSERT INTo dbo.Seat
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
INSERT INTo dbo.[Role]
VALUES ('User'),
('Event manager'),
('Venue manager')
