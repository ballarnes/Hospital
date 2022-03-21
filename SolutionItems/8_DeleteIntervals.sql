IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DeleteIntervals')
DROP PROCEDURE DeleteIntervals
GO
CREATE PROCEDURE DeleteIntervals
    @id int
AS
DELETE Intervals
WHERE Id = @id