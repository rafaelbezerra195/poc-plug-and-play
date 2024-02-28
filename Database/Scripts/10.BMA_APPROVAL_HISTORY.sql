USE PlugAndPlay

CREATE TABLE [dbo].[BMA_APPROVAL_HISTORY] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [RequestId]   INT            NOT NULL,
    [ApproverId]   INT            NOT NULL,
    [Status]       NVARCHAR (20)            NULL,
    [Comments]       NVARCHAR (MAX)            NULL,
    [CreateDate] DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    [UpdateDate] DATETIME2 (7)  NULL,
    CONSTRAINT [PK_BMA_APPROVAL_HISTORY] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BMA_APPROVAL_HISTORY_BMA_REQUEST_RequestId] FOREIGN KEY ([RequestId]) REFERENCES [dbo].[BMA_REQUEST] ([Id]),
    CONSTRAINT [FK_BMA_APPROVAL_HISTORY_BMA_APPROVERS_ApproverId] FOREIGN KEY ([ApproverId]) REFERENCES [dbo].[BMA_APPROVERS] ([Id])
);


GO