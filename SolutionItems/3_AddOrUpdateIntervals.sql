IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddOrUpdateIntervals')
DROP PROCEDURE AddOrUpdateIntervals
GO
CREATE PROCEDURE AddOrUpdateIntervals
    @start time,
	@end time,
	@id int = 0
AS
IF @id = 0
	BEGIN
		INSERT INTO Intervals([Start], [End]) 
		VALUES(@start, @end)

		SET @id = @@IDENTITY
		RETURN @id
	END
ELSE
	BEGIN
		UPDATE Intervals
		SET
		[Start] = @start,
		[End] = @end
		WHERE Id = @id
	END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'TR' AND [name] = 'Intervals_INSERT')
DROP TRIGGER Intervals_INSERT;

GO

CREATE TRIGGER Intervals_INSERT ON Intervals
AFTER INSERT AS
INSERT INTO IntervalsChangeLog (IntervalId, [Start], [End], Operation)
SELECT Id, [Start], [End], 'INSERT'
FROM INSERTED

GO

IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'TR' AND [name] = 'Intervals_UPDATE')
DROP TRIGGER Intervals_UPDATE;

GO

CREATE TRIGGER Intervals_UPDATE ON Intervals
AFTER UPDATE AS
INSERT INTO IntervalsChangeLog (IntervalId, [Start], [End], Operation)
SELECT Id, [Start], [End], 'UPDATE'
FROM INSERTED