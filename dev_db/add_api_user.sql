CREATE DATABASE lego;
GO

USE lego;
GO

CREATE LOGIN [lego-api] WITH PASSWORD = 'kUb6E4dWuqTtyWZoyDwYMMF5qNYgtR';
GO

CREATE USER [lego-api] FOR LOGIN [lego-api];
GO

EXEC sp_addrolemember 'db_datareader', 'lego-api';
EXEC sp_addrolemember 'db_datawriter', 'lego-api';

GRANT CREATE TABLE, ALTER, VIEW DEFINITION TO [lego-api];
GO

ALTER LOGIN [lego-api] WITH DEFAULT_DATABASE = lego;
GO