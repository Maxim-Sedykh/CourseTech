import { RouteConstants } from '../constants/route-constants';
import { LessonPracticeDto } from '../types/dto/question/lesson-practice-dto';
import { PracticeCorrectAnswersDto } from '../types/dto/question/practice-correct-answer-dto';
import { PracticeUserAnswersDto } from '../types/dto/question/practice-user-answer-dto';
import { DataResult } from '../types/result/data-result';
import { ApiClient } from './api-client';

export class QuestionService {
   private apiClient: ApiClient;

   constructor(baseUrl: string) {
       this.apiClient = new ApiClient(baseUrl);
   }

   public async getLessonQuestions(lessonId: number, isDemoMode: boolean): Promise<DataResult<LessonPracticeDto>> {
        const req = new GetLessonQuestionsRequest(); // Создаем экземпляр DTO
        req.lessonId = lessonId; // Заполняем поля DTO
        req.isDemoMode = isDemoMode;

       return this.apiClient.post<DataResult<LessonPracticeDto>>(RouteConstants.GET_LESSON_QUESTIONS, req);
   }

   public async passLessonQuestions(dto: PracticeUserAnswersDto): Promise<DataResult<PracticeCorrectAnswersDto>> {
       return this.apiClient.post<DataResult<PracticeCorrectAnswersDto>>(RouteConstants.PASS_LESSON_QUESTIONS, dto);
   }
}

export class GetLessonQuestionsRequest {
    lessonId!: number; // Используем definite assignment assertion (!)
    isDemoMode!: boolean; // Используем definite assignment assertion (!)
}