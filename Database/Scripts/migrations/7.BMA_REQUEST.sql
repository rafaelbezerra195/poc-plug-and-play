USE PlugAndPlay

CREATE TABLE [dbo].[BMA_REQUEST] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [RequestSchemaId]   INT            NOT NULL,
    [Requester]       NVARCHAR (50)            NOT NULL,
    [DocumentNumber]       NVARCHAR (50)            NOT NULL,
    [Status]       NVARCHAR (20)            NULL,
    [InternalUniqueKey]         NVARCHAR (255)  NOT NULL,
    [CreateDate] DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    [UpdateDate] DATETIME2 (7)  NULL,
    CONSTRAINT [PK_BMA_REQUEST] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BMA_REQUEST_BMA_REQUEST_SCHEMA_RequestSchemaId] FOREIGN KEY ([RequestSchemaId]) REFERENCES [dbo].[BMA_REQUEST_SCHEMA] ([Id])
);


GO