import { useState } from "react";
import { Alert, Form, Button } from "react-bootstrap";
import { ApiPaths } from "../../constants/api-paths";
import { useModal } from "../../context/ModalContext";
import { AuthService } from "../../services/auth-service";
import { LoginUserDto } from "../../types/dto/auth/login-user-dto";

const authService = new AuthService(ApiPaths.AUTH_API_PATH);

export function LoginModalContent() {

    const { close } = useModal();

    const [formData, setFormData] = useState<LoginUserDto>({
        login: '',
        password: ''
    });

    const [error, setError] = useState<string | null>(null);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const result = await authService.login(formData);

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
                    <Form.Label>Пароль</Form.Label>
                    <Form.Control
                        type="password"
                        name="password"
                        value={formData.password}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Button className="mt-3" variant="primary" type="submit">
                    Войти
                </Button>
            </Form>
        </>
    );
}