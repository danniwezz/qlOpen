ALTER TABLE [AssignedService] ADD [Price] decimal(18,4) NOT NULL
ALTER TABLE [AssignedService] ADD [Currency] NVARCHAR(3) NOT NULL
ALTER TABLE [AssignedService] ADD [StartDate] DATE NOT NULL
ALTER TABLE [AssignedService] ADD [EndDate] DATE NULL
ALTER TABLE [AssignedService] ADD [ValidToWeekDayNumber] INT NOT NULL
ALTER TABLE [AssignedService] ADD [ValidFromWeekDayNumber] INT NOT NULL