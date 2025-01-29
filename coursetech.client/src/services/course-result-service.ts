import { RouteConstants } from "../constants/route-constants";
import { CourseResultDto } from "../types/dto/courseResult/course-result-dto";
import { UserAnalysDto } from "../types/dto/courseResult/user-analys-dto";
import { DataResult } from "../types/result/data-result";
import { ApiClient } from "./api-client";

export class AuthService {
    private apiClient: ApiClient;

    constructor(baseUrl: string) {
        this.apiClient = new ApiClient(baseUrl);
    }

    public async getUserAnalys(): Promise<DataResult<UserAnalysDto>> {
        return this.apiClient.get<DataResult<UserAnalysDto>>(RouteConstants.GET_USER_ANALYS);
    }

    public async getCourseResult(): Promise<DataResult<CourseResultDto>> {
        return this.apiClient.get<DataResult<CourseResultDto>>(RouteConstants.GET_COURSE_RESULT);
    }
}