-- Migration: AddRankAndMaxOrderQuantity
-- Date: 2025-01-20
-- Description: Adds Rank and MaxOrderQuantity fields to Products and Rank to Categories

USE [PharmacyDB]; -- Your database name from appsettings.json
GO

-- Add Rank column to Products table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND name = 'Rank')
BEGIN
    ALTER TABLE [dbo].[Products]
    ADD [Rank] INT NULL;
    PRINT 'Added Rank column to Products table';
END
ELSE
BEGIN
    PRINT 'Rank column already exists in Products table';
END
GO

-- Add MaxOrderQuantity column to Products table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND name = 'MaxOrderQuantity')
BEGIN
    ALTER TABLE [dbo].[Products]
    ADD [MaxOrderQuantity] INT NULL;
    PRINT 'Added MaxOrderQuantity column to Products table';
END
ELSE
BEGIN
    PRINT 'MaxOrderQuantity column already exists in Products table';
END
GO

-- Add Rank column to Categories table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND name = 'Rank')
BEGIN
    ALTER TABLE [dbo].[Categories]
    ADD [Rank] INT NULL;
    PRINT 'Added Rank column to Categories table';
END
ELSE
BEGIN
    PRINT 'Rank column already exists in Categories table';
END
GO

PRINT 'Migration completed successfully!';
GO
