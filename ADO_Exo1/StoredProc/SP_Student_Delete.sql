CREATE PROCEDURE [dbo].[DeleteStudent]
	@Student_ID int 

AS
	UPDATE Student   SET Active = 0  WHERE ID = @Student_ID