IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'PlugAndPlay')
BEGIN
    CREATE DATABASE PlugAndPlay;
END;