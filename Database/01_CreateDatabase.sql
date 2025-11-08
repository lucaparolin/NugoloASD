-- =============================================
-- NugoloASD - Database Creation Script
-- Multi-Tenant SaaS Platform for Sports Associations
-- =============================================

USE master;
GO

-- Drop database if exists (ONLY FOR DEVELOPMENT)
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'NugoloASD')
BEGIN
    ALTER DATABASE NugoloASD SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE NugoloASD;
END
GO

-- Create database
CREATE DATABASE NugoloASD;
GO

USE NugoloASD;
GO

-- Enable snapshot isolation for better concurrency
ALTER DATABASE NugoloASD SET READ_COMMITTED_SNAPSHOT ON;
GO

PRINT 'Database NugoloASD created successfully';
GO
