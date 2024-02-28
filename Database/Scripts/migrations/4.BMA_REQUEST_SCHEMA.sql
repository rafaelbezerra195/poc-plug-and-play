USE PlugAndPlay

CREATE TABLE [dbo].[BMA_REQUEST_SCHEMA] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Type]       NVARCHAR (5) NOT NULL,
    [Origin]     NVARCHAR (20) NOT NULL,
    [Name]       NVARCHAR (50)            NOT NULL,
    [Display]       NVARCHAR (50)            NOT NULL,
    [Active]       BIT            NOT NULL,
    [AllowComments]       BIT            NOT NULL,
    [ApprovalRuleId]   INT            NOT NULL,
    [CreateDate] DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    [UpdateDate] DATETIME2 (7)  NULL,
    CONSTRAINT [PK_BMA_REQUEST_SCHEMA] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BMA_REQUEST_SCHEMA_BMA_APPROVAL_RULES_ApprovalRuleId] FOREIGN KEY ([ApprovalRuleId]) REFERENCES [dbo].[BMA_APPROVAL_RULES] ([Id])
);


GO