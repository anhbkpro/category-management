-- Insert 100 more sample sessions
DECLARE @SessionId INT = (SELECT MAX(Id) FROM Sessions) + 1;
DECLARE @SpeakerCount INT = (SELECT COUNT(*) FROM Speakers);
DECLARE @TagCount INT = (SELECT COUNT(*) FROM Tags);
DECLARE @Counter INT = 1;

WHILE @Counter <= 100
BEGIN
    -- Insert a new session
    INSERT INTO Sessions (Title, Description, StartDate, EndDate, Location, IsOnline)
    VALUES (
        'Session ' + CAST(@Counter AS NVARCHAR(10)) + ': ' +
        CASE @Counter % 10
            WHEN 0 THEN 'Advanced Cloud Security Workshop'
            WHEN 1 THEN 'Introduction to Serverless Architecture'
            WHEN 2 THEN 'Building Modern Web Applications'
            WHEN 3 THEN 'DevOps Best Practices'
            WHEN 4 THEN 'Microservices with Kubernetes'
            WHEN 5 THEN 'Data Analytics with Cloud Services'
            WHEN 6 THEN 'Mobile App Development Fundamentals'
            WHEN 7 THEN 'AI and Machine Learning Overview'
            WHEN 8 THEN 'Scaling Web Applications'
            WHEN 9 THEN 'Frontend Development with Modern Frameworks'
        END,
        'Detailed description for session ' + CAST(@Counter AS NVARCHAR(10)) + ' covering key concepts and practical examples.',
        DATEADD(DAY, (@Counter % 30), '2025-05-01T10:00:00'),
        DATEADD(DAY, (@Counter % 30), '2025-05-01T13:00:00'),
        CASE @Counter % 5
            WHEN 0 THEN 'Online'
            WHEN 1 THEN 'Seattle'
            WHEN 2 THEN 'San Francisco'
            WHEN 3 THEN 'Boston'
            WHEN 4 THEN 'Online'
        END,
        CASE @Counter % 5
            WHEN 0 THEN 1
            WHEN 1 THEN 0
            WHEN 2 THEN 0
            WHEN 3 THEN 0
            WHEN 4 THEN 1
        END
    );

    -- Link the session with 1-2 speakers
    INSERT INTO SessionSpeakers (SessionId, SpeakerId)
    VALUES (@SessionId, (@Counter % @SpeakerCount) + 1);

    -- Add a second speaker for some sessions
    IF @Counter % 3 = 0
    BEGIN
        INSERT INTO SessionSpeakers (SessionId, SpeakerId)
        VALUES (@SessionId, ((@Counter + 1) % @SpeakerCount) + 1);
    END

    -- Link the session with 2-4 tags
    -- First tag (always add)
    INSERT INTO SessionTags (SessionId, TagId)
    VALUES (@SessionId, ((@Counter % 8) + 1));

    -- Second tag (always add)
    INSERT INTO SessionTags (SessionId, TagId)
    VALUES (@SessionId, ((@Counter % 4) + 9));

    -- Third tag (add for some sessions)
    IF @Counter % 2 = 0
    BEGIN
        INSERT INTO SessionTags (SessionId, TagId)
        VALUES (@SessionId, ((@Counter % 3) + 13));
    END

    -- Fourth tag (add for fewer sessions)
    IF @Counter % 4 = 0
    BEGIN
        INSERT INTO SessionTags (SessionId, TagId)
        VALUES (@SessionId, ((@Counter % 2) + 16));
    END

    SET @SessionId = @SessionId + 1;
    SET @Counter = @Counter + 1;
END
