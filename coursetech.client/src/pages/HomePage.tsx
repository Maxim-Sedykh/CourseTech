import { Container, Row, Col, Button, Card } from "react-bootstrap";
import { Link } from "react-router-dom";
import { AuthService } from "../services/auth-service";
import { ApiPaths } from "../constants/api-paths";
import { useModal } from "../context/ModalContext";
import { CoursePlanModalContent } from "../modal/ModalContent/CoursePlanModalContent";
import { DocumentationModalContent } from "../modal/ModalContent/DocumentationModalContent";
import { useMemo } from "react";

const authService = new AuthService(ApiPaths.AUTH_API_PATH);

export function HomePage() {
    const { open } = useModal();
    

    
    const modalHandlers = useMemo(() => ({
        documentation: () => open(<DocumentationModalContent />, 'Документация'),
        coursePlan: () => open(<CoursePlanModalContent />, 'План курса')
    }), [open]);

    return (
        <Container fluid className="px-0">
            <Container className="bg-primary rounded-4 p-4 my-4 shadow">
                <Row className="d-flex justify-content-between align-items-center">
                    <Col lg={8} md={7} className="text-white py-3">
                        <h2 className="fw-bold mb-3">Курс по СУБД MS SQL Server c изучением основ языка запросов T-SQL</h2>
                        <p className="mb-3">
                            Изучите работу с базами данных с СУБД MS SQL Server и языком запросов T-SQL: теория и практика с разборами правильных ответов.
                            Наша система практических заданий позволит вам легко и доступно понять учебный материал.
                            Учите T-SQL быстро и эффективно!
                        </p>
                        <div className="d-grid gap-2 d-sm-block">
                            {authService.isAuthenticated() ? (
                                <Link to="/profile" className="btn btn-info btn-sm rounded-pill px-3">
                                    Перейти к обучению
                                </Link>
                            ) : (
                                <Link to="/register" className="btn btn-info btn-sm rounded-pill px-3">
                                    Начать обучение
                                </Link>
                            )}
                        </div>
                    </Col>
                    <Col lg={4} md={5} className="text-center py-3">
                        <h4 className="text-white mb-3">Курс содержит:</h4>
                        <Card className="border-0 bg-dark text-white mb-3">
                            <Card.Body className="p-2">
                                <ul className="list-unstyled">
                                    <li className="mb-1">Занятия: 10</li>
                                    <li className="mb-1">Всего заданий: 91</li>
                                    <li>Практические задания: 4</li>
                                </ul>
                            </Card.Body>
                        </Card>
                        <Button
                            variant="info"
                            size="sm"
                            className="rounded-pill px-3"
                            onClick={modalHandlers.coursePlan}
                        >
                            План обучения
                        </Button>
                    </Col>
                </Row>
            </Container>

            <Container className="my-4">
                <h3 className="fw-bold mb-4 text-center">Чему вы научитесь на этом курсе</h3>
                <Row className="g-3">
                    <Col md={4} className="d-flex flex-column align-items-center text-center">
                        <div className="main-page-pic2 rounded-circle p-3 mb-2" style={{width: '120px', height: '120px'}}>
                            <div className="img-fluid rounded-3 shadow-sm main-page-pic2"></div>
                        </div>
                        <p>Создавать так называемые «программы» внутри базы данных и управлять алгоритмами бизнес-логики</p>
                    </Col>
                    <Col md={4} className="d-flex flex-column align-items-center text-center">
                        <div className="main-page-pic3 rounded-circle p-3 mb-2" style={{width: '120px', height: '120px'}}>
                            <div  className="rounded-circle main-page-pic3"></div>
                        </div>
                        <p>Основам работы с языком SQL в Среде СУБД Microsoft SQL Server</p>
                    </Col>
                    <Col md={4} className="d-flex flex-column align-items-center text-center">
                        <div className="main-page-pic4 rounded-circle p-3 mb-2" style={{width: '120px', height: '120px'}}>
                            <div className="rounded-circle main-page-pic4"></div>
                        </div>
                        <p>Формулировать, создавать и отлаживать различные типы запросов T-SQL</p>
                    </Col>
                </Row>
            </Container>

            <Container id="section-3" className="my-4">
                <h3 className="fw-bold mb-4 text-center">О курсе</h3>
                <Row className="align-items-center g-3">
                    <Col lg={7}>
                        <Card className="border-0 shadow-sm h-100">
                            <Card.Body className="p-3 d-flex flex-column">
                                <p className="flex-grow-1">
                                    В этом курсе вы научитесь работать: с базой данных, путём создания и чтения различных запросов к базам данных
                                    в системе управления базами данных (СУБД) "Microsoft SQL Server 2019" и с основами языка запросов T-SQL.
                                </p>
                                <div className="text-center mt-3">
                                    {authService.isAuthenticated() ? (
                                        <Link to="/user/profile" className="btn btn-info btn-sm rounded-pill px-3">
                                            Продолжите обучение!
                                        </Link>
                                    ) : (
                                        <Link to="/register" className="btn btn-info btn-sm rounded-pill px-3">
                                            Зарегистрируйтесь и начните обучение!
                                        </Link>
                                    )}
                                </div>
                            </Card.Body>
                        </Card>
                    </Col>
                    <Col lg={5}>
                        <div 
                            className="img-fluid rounded-3 shadow-sm main-page-pic5"
                            style= {{
                              height: '250px'
                            }}
                        >
                        </div>
                    </Col>
                </Row>
            </Container>

            <Container className="my-4">
                <h3 className="fw-bold mb-3">Для кого этот курс</h3>
                <Card className="border-0 bg-light mb-3">
                    <Card.Body className="p-3">
                        <p>
                            Этот курс для новичков в SQL и для людей, которые имеют некоторые
                            знания в сфере работы с данными, в частности языка SQL.
                            Если вы новичок в изучении БД, то для прохождения этого курса рекомендуется ознакомиться со справочным материалом.
                        </p>
                        <Button 
                            variant="info" 
                            size="sm" 
                            className="rounded-pill mt-2"
                            onClick={modalHandlers.documentation}
                        >
                            Документация
                        </Button>
                    </Card.Body>
                </Card>
            </Container>

            <Container id="section-2" className="my-4">
                <h3 className="fw-bold mb-3">После обучения вы сможете</h3>
                <Card className="border-0 bg-light">
                    <Card.Body className="p-3">
                        <ul>
                            <li className="mb-1">Работать в среде СУБД "MS SQL Server"</li>
                            <li className="mb-1">Писать сложные расширенные запросы используя различные функции языка T-SQL</li>
                            <li className="mb-1">Создавать запросы для выполнения операций над таблицами</li>
                            <li className="mb-1">Выполнять транзакции</li>
                            <li className="mb-1">Писать хранимые процедуры</li>
                            <li>Писать запросы, используя локальные переменные</li>
                        </ul>
                    </Card.Body>
                </Card>
            </Container>

            <Container className="my-4">
                <h3 className="fw-bold mb-3">Дополнительная литература</h3>
                <Card className="border-0 bg-light">
                    <Card.Body className="p-3">
                        <ul>
                            <li className="mb-1">
                                <a href="https://learn.microsoft.com/ru-ru/sql/t-sql/language-reference?view=sql-server-ver15" target="_blank" rel="noopener noreferrer">
                                    Справочник по Transact-SQL | Microsoft Learn
                                </a>
                            </li>
                            <li className="mb-1">
                                <a href="https://info-comp.ru/programmirovanie/412-directory-transact-sql.html" target="_blank" rel="noopener noreferrer">
                                    Справочник Transact-SQL – основы для новичков | info-comp.ru
                                </a>
                            </li>
                            <li className="mb-1">Microsoft SQL Server 2012, основы T-SQL, Бен-Ган И., Райтман М.А., 2015.</li>
                            <li>
                                <a href="https://metanit.com/sql/sqlserver/" target="_blank" rel="noopener noreferrer">
                                    MS SQL Server 2022 и T-SQL | metanit.com
                                </a>
                            </li>
                        </ul>
                    </Card.Body>
                </Card>
            </Container>
        </Container>
    );
}