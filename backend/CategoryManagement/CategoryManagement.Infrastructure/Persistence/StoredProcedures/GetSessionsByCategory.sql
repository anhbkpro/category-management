CREATE PROCEDURE [dbo].[GetSessionsByCategory]
    @CategoryId INT,
    @Page INT = 1,
    @PageSize INT = 10,
    @SortBy NVARCHAR(50) = 'StartDate',
    @Ascending BIT = 0,
    @TotalCount INT OUTPUT
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

    -- Create a temporary table to store the filtered sessions
    CREATE TABLE #FilteredSessions (
        Id INT PRIMARY KEY
    );

    -- Get initial set of sessions with IncludeTag conditions (Type = 0)
    INSERT INTO #FilteredSessions (Id)
    SELECT DISTINCT s.Id
    FROM Sessions s
    INNER JOIN SessionTags st ON s.Id = st.SessionId
    INNER JOIN Tags t ON st.TagId = t.Id
    INNER JOIN CategoryConditions cc ON cc.Value = t.Name
    WHERE cc.CategoryId = @CategoryId
    AND cc.Type = 0; -- IncludeTag

    -- Remove sessions that match ExcludeTag conditions (Type = 1)
    DELETE fs
    FROM #FilteredSessions fs
    INNER JOIN Sessions s ON fs.Id = s.Id
    INNER JOIN SessionTags st ON s.Id = st.SessionId
    INNER JOIN Tags t ON st.TagId = t.Id
    INNER JOIN CategoryConditions cc ON cc.Value = t.Name
    WHERE cc.CategoryId = @CategoryId
    AND cc.Type = 1; -- ExcludeTag

    -- Apply Location filter (Type = 2)
    DELETE fs
    FROM #FilteredSessions fs
    INNER JOIN Sessions s ON fs.Id = s.Id
    INNER JOIN CategoryConditions cc ON cc.CategoryId = @CategoryId
    WHERE cc.Type = 2 -- Location
    AND s.Location != cc.Value;

    -- Apply StartDateMin filter (Type = 3)
    DELETE fs
    FROM #FilteredSessions fs
    INNER JOIN Sessions s ON fs.Id = s.Id
    INNER JOIN CategoryConditions cc ON cc.CategoryId = @CategoryId
    WHERE cc.Type = 3 -- StartDateMin
    AND s.StartDate < CAST(cc.Value AS DATETIME);

    -- Apply StartDateMax filter (Type = 4)
    DELETE fs
    FROM #FilteredSessions fs
    INNER JOIN Sessions s ON fs.Id = s.Id
    INNER JOIN CategoryConditions cc ON cc.CategoryId = @CategoryId
    WHERE cc.Type = 4 -- StartDateMax
    AND s.StartDate > CAST(cc.Value AS DATETIME);

    -- Get the total count after all filters
    SELECT @TotalCount = COUNT(*) FROM #FilteredSessions;

    -- Get the final paginated results with all session data
    DECLARE @Sql NVARCHAR(MAX);
    SET @Sql = N'
    SELECT s.*
    FROM Sessions s
    INNER JOIN #FilteredSessions fs ON s.Id = fs.Id
    ORDER BY ' + @OrderBy + '
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY';

    EXEC sp_executesql @Sql,
        N'@CategoryId INT, @Offset INT, @PageSize INT',
        @CategoryId, @Offset, @PageSize;

    -- Clean up
    DROP TABLE #FilteredSessions;
END
