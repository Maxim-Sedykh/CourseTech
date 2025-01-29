import { LessonTypes } from "../../../enums/lesson-types";

export interface LessonDto {
    id: number;
    name: string;
    lessonType: LessonTypes;
}