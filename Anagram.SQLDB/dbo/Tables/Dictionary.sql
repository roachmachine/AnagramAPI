CREATE TABLE [dbo].[Dictionary] (
    [word_id]            INT           IDENTITY (1, 1) NOT NULL,
    [word]               VARCHAR (200) NOT NULL,
    [word_ordered_array] VARCHAR (200) NOT NULL,
    [word_type]          VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Words] PRIMARY KEY CLUSTERED ([word_id] ASC)
);

