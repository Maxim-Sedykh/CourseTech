import { RouteConstants } from '../constants/route-constants';
import { LessonLectureDto } from '../types/dto/lesson/lesson-lecture-dto';
import { LessonNameDto } from '../types/dto/lesson/lesson-name-dto';
import { UserLessonsDto } from '../types/dto/lesson/user-lessons-dto';
import { CollectionResult } from '../types/result/collection-result';
import { DataResult } from '../types/result/data-result';
import { ApiClient } from './api-client';

export class LessonService {
   private apiClient: ApiClient;

   constructor(baseUrl: string) {
       this.apiClient = new ApiClient(baseUrl);
   }

   public async getLessonLecture(lessonId: number): Promise<DataResult<LessonLectureDto>> {
       return this.apiClient.get<DataResult<LessonLectureDto>>(`${RouteConstants.GET_LESSON_LECTURE}${lessonId}`);
   }

   public async updateLessonLecture(dto: LessonLectureDto): Promise<DataResult<LessonLectureDto>> {
       return this.apiClient.put<DataResult<LessonLectureDto>>(RouteConstants.UPDATE_LESSON_LECTURE, dto);
   }

   public async getLessonNames(): Promise<CollectionResult<LessonNameDto>> {
       const response = await this.apiClient.get<CollectionResult<LessonNameDto>>(RouteConstants.GET_LESSON_NAMES);

       const dataResponse = response as CollectionResult<LessonNameDto>

       return dataResponse;
   }

   public async getLessonsForUser(): Promise<DataResult<UserLessonsDto>> {
       return this.apiClient.get<DataResult<UserLessonsDto>>(RouteConstants.GET_LESSONS_FOR_USER);
   }
}