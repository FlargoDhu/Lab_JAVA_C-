
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/30/2019 13:50:17
-- Generated from EDMX file: D:\Dane\GitHub\Lab_JAVA_C-\Projekt_1_dotNET\Stock\Stock\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Database];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Symbol]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Symbol];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Symbol'
CREATE TABLE [dbo].[Symbol] (
    [Id] int  NOT NULL,
    [Name] nchar(4000)  NULL,
    [Company] nchar(4000)  NULL,
    [Price] float  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Symbol'
ALTER TABLE [dbo].[Symbol]
ADD CONSTRAINT [PK_Symbol]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------