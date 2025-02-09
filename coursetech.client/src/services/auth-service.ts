import { jwtDecode } from "jwt-decode";
import { RouteConstants } from "../constants/route-constants";
import { DecodedToken } from "../types/auth/decoded-token";
import { LoginUserDto } from "../types/dto/auth/login-user-dto";
import { RegisterUserDto } from "../types/dto/auth/register-user-dto";
import { BaseResult } from "../types/result/base-result";
import { ApiClient } from "./api-client";
import { DataResult } from "../types/result/data-result";
import { TokenDto } from "../types/dto/auth/token-dto";

export class AuthService {
    private apiClient: ApiClient;

    constructor(baseUrl: string) {
        this.apiClient = new ApiClient(baseUrl);
    }

    public async register(dto: RegisterUserDto): Promise<BaseResult> {
        return this.apiClient.post(RouteConstants.REGISTER, dto);
    }

    public async login(dto: LoginUserDto): Promise<DataResult<TokenDto>> {
        const response = await this.apiClient.post(RouteConstants.LOGIN, dto);

        const dataResult = response as DataResult<TokenDto>;

        if (dataResult.data?.accessToken) {
            localStorage.setItem('token', dataResult.data.accessToken);
        }

        if (dataResult.data?.accessToken) {
            const decoded = jwtDecode(dataResult.data.accessToken);
            console.log(decoded); // Выводит содержимое декодированного токена
        }
        return dataResult;
    }

    public logout(): void {
        localStorage.removeItem('token');
    }

    public getToken(): string | null {
        return localStorage.getItem('token');
    }

    public isLoggedIn(): boolean {
        return !!this.getToken();
    }

    public isAuthenticated(): boolean {
        return this.isLoggedIn();
    }

    public isInRole(role: string): boolean {
        const token = this.getToken();
        if (!token) return false;

        try {
            const decoded: DecodedToken = jwtDecode(token);
            return decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] ? decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"].includes(role) : false;
        } catch (error) {
            return false;
        }
    }

    public getUsername(): string | null {
        const token = this.getToken();
        if (!token) return null;
    
        try {
            const decoded: DecodedToken = jwtDecode(token);

            return decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] || null; // Используйте правильный ключ
        } catch (error) {
            console.error("Ошибка декодирования токена:", error);
            return null;
        }
    }

    public getUserId(): string | null {
        const token = this.getToken();
        if (!token) return null;
    
        try {
            const decoded: DecodedToken = jwtDecode(token);

            return decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"] || null; // Используйте правильный ключ
        } catch (error) {
            console.error("Ошибка декодирования токена:", error);
            return null;
        }
    }
}