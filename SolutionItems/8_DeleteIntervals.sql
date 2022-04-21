IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DeleteIntervals')
DROP PROCEDURE DeleteIntervals
GO
CREATE PROCEDURE DeleteIntervals
    @id int
AS
DELETE Intervals
WHERE Id = @id

GO

IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'TR' AND [name] = 'Intervals_DELETE')
DROP TRIGGER Intervals_DELETE;

GO

CREATE TRIGGER Intervals_DELETE ON Intervals
AFTER DELETE AS
INSERT INTO IntervalsChangeLog (IntervalId, [Start], [End], Operation)
SELECT Id, [Start], [End], 'DELETE'
FROM DELETED