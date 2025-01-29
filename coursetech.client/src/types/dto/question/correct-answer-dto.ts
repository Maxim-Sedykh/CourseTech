export interface ICorrectAnswerDto {
    id: number;
    correctAnswer: string;
    answerCorrectness: boolean;
}

export interface OpenQuestionCorrectAnswerDto extends ICorrectAnswerDto { }

export interface TestQuestionCorrectAnswerDto extends ICorrectAnswerDto { }

export interface PracticalQuestionCorrectAnswerDto extends ICorrectAnswerDto {
    questionUserGrade: number;
    queryResult: any[];
    userQueryAnalys: string;
}