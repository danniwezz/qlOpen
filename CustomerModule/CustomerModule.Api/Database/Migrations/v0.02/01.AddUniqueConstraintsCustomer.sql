ALTER TABLE [Discount] DROP CONSTRAINT [FK_AssignedServiceId_AssignedService]

ALTER TABLE [AssignedService] DROP CONSTRAINT [FK_CustomerId_Customer]

ALTER TABLE [Discount]  WITH CHECK ADD CONSTRAINT [FK_AssignedServiceId_AssignedService] FOREIGN KEY ([AssignedServiceId]) REFERENCES [AssignedService](Id) ON DELETE CASCADE

ALTER TABLE [Discount] CHECK CONSTRAINT [FK_AssignedServiceId_AssignedService]


ALTER TABLE [AssignedService]  WITH CHECK ADD CONSTRAINT [FK_CustomerId_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Customer](Id) ON DELETE CASCADE

ALTER TABLE [AssignedService] CHECK CONSTRAINT [FK_CustomerId_Customer]