-- Verify Migration: AddRankAndMaxOrderQuantity
-- Run this to verify the columns were added successfully

USE [PharmacyDB];
GO

-- Check Products table columns
SELECT 
    COLUMN_NAME, 
    DATA_TYPE, 
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Products'
AND COLUMN_NAME IN ('Rank', 'MaxOrderQuantity')
ORDER BY COLUMN_NAME;
GO

-- Check Categories table columns
SELECT 
    COLUMN_NAME, 
    DATA_TYPE, 
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Categories'
AND COLUMN_NAME = 'Rank';
GO

-- Expected Results:
-- Products table should have:
--   - Rank (int, nullable)
--   - MaxOrderQuantity (int, nullable)
-- Categories table should have:
--   - Rank (int, nullable)

PRINT 'Verification complete!';
GO
