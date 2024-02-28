USE PlugAndPlay

CREATE TABLE [dbo].[BMA_TAB_SCHEMA] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [RequestSchemaId]   INT            NOT NULL,
    [Name]       NVARCHAR (50)            NOT NULL,
    [Display]     NVARCHAR (300) NOT NULL,
    [Order]   INT            NOT NULL,
    [CreateDate] DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    [UpdateDate] DATETIME2 (7)  NULL,
    CONSTRAINT [PK_BMA_TAB_SCHEMA] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BMA_TAB_SCHEMA_BMA_REQUEST_SCHEMA_RequestSchemaId] FOREIGN KEY ([RequestSchemaId]) REFERENCES [dbo].[BMA_REQUEST_SCHEMA] ([Id])
);


GO