import { LessonTypes } from "../../../enums/lesson-types";
import { IQuestionDto } from "./question-dto-interface";

export interface LessonPracticeDto {
    lessonId: number;
    lessonType: LessonTypes;
    questions: IQuestionDto[];
    isDemoMode: boolean;
}