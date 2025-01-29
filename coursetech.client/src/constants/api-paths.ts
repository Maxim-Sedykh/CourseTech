export class ApiPaths {
    // ������� URL API
    private static readonly BASE_URL: string = "http://localhost:8080/api/v1";

    // ��������������
    public static readonly AUTH_API_PATH: string = `${ ApiPaths.BASE_URL }/AuthApi`;
    public static readonly USER_TOKEN_API_PATH: string = `${ ApiPaths.BASE_URL }/UserToken`;

    // �����
    public static readonly LESSON_API_PATH: string = `${ApiPaths.BASE_URL }/lesson`;
    public static readonly LESSON_RECORD_API_PATH: string = `${ApiPaths.BASE_URL }/LessonRecord`;
    public static readonly QUESTION_API_PATH: string = `${ApiPaths.BASE_URL }/Question`;

    // ������� ������������
    public static readonly USER_PROFILE_API_PATH: string = `${ApiPaths.BASE_URL }/UserProfile`;

    // ���������� �����
    public static readonly COURSE_RESULT_API_PATH: string = `${ApiPaths.BASE_URL }/CourseResult`;
}