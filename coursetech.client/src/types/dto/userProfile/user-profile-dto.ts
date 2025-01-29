export interface UserProfileDto {
    id: number;
    login: string;
    name: string;
    surname: string;
    age: number;
    userId: string;
    currentGrade: number;
    isExamCompleted: boolean;
    lessonsCompleted: number;
    isEditable: boolean;
}