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