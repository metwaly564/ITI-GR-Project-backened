-- =============================================
-- QUICK FIX: Add Recommendation Fields to Products Table
-- =============================================
-- Copy and paste this entire script into SQL Server Management Studio
-- and execute it on your PharmacyDB database

USE PharmacyDB;
GO

-- Add the missing columns
ALTER TABLE [Products]
ADD [SuitableForSkinTypes] NVARCHAR(MAX) NULL,
    [Benefits] NVARCHAR(MAX) NULL,
    [Ingredients] NVARCHAR(MAX) NULL,
    [Rating] DECIMAL(3,2) NULL,
    [ReviewCount] INT NULL DEFAULT 0;
GO

PRINT '✅ All recommendation fields have been added successfully!';
GO

