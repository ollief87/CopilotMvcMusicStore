-- Create a SQL view that summarizes the number of albums & artists per genre
CREATE VIEW [dbo].[vwMusicStoreSummary]
AS
    SELECT
        g.GenreId,
        g.Name AS GenreName,
        COUNT(DISTINCT a.AlbumId) AS AlbumCount,
        COUNT(DISTINCT a.ArtistId) AS ArtistCount
    FROM dbo.Genre g
    LEFT JOIN dbo.Album a ON a.GenreId = g.GenreId
    GROUP BY g.GenreId, g.Name