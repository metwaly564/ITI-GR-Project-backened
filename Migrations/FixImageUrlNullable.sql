-- SQL Script to make ImageUrl nullable in Products table
-- Run this script directly on your SQL Server database

USE PharmacyDB;
GO

-- Make ImageUrl nullable
ALTER TABLE [Products]
ALTER COLUMN [ImageUrl] NVARCHAR(500) NULL;
GO

PRINT 'ImageUrl column is now nullable!';
GO

