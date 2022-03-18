IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DeleteSpecializations')
DROP PROCEDURE DeleteSpecializations
GO
CREATE PROCEDURE DeleteSpecializations
    @id int
AS
DELETE Specializations
WHERE Id = @id