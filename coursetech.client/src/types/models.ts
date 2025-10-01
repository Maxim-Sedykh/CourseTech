export interface LoginRequest {
    email: string;
    password: string;
    rememberMe?: boolean;
}

export interface RegisterRequest {
    email: string;
    password: string;
    firstName: string;
    lastName: string;
}

export interface SessionConfigRequest {
    categoryId: string;
    difficulty: 'Junior' | 'Middle' | 'Senior';
}

export interface AttemptRequest {
    sessionId: string;
    questionId: string;
    audioFile: File;
}

// Response модели
export interface AuthResponse {
    success: boolean;
    token?: string;
    user?: UserModel;
    errors?: string[];
}

export interface AttemptResultResponse {
    attempt: AttemptModel;
    analysis: AiAnalysisModel;
}

export interface UserStatisticsResponse {
    totalAttempts: number;
    averageScore: number;
    sessionsCompleted: number;
    bestCategory: string;
    categoryProgress: CategoryProgress[];
}

export interface CategoryProgress {
    categoryName: string;
    averageScore: number;
    attemptsCount: number;
}