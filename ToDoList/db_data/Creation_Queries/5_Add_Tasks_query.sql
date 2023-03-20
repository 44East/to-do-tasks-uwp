SET IDENTITY_INSERT Tasks ON
INSERT Tasks(ID, Description,Assigned_Person, Name) VALUES
(1, N'Tap into the tree', 2, N'Woody deals'),
(2, N'Delivery pizza', 4, N'Spider-Man deals'),
(3, N'Write an article and send it to the editor.', 3, N'Super-Man deals'),
(4, N'Walking on a broken glass', 1,N'Die-Hard deals')
SET IDENTITY_INSERT Tasks OFF
