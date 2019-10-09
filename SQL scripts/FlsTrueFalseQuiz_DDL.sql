IF NOT EXISTS (SELECT 1 FROM sys.databases AS d WHERE d.name = 'FlsTrueFalseQuiz') CREATE DATABASE FlsTrueFalseQuiz;
GO

USE FlsTrueFalseQuiz;

--IF OBJECT_ID('dbo.Results') IS NOT NULL DROP TABLE dbo.Results;
IF OBJECT_ID('dbo.Questions') IS NOT NULL DROP TABLE dbo.Questions;

CREATE TABLE dbo.Questions(
    ID INT IDENTITY(0, 1) PRIMARY KEY,
    Text NVARCHAR(1000) NOT NULL,
    Answer BIT NOT NULL,
    Explanation NVARCHAR(1000) NULL
);

IF OBJECT_ID('dbo.Results') IS NULL
BEGIN

CREATE TABLE dbo.Results(
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(200) NOT NULL,
    Answers NVARCHAR(4000) NOT NULL,
    ValidCount INT NOT NULL,
    TotalCount INT NOT NULL,
    EmailSent BIT NOT NULL,
    Nick NVARCHAR(200) NULL,
    Stack NVARCHAR(100) NULL,
    Phone NVARCHAR(25) NULL,
    Comment NVARCHAR(2000) NULL,
    Timestamp DATETIME2(7) NOT NULL CONSTRAINT DF_Results_Timestamp DEFAULT (GETDATE())
);

END;


-- Stored procedures

IF EXISTS(SELECT TOP 1 1 FROM sys.procedures WHERE NAME = 'sp_GetRandomQuestion')
BEGIN
    DROP PROCEDURE sp_GetRandomQuestion;
    PRINT 'Procedure sp_GetRandomQuestion dropped';
END
GO

IF EXISTS(SELECT TOP 1 1 FROM sys.procedures WHERE NAME = 'sp_GetQuestions')
BEGIN
    DROP PROCEDURE sp_GetQuestions;
    PRINT 'Procedure sp_GetQuestions dropped';
END
GO

IF EXISTS(SELECT TOP 1 1 FROM sys.procedures WHERE NAME = 'sp_TestResultEmail')
BEGIN
    DROP PROCEDURE sp_TestResultEmail;
    PRINT 'Procedure sp_TestResultEmail dropped';
END
GO

IF EXISTS(SELECT TOP 1 1 FROM sys.procedures WHERE NAME = 'sp_SaveTestResult')
BEGIN
    DROP PROCEDURE sp_SaveTestResult;
    PRINT 'Procedure sp_SaveTestResult dropped';
END
GO

IF type_id('type_int_ID_Table') IS NOT NULL
    DROP TYPE type_int_ID_Table;
GO

CREATE TYPE type_int_ID_Table AS TABLE (
    ID INT NOT NULL
)
GO

CREATE PROCEDURE sp_GetRandomQuestion(
    @ExcludedIdsTable type_int_ID_Table READONLY
)
AS
BEGIN
    SELECT TOP 1
        ID, Text
    FROM Questions
    WHERE ID NOT IN (SELECT
                ID
            FROM @ExcludedIdsTable)
    ORDER BY NEWID()
END
GO
PRINT 'Procedure sp_GetRandomQuestion created';
GO

CREATE PROCEDURE sp_GetQuestions(
    @IdsTable type_int_ID_Table READONLY
)
AS
BEGIN
    SELECT
        ID, Text, Answer, Explanation
    FROM Questions
    WHERE ID IN (SELECT
                ID
            FROM @IdsTable)
    ORDER BY NEWID()
END
GO
PRINT 'Procedure sp_GetQuestions created';
GO

CREATE PROCEDURE sp_TestResultEmail(
    @Email NVARCHAR(200)
)
AS
BEGIN
    SELECT COUNT(Email) AS EmailCount FROM Results WHERE Email = @Email AND EmailSent = 1
END
GO
PRINT 'Procedure sp_TestResultEmail created';
GO

CREATE PROCEDURE sp_SaveTestResult(
    @Email NVARCHAR(200),
    @Answers NVARCHAR(1000),
    @ValidCount INT,
    @TotalCount INT,
    @EmailSent BIT,
    @Nick NVARCHAR(200),
    @Stack NVARCHAR(100),
    @Phone NVARCHAR(25),
    @Comment NVARCHAR(2000)
)
AS
BEGIN
    INSERT INTO Results (Email, Answers, ValidCount, TotalCount, EmailSent, Nick, Stack, Phone, Comment)
    VALUES (@Email, @Answers, @ValidCount, @TotalCount, @EmailSent, @Nick, @Stack, @Phone, @Comment)
END
GO
PRINT 'Procedure sp_SaveTestResult created';
GO
