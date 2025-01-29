export interface RegisterUserDto {
    login: string;
    userName: string;
    surname: string;
    dateOfBirth: Date; // Можно использовать строку, если дата приходит в формате ISO
    password: string;
    passwordConfirm: string;
}