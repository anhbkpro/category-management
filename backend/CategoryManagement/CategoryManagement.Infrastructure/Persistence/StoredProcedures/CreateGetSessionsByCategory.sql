-- Drop the procedure if it exists
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSessionsByCategory]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[GetSessionsByCategory]
GO

-- Create the procedure
CREATE PROCEDURE [dbo].[GetSessionsByCategory]
    @CategoryId INT,
    @Page INT = 1,
    @PageSize INT = 10,
    @SortBy NVARCHAR(50) = 'StartDate',
    @Ascending BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@Page - 1) * @PageSize;
    DECLARE @OrderBy NVARCHAR(100);

    -- Build the ORDER BY clause based on the sort parameter
    SET @OrderBy = CASE @SortBy
        WHEN 'Title' THEN 's.Title'
        WHEN 'StartDate' THEN 's.StartDate'
        WHEN 'EndDate' THEN 's.EndDate'
        WHEN 'Location' THEN 's.Location'
        ELSE 's.StartDate'
    END;

    SET @OrderBy = @OrderBy + CASE WHEN @Ascending = 1 THEN ' ASC' ELSE ' DESC' END;

    -- Get the total count
    DECLARE @TotalCount INT;
    SELECT @TotalCount = COUNT(DISTINCT s.Id)
    FROM Sessions s
    INNER JOIN SessionTags st ON s.Id = st.SessionId
    INNER JOIN Tags t ON st.TagId = t.Id
    INNER JOIN CategoryConditions cc ON cc.Value = t.Name AND cc.Type = 1 -- Type 1 for Tag conditions
    WHERE cc.CategoryId = @CategoryId;

    -- Get the paginated results
    DECLARE @Sql NVARCHAR(MAX);
    SET @Sql = N'
    SELECT s.*, @TotalCount AS TotalCount
    FROM Sessions s
    INNER JOIN SessionTags st ON s.Id = st.SessionId
    INNER JOIN Tags t ON st.TagId = t.Id
    INNER JOIN CategoryConditions cc ON cc.Value = t.Name AND cc.Type = 1
    WHERE cc.CategoryId = @CategoryId
    ORDER BY ' + @OrderBy + '
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY';

    EXEC sp_executesql @Sql,
        N'@CategoryId INT, @Offset INT, @PageSize INT, @TotalCount INT',
        @CategoryId, @Offset, @PageSize, @TotalCount;
END
GO
