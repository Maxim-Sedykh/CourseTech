import axios, { AxiosInstance, AxiosRequestConfig } from 'axios';
import { BaseResult } from '../types/result/base-result';

export class ApiClient {
    private axiosInstance: AxiosInstance;

    constructor(baseUrl: string) {
        this.axiosInstance = axios.create({
            baseURL: baseUrl,
        });
    }

    private handleError(error: unknown): BaseResult {
        const apiError = { code: -1, message: 'Something went wrong' }; // Default error

        if (axios.isAxiosError(error) && error.response) {
            apiError.message = (error.response.data as any)?.message || error.message || apiError.message;
            apiError.code = error.response.status;
            console.error('API Error:', apiError.message, error.response?.data);
        } else {
            console.error('Network error:', error);
            apiError.message = 'Network error';
            apiError.code = -1;
        }

        return {
            isSuccess: false,
            error: apiError
        };
    }

    private async request<T>(config: AxiosRequestConfig): Promise<T | BaseResult> {
        try {
            const token = localStorage.getItem('token');
            const requestConfig = { ...config }; // Копируем исходную конфигурацию

            console.log(requestConfig.baseURL);
            console.log(requestConfig.url);

            if (token) {
                requestConfig.headers = {
                    ...requestConfig.headers,
                    Authorization: `Bearer ${token}`,
                };
            }

            const response = await this.axiosInstance.request<T>(requestConfig); // Используем обновленную конфигурацию
            return response.data;
        } catch (error) {
            return this.handleError(error);
        }
    }

    public get<T>(endpoint: string, params?: unknown, config?: AxiosRequestConfig): Promise<T | BaseResult> {

        if (params && typeof params !== 'object') {
            throw new TypeError('params must be an object');
        }
        
        return this.request<T>({
            method: 'GET',
            url: endpoint,
            params: params,
            ...config,
        });
    }

    public post<T>(endpoint: string, body: unknown, config?: AxiosRequestConfig): Promise<T | BaseResult> {
        return this.request<T>({
            method: 'POST',
            url: endpoint,
            data: body,
            ...config,
        });
    }

    public put<T>(endpoint: string, body: unknown, config?: AxiosRequestConfig): Promise<T | BaseResult> {
        return this.request<T>({
            method: 'PUT',
            url: endpoint,
            data: body,
            ...config,
        });
    }

    public delete<T>(endpoint: string, params?: unknown, config?: AxiosRequestConfig): Promise<T | BaseResult> {
        return this.request<T>({
            method: 'DELETE',
            url: endpoint,
            params: params,
            ...config,
        });
    }
}