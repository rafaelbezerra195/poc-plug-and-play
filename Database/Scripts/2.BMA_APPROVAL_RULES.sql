USE PlugAndPlay

CREATE TABLE [dbo].[BMA_APPROVAL_RULES] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (50)            NOT NULL,
    CONSTRAINT [PK_BMA_APPROVAL_RULES] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO