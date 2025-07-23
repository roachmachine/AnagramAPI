
CREATE PROCEDURE get_basic_english_dictionary

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT * FROM [dbo].[Dictionary]
	where [word_type] in ('english-words.10','english-words.20','english-words.35','english-words.40', 'english-words.50', 'english-words.55', 'english-words.60', 'english-words.70')
END
