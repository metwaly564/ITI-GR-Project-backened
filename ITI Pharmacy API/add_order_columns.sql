-- Add PhoneNumber and CityId columns to Orders table
-- This script applies the migration: AddPhoneNumberAndCityIdToOrder

-- Check if PhoneNumber column exists, if not add it
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Orders]') AND name = 'PhoneNumber')
BEGIN
    ALTER TABLE [Orders]
    ADD [PhoneNumber] nvarchar(max) NOT NULL DEFAULT '';
    PRINT 'PhoneNumber column added successfully';
END
ELSE
BEGIN
    PRINT 'PhoneNumber column already exists';
END

-- Check if CityId column exists, if not add it
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Orders]') AND name = 'CityId')
BEGIN
    ALTER TABLE [Orders]
    ADD [CityId] int NULL;
    PRINT 'CityId column added successfully';
END
ELSE
BEGIN
    PRINT 'CityId column already exists';
END

-- Create index on CityId if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Orders_CityId' AND object_id = OBJECT_ID(N'[dbo].[Orders]'))
BEGIN
    CREATE INDEX [IX_Orders_CityId] ON [Orders] ([CityId]);
    PRINT 'Index IX_Orders_CityId created successfully';
END
ELSE
BEGIN
    PRINT 'Index IX_Orders_CityId already exists';
END

-- Add foreign key constraint if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Orders_Cities_CityId')
BEGIN
    ALTER TABLE [Orders]
    ADD CONSTRAINT [FK_Orders_Cities_CityId] 
    FOREIGN KEY ([CityId]) 
    REFERENCES [Cities] ([Id]) 
    ON DELETE NO ACTION;
    PRINT 'Foreign key FK_Orders_Cities_CityId created successfully';
END
ELSE
BEGIN
    PRINT 'Foreign key FK_Orders_Cities_CityId already exists';
END

PRINT 'Migration completed successfully!';

