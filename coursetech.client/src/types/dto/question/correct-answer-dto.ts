export interface ICorrectAnswerDto {
    id: number;
    correctAnswer: string;
    answerCorrectness: boolean;
    questionType: string;
}

export interface OpenQuestionCorrectAnswerDto extends ICorrectAnswerDto { }

export interface TestQuestionCorrectAnswerDto extends ICorrectAnswerDto { }

export interface PracticalQuestionCorrectAnswerDto extends ICorrectAnswerDto {
    questionUserGrade: number;
    userQueryResult: any[];
    correctQueryResult: any[];
    chatGptAnalysis: ChatGptAnalysResponseDto;
}

export interface ChatGptAnalysResponseDto {
    UserQueryAnalys: string;
    Vertexes: Vertex[];
    Edges: Edge[];
}

export interface Vertex {
    number: number;
    name: string;
}

export interface Edge {
    from: number;
    to: number;
}