export interface RegisterUserDto {
    login: string;
    userName: string;
    surname: string;
    dateOfBirth: Date; // ����� ������������ ������, ���� ���� �������� � ������� ISO
    password: string;
    passwordConfirm: string;
}