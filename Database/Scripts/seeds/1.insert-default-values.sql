USE PlugAndPlay

BEGIN TRANSACTION;

    INSERT INTO dbo.BMA_APPROVAL_RULES(name) 
    values ('sequencial'), 
           ('parallel');

    INSERT INTO dbo.BMA_FIELD_SCHEMA_TYPES(name) 
    values ('string'), 
           ('int'),
           ('currency'),
           ('complex');

COMMIT TRANSACTION;