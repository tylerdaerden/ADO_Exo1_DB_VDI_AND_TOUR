CREATE PROCEDURE [dbo].[UpdateStudent]
	@id int,
	@section_id int,
	@year_result int
AS
UPDATE [Student]
SET
[SectionID] = @section_id,
[YearResult] = @year_result
WHERE [ID] = @id;

