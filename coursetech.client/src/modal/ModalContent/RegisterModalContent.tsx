import { useState } from "react";
import { Button, Form } from "react-bootstrap";
import { RegisterUserDto } from "../../types/dto/auth/register-user-dto";
import { AuthService } from "../../services/auth-service";
import { ApiPaths } from "../../constants/api-paths";
import { Alert } from "react-bootstrap";
import { BaseResult } from "../../types/result/base-result";
import { useModal } from "../../context/ModalContext";

const authService = new AuthService(ApiPaths.AUTH_API_PATH);

export function RegisterModalContent() {

    const { close } = useModal();

    const [formData, setFormData] = useState<RegisterUserDto>({
        login: '',
        userName: '',
        surname: '',
        dateOfBirth: new Date(),
        password: '',
        passwordConfirm: '',
    });

    const [error, setError] = useState<string | null>(null);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };



    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        if (formData.password !== formData.passwordConfirm) {
            setError('Пароли не равны');
            return;
        }

            const result: BaseResult = await authService.register(formData);

            if (result.isSuccess === false) {
                setError(result.error.message);
            } else {
                close(); 
            }
    };

    return (
        <>
        {error && <Alert variant="danger">{error}</Alert>}
        <Form onSubmit={handleSubmit}>
            <Form.Group controlId="formLogin">
                <Form.Label>Логин</Form.Label>
                <Form.Control
                    type="text"
                    name="login"
                    value={formData.login}
                    onChange={handleChange}
                    required
                />
            </Form.Group>

            <Form.Group controlId="formUserName">
                <Form.Label>Имя</Form.Label>
                <Form.Control
                    type="text"
                    name="userName"
                    value={formData.userName}
                    onChange={handleChange}
                    required
                />
            </Form.Group>

            <Form.Group controlId="formSurname">
                <Form.Label>Фамилия</Form.Label>
                <Form.Control
                    type="text"
                    name="surname"
                    value={formData.surname}
                    onChange={handleChange}
                    required
                />
            </Form.Group>

            <Form.Group controlId="formDateOfBirth">
                <Form.Label>Дата рождения</Form.Label>
                <Form.Control
                    type="date"
                    name="dateOfBirth"
                    value={formData.dateOfBirth.toISOString().split('T')[0]}
                    onChange={(e) => setFormData({ ...formData, dateOfBirth: new Date(e.target.value) })} required
                />
            </Form.Group>

            <Form.Group controlId="formPassword">
                <Form.Label>Пароль</Form.Label>
                <Form.Control
                    type="password"
                    name="password"
                    value={formData.password}
                    onChange={handleChange}
                    required
                />
            </Form.Group>

            <Form.Group controlId="formPasswordConfirm">
                <Form.Label>Подтвердите пароль</Form.Label>
                <Form.Control
                    type="password"
                    name="passwordConfirm"
                    value={formData.passwordConfirm}
                    onChange={handleChange}
                    required
                />
            </Form.Group>

            <Button className="mt-3" variant="primary" type="submit">
                Зарегистрироваться
            </Button>
            </Form>
        </>
    );
}