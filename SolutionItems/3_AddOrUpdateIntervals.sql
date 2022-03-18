IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'AddOrUpdateIntervals')
DROP PROCEDURE AddOrUpdateIntervals
GO
CREATE PROCEDURE AddOrUpdateIntervals
    @id int,
    @start smalldatetime,
	@end smalldatetime
AS
IF @id = 0
	INSERT INTO Intervals(Id, [Start], [End]) 
	VALUES(@id, @start, @end)
ELSE
	UPDATE Intervals
	SET
	Id = @id,
	[Start] = @start,
	[End] = @end
SET @id = @@IDENTITY
RETURN @id