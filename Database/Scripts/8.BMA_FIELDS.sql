USE PlugAndPlay

CREATE TABLE [dbo].[BMA_FIELDS] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [RequestId]   INT            NOT NULL,
    [FieldSchemaId]   INT            NOT NULL,
    [Value]       NVARCHAR (MAX)            NOT NULL,
    [CreateDate] DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    [UpdateDate] DATETIME2 (7)  NULL,
    CONSTRAINT [PK_BMA_FIELDS] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BMA_FIELDS_BMA_REQUEST_RequestId] FOREIGN KEY ([RequestId]) REFERENCES [dbo].[BMA_REQUEST] ([Id]),
    CONSTRAINT [FK_BMA_FIELDS_BMA_FIELD_SCHEMA_RequestId] FOREIGN KEY ([FieldSchemaId]) REFERENCES [dbo].[BMA_FIELD_SCHEMA] ([Id])
);


GO