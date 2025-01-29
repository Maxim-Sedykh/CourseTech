import { ICorrectAnswerDto } from "./correct-answer-dto";

export interface PracticeCorrectAnswersDto {
    lessonId: number;
    questionCorrectAnswers: ICorrectAnswerDto[];
}