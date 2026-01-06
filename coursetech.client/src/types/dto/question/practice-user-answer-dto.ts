import { IUserAnswerDto } from "./user-answer-dto";

export interface PracticeUserAnswersDto {
    lessonId: number;
    userAnswerDtos: IUserAnswerDto[];
}