USE PlugAndPlay

CREATE TABLE [dbo].[BMA_FIELD_SCHEMA] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [RequestSchemaId]   INT            NOT NULL,
    [Name]       NVARCHAR (50)            NOT NULL,
    [TabSchemaId]   INT            NOT NULL,
    [FieldSchemaTypesId]   INT            NOT NULL,
    [Required]       BIT            NOT NULL,
    [Display]     NVARCHAR (300) NOT NULL,
    [Visible]       BIT            NOT NULL,
    [Order]   INT            NOT NULL,
    [CreateDate] DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    [UpdateDate] DATETIME2 (7)  NULL,
    [Parent] INT,
    CONSTRAINT [PK_BMA_FIELD_SCHEMA] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BMA_FIELD_SCHEMA_BMA_REQUEST_SCHEMA_RequestSchemaId] FOREIGN KEY ([RequestSchemaId]) REFERENCES [dbo].[BMA_REQUEST_SCHEMA] ([Id]),
    CONSTRAINT [FK_BMA_FIELD_SCHEMA_BMA_TAB_SCHEMA_TabSchemaId] FOREIGN KEY ([TabSchemaId]) REFERENCES [dbo].[BMA_TAB_SCHEMA] ([Id]),
    CONSTRAINT [FK_BMA_FIELD_SCHEMA_BMA_FIELD_SCHEMA_TYPES_FieldSchemaTypeId] FOREIGN KEY ([FieldSchemaTypesId]) REFERENCES [dbo].[BMA_FIELD_SCHEMA_TYPES] ([Id]),
    CONSTRAINT [FK_BMA_FIELD_SCHEMA_BMA_FIELD_SCHEMA_FieldSchemaId] FOREIGN KEY ([Parent]) REFERENCES [dbo].[BMA_FIELD_SCHEMA] ([Id])
);


GO