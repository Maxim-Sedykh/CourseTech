import { Navbar, Container, Nav, Dropdown } from "react-bootstrap";
import { useModal } from "../context/ModalContext";
import { UniversalModal } from "../modal/UniversalModal";
import { DocumentationModalContent } from "../modal/ModalContent/DocumentationModalContent";
import { RegisterModalContent } from "../modal/ModalContent/RegisterModalContent";
import { LoginModalContent } from "../modal/ModalContent/LoginModalContent";
import { AuthService } from "../services/auth-service";
import { ApiPaths } from "../constants/api-paths";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

export function Header() {
    const authService = new AuthService(ApiPaths.AUTH_API_PATH);
    const { open } = useModal();
    const navigate = useNavigate();

    const [isAuthenticated, setIsAuthenticated] = useState(authService.isAuthenticated());
    const [username, setUsername] = useState(authService.getUsername());

    useEffect(() => {
        const checkAuth = () => {
            setIsAuthenticated(authService.isAuthenticated());
            setUsername(authService.getUsername());
        };

        // Можно добавить подписку на события или использовать другие методы для проверки состояния аутентификации
        checkAuth();
    }, [authService]);

    const modalHandlers = {
        documentation: () => open(<DocumentationModalContent />, 'Документация'),
        register: () => open(<RegisterModalContent />, 'Регистрация пользователя'),
        login: () => open(<LoginModalContent />, 'Логин'),
    };

    const handleLogout = () => {
        authService.logout();
        setIsAuthenticated(false);
        setUsername('');
        navigate('/'); // Добавляем редирект на главную страницу
    };

    const renderDropdownItems = () => {
        if (isAuthenticated) {
            return (
                <>
                    <Dropdown.Item href="/user/profile">Личный кабинет</Dropdown.Item>
                    <Dropdown.Item onClick={handleLogout}>Выйти</Dropdown.Item>
                </>
            );
        }
        return (
            <>
                <Dropdown.Item onClick={modalHandlers.register}>Зарегистрироваться</Dropdown.Item>
                <Dropdown.Item onClick={modalHandlers.login}>Войти</Dropdown.Item>
            </>
        );
    };

    return (
        <>
            <Navbar bg="dark" variant="dark" expand="lg">
                <Container>
                    <Navbar.Brand href="/">CourseTech</Navbar.Brand>
                    <Nav.Link className="text-secondary ms-3" href="/">Главная</Nav.Link>
                    <Navbar.Toggle aria-controls="basic-navbar-nav" />
                    <Navbar.Collapse id="basic-navbar-nav">
                        <Nav className="ms-auto">
                            <Nav.Link href="/about" className="text-gray">Отзывы</Nav.Link>
                            <Nav.Link onClick={modalHandlers.documentation} className="text-gray">Документация</Nav.Link>
                            {isAuthenticated && <Nav.Link className="text-gray">{username}</Nav.Link>}
                            <Dropdown>
                                <Dropdown.Toggle variant="link" id="dropdown-basic" className="text-light text-decoration-none">
                                    Аккаунт
                                </Dropdown.Toggle>
                                <Dropdown.Menu>
                                    {renderDropdownItems()}
                                </Dropdown.Menu>
                            </Dropdown>
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
            <UniversalModal />
        </>
    );
}