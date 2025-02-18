import { LessonDto } from "./lesson-dto";

export interface UserLessonsDto {
    lessonsCompleted: number;
    lessonNames: LessonDto[];
    lessonsCount: number;
}