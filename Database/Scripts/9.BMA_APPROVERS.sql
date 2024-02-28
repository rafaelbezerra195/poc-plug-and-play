USE PlugAndPlay

CREATE TABLE [dbo].[BMA_APPROVERS] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [RequestId]   INT            NOT NULL,
    [Code]       NVARCHAR (20)            NOT NULL,
    [ApproveLogin]       NVARCHAR (50)            NOT NULL,
    [ApproveOrder]   INT            NULL,
    [ApproveDate]   INT            NULL,
    [IsCurrentApprover]      BIT            DEFAULT 0 NOT NULL,
    [CreateDate] DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    [UpdateDate] DATETIME2 (7)  NULL,
    CONSTRAINT [PK_BMA_APPROVERS] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BMA_APPROVERS_BMA_REQUEST_RequestId] FOREIGN KEY ([RequestId]) REFERENCES [dbo].[BMA_REQUEST] ([Id])
);


GO