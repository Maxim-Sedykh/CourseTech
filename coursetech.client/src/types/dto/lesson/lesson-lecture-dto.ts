import { LessonTypes } from "../../../enums/lesson-types";

export interface LessonLectureDto {
    id: number;
    name: string;
    lessonType: LessonTypes;
    lectureMarkup: string;
}