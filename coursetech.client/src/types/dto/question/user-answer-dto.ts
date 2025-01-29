export interface IUserAnswerDto {
    questionId: number;
}

export interface OpenQuestionUserAnswerDto extends IUserAnswerDto {
    userAnswer: string;
}

export interface PracticalQuestionUserAnswerDto extends IUserAnswerDto {
    userCodeAnswer: string;
}

export interface TestQuestionUserAnswerDto extends IUserAnswerDto {
    userAnswerNumberOfVariant: number;
}