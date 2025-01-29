import { RouteConstants } from "../constants/route-constants";
import { LoginUserDto } from "../types/dto/auth/login-user-dto";
import { RegisterUserDto } from "../types/dto/auth/register-user-dto";
import { BaseResult } from "../types/result/base-result";
import { ApiClient } from "./api-client";

export class AuthService {
    private apiClient: ApiClient;

    constructor(baseUrl: string) {
        this.apiClient = new ApiClient(baseUrl);
    }

    public async register(dto: RegisterUserDto): Promise<BaseResult> {
        return this.apiClient.post<BaseResult>(RouteConstants.LOGIN, dto);
    }

    public async login(dto: LoginUserDto): Promise<BaseResult> {
        return this.apiClient.post<BaseResult>(RouteConstants.LOGIN, dto);
    }
}