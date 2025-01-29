export interface RegisterUserDto {
    login: string;
    userName: string;
    surname: string;
    dateOfBirth: Date;
    password: string;
    passwordConfirm: string;
}