IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DeleteOffices')
DROP PROCEDURE DeleteOffices
GO
CREATE PROCEDURE DeleteOffices
    @id int
AS
DELETE Offices
WHERE Id = @id

GO

IF EXISTS (SELECT * FROM sys.objects WHERE [type] = 'TR' AND [name] = 'Offices_DELETE')
DROP TRIGGER Offices_DELETE;

GO

CREATE TRIGGER Offices_DELETE ON Offices
AFTER DELETE AS
INSERT INTO OfficesChangeLog (OfficeId, Number, Operation)
SELECT Id, Number, 'DELETE'
FROM DELETED