export interface DecodedToken {
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": string; // Имя пользователя
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": string; // Идентификатор пользователя
    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string[]; // Роли пользователя
    exp: number; // Время истечения токена
    iss: string; // Издатель токена
    aud: string; // Аудитория токена
}