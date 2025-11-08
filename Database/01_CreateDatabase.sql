-- =============================================
-- AISSURE PILOT - Database Creation Script
-- Multi-Tenant SaaS Platform for Sports Associations
-- =============================================

USE master;
GO

-- Drop database if exists (ONLY FOR DEVELOPMENT)
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'AISSURE_Pilot')
BEGIN
    ALTER DATABASE AISSURE_Pilot SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE AISSURE_Pilot;
END
GO

-- Create database
CREATE DATABASE AISSURE_Pilot;
GO

USE AISSURE_Pilot;
GO

-- Enable snapshot isolation for better concurrency
ALTER DATABASE AISSURE_Pilot SET READ_COMMITTED_SNAPSHOT ON;
GO

PRINT 'Database AISSURE_Pilot created successfully';
GO
