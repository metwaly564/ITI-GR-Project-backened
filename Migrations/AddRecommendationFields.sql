-- SQL Script to add recommendation fields to Products table
-- Run this script directly on your SQL Server database

USE PharmacyDB;
GO

-- Check if columns exist before adding them
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND name = 'SuitableForSkinTypes')
BEGIN
    ALTER TABLE [Products]
    ADD [SuitableForSkinTypes] NVARCHAR(MAX) NULL;
    PRINT 'Added SuitableForSkinTypes column';
END
ELSE
    PRINT 'SuitableForSkinTypes column already exists';
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND name = 'Benefits')
BEGIN
    ALTER TABLE [Products]
    ADD [Benefits] NVARCHAR(MAX) NULL;
    PRINT 'Added Benefits column';
END
ELSE
    PRINT 'Benefits column already exists';
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND name = 'Ingredients')
BEGIN
    ALTER TABLE [Products]
    ADD [Ingredients] NVARCHAR(MAX) NULL;
    PRINT 'Added Ingredients column';
END
ELSE
    PRINT 'Ingredients column already exists';
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND name = 'Rating')
BEGIN
    ALTER TABLE [Products]
    ADD [Rating] DECIMAL(3,2) NULL;
    PRINT 'Added Rating column';
END
ELSE
    PRINT 'Rating column already exists';
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND name = 'ReviewCount')
BEGIN
    ALTER TABLE [Products]
    ADD [ReviewCount] INT NULL DEFAULT 0;
    PRINT 'Added ReviewCount column';
END
ELSE
    PRINT 'ReviewCount column already exists';
GO

PRINT 'All recommendation fields have been added successfully!';
GO

