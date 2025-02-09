import { Container, Row, Col } from "react-bootstrap";

export function DocumentationModalContent() {
    return (
        <Container fluid>
            <Row>
                <Col md={12} className="color-white br-bottom-40 fs-6 documentation-modal-content">
                    <ul>
                        <li className = "mb-2">
                            <a href="https://habr.com/ru/articles/193136/">Хабр | Руководство по проектированию реляционных баз данных
                            </a>
                        </li>
                        <li className="mb-2">
                            <a href="https://habr.com/ru/articles/440556/">
                                Хабр | Учимся программированию Entity Relationship – диаграмм
                            </a>
                        </li>
                        <li className="mb-2">
                            <a href="https://plantuml.com/ru/ie-diagram">
                                Ezoic | Диаграмма взаимосвязи сущностей
                            </a>
                        </li>
                        <li className="mb-2">
                            <a href="https://habr.com/ru/articles/254773/">
                                Хабр | Нормализация отношений. Шесть нормальных форм
                            </a>
                        </li>
                        <li className="mb-2">
                            <a href="https://falconspace.ru/list/sqlserver/sozdanie-bazy-dannykh--tablic-i-svyazey-mezhdu-nimi---489">
                                Falcon Space | SQL Server. Создание базы данных, таблиц и связей
                                между ними
                            </a>
                        </li>
                    </ul>
                </Col>
            </Row>
        </Container>
    );
}