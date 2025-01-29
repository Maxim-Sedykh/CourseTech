import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from 'axios';

export class ApiClient {
    private axiosInstance: AxiosInstance;

    constructor(baseUrl: string) {
        this.axiosInstance = axios.create({
            baseURL: baseUrl,
        });
    }

    private handleError(error: unknown): never {
        if (axios.isAxiosError(error) && error.response) {
            const errorMessage = error.response.data?.message || error.message || 'Something went wrong';
            console.error('API Error:', errorMessage, error.response?.data)
            throw new Error(errorMessage);
        }
        console.error('Network error:', error)
        throw new Error('Network error');
    }

    private async request<T>(config: AxiosRequestConfig): Promise<T> {
        try {
            const response: AxiosResponse<T> = await this.axiosInstance.request<T>(config);
            return response.data;
        } catch (error) {
            this.handleError(error);
        }
    }

    public get<T>(endpoint: string, params?: unknown, config?: AxiosRequestConfig): Promise<T> {
        return this.request<T>({
            method: 'GET',
            url: endpoint,
            params: params,
            ...config,
        });
    }

    public post<T>(endpoint: string, body: unknown, config?: AxiosRequestConfig): Promise<T> {
        return this.request<T>({
            method: 'POST',
            url: endpoint,
            data: body,
            ...config,
        });
    }

    public put<T>(endpoint: string, body: unknown, config?: AxiosRequestConfig): Promise<T> {
        return this.request<T>({
            method: 'PUT',
            url: endpoint,
            data: body,
            ...config,
        });
    }


    public delete<T>(endpoint: string, params?: unknown, config?: AxiosRequestConfig): Promise<T> {
        return this.request<T>({
            method: 'DELETE',
            url: endpoint,
            params: params,
            ...config,
        });
    }
}