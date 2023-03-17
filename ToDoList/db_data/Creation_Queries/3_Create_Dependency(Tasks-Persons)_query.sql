ALTER TABLE Tasks ADD CONSTRAINT FK_Tasks_Assigned_Persons
FOREIGN KEY(Assigned_Person) REFERENCES Persons(ID)