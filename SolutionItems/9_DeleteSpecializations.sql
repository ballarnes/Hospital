IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DeleteSpecializations')
DROP PROCEDURE DeleteSpecializations
GO
CREATE PROCEDURE DeleteSpecializations
    @id int
AS
DELETE Specializations
WHERE Id = @id

GO

IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'TR' AND [name] = 'Specializations_DELETE')
DROP TRIGGER Specializations_DELETE;

GO

CREATE TRIGGER Specializations_DELETE ON Specializations
AFTER DELETE AS
INSERT INTO SpecializationsChangeLog (SpecializationId, [Name], [Description], Operation)
SELECT Id, [Name], [Description], 'DELETE'
FROM DELETED