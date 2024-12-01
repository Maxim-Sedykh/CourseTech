CREATE VIEW View_QuestionTypeGrade AS
WITH BaseQuestionCounts AS (
    SELECT
        SUM(CASE WHEN Discriminator = 'TestQuestion' THEN 1 ELSE 0 END) AS TestQuestionCount,
        SUM(CASE WHEN Discriminator = 'OpenQuestion' THEN 1 ELSE 0 END) AS OpenQuestionCount,
        SUM(CASE WHEN Discriminator = 'PracticalQuestion' THEN 1 ELSE 0 END) AS PracticalQuestionCount
    FROM BaseQuestion
),
WeightedScores AS (
    SELECT
        TestQuestionCount,
        OpenQuestionCount,
        PracticalQuestionCount,
        (TestQuestionCount * 5 + OpenQuestionCount * 15 + PracticalQuestionCount * 25) AS TotalScore
    FROM BaseQuestionCounts
),
GradePercentages AS (
  SELECT 
    TotalScore,
    ROUND((100.0 / TotalScore), 3) AS Percentage
  FROM WeightedScores
)
SELECT
  q.Discriminator AS QuestionTypeName,
  CAST(ROUND(CASE
    WHEN q.Discriminator = 'TestQuestion' THEN 5 * (SELECT Percentage FROM GradePercentages)
    WHEN q.Discriminator = 'OpenQuestion' THEN 15 * (SELECT Percentage FROM GradePercentages)
    WHEN q.Discriminator = 'PracticalQuestion' THEN 25 * (SELECT Percentage FROM GradePercentages)
    ELSE 0
  END, 3) AS REAL) AS Grade
FROM BaseQuestion q
GROUP BY q.Discriminator;