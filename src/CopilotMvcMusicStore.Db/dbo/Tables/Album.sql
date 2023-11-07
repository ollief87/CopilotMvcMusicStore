CREATE TABLE [dbo].[Album] (
    [AlbumId]     INT             IDENTITY (1, 1) NOT NULL,
    [GenreId]     INT             NOT NULL,
    [ArtistId]    INT             NOT NULL,
    [Title]       NVARCHAR (MAX)  NOT NULL,
    [Price]       DECIMAL (18, 2) NOT NULL,
    [AlbumArtUrl] NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_Album] PRIMARY KEY CLUSTERED ([AlbumId] ASC),
    CONSTRAINT [FK_Album_Artist_ArtistId] FOREIGN KEY ([ArtistId]) REFERENCES [dbo].[Artist] ([ArtistId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Album_Genre_GenreId] FOREIGN KEY ([GenreId]) REFERENCES [dbo].[Genre] ([GenreId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Album_ArtistId]
    ON [dbo].[Album]([ArtistId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Album_GenreId]
    ON [dbo].[Album]([GenreId] ASC);

