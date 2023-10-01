ALTER TABLE [AssignedService] ADD CONSTRAINT [UC_Service_Customer] UNIQUE ([ServiceId], [CustomerId])
ALTER TABLE [Customer] ADD CONSTRAINT [UC_CustomerName] UNIQUE ([Name])