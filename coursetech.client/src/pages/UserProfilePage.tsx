import { useState, useEffect } from "react";
import { Button, Card, Col, Container, Row, Table, Spinner, Badge } from "react-bootstrap";
import { UserProfileDto } from "../types/dto/userProfile/user-profile-dto";
import { UserProfileService } from "../services/user-profile-service";
import { ApiPaths } from "../constants/api-paths";
import { AuthService } from "../services/auth-service";
import { LessonsListModalContent } from "../modal/ModalContent/LessonsListModalContent";
import { useModal } from "../context/ModalContext";
import { Alert } from "react-bootstrap";
import { LessonRecordDto } from "../types/dto/lessonRecord/lesson-record-dto";
import { LessonRecordService } from "../services/lesson-record-service";

const authService = new AuthService(ApiPaths.AUTH_API_PATH);

const userProfileService = new UserProfileService(ApiPaths.USER_PROFILE_API_PATH);

export function UserProfilePage() {

    const { open } = useModal();

    const [userProfile, setUserProfile] = useState<UserProfileDto | null>(null);
    const [lessonRecords, setLessonRecords] = useState<LessonRecordDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [recordsLoading, setRecordsLoading] = useState(true);
    const [isCurrentUserProfile, setIsCurrentUserProfile] = useState(true);
    const currentLogin = authService.getUsername();
    let currentUserId = authService.getUserId();

    const modalHandlers = {
        lessonList: () => open(<LessonsListModalContent />, 'Список уроков'),
    };

    useEffect(() => {
        const fetchData = async () => {
            try {
                setLoading(true);

                if (!currentUserId) {
                    currentUserId = "костыль, позже будет пофикшено";
                }

                const [profileResponse, recordsResponse] = await Promise.all([
                    userProfileService.getUserProfileAsync(currentUserId),
                    new LessonRecordService(ApiPaths.LESSON_RECORD_API_PATH).getLessonsRecords()
                ]);

                if (profileResponse.data) {
                    setUserProfile(profileResponse.data);
                    setIsCurrentUserProfile(profileResponse.data.login === currentLogin);
                }

                if (recordsResponse.data) {
                    setLessonRecords(recordsResponse.data);
                }

            } catch (error) {
                console.error("Error fetching data:", error);
            } finally {
                setLoading(false);
                setRecordsLoading(false);
            }
        };

        if (currentLogin) {
            fetchData();
        }
    }, [currentLogin, currentUserId]);

    if (loading) {
        return (
            <Container className="d-flex justify-content-center align-items-center" style={{ height: '80vh' }}>
                <Spinner animation="border" variant="primary" />
            </Container>
        );
    }

    if (!userProfile) {
        return (
            <Container className="text-center my-5">
                <Alert variant="danger">Не удалось загрузить профиль пользователя</Alert>
            </Container>
        );
    }

    return (
        <Container fluid className="px-0">
            <div className="bg-primary bg-gradient py-4 mb-4">
                <Container>
                    <h1 className="text-white text-center mb-0">
                        {isCurrentUserProfile ? 'Ваш профиль' : `Профиль пользователя ${userProfile.login}`}
                    </h1>
                </Container>
            </div>

            <Container>
                <Row className="g-4 mb-4">
                    <Col md={6}>
                        <Card className="shadow-sm h-100">
                            <Card.Header className="bg-primary text-white">
                                <h5 className="mb-0">Личная информация</h5>
                            </Card.Header>
                            <Card.Body>
                                <div className="d-flex flex-column">
                                    <div className="d-flex justify-content-between py-2 border-bottom">
                                        <span className="fw-bold">Логин:</span>
                                        <span>{userProfile.login}</span>
                                    </div>
                                    <div className="d-flex justify-content-between py-2 border-bottom">
                                        <span className="fw-bold">Имя:</span>
                                        <span>{userProfile.name || 'Не указано'}</span>
                                    </div>
                                    <div className="d-flex justify-content-between py-2 border-bottom">
                                        <span className="fw-bold">Фамилия:</span>
                                        <span>{userProfile.surname || 'Не указана'}</span>
                                    </div>
                                    <div className="d-flex justify-content-between py-2">
                                        <span className="fw-bold">Возраст:</span>
                                        <span>{userProfile.age || 'Не указан'}</span>
                                    </div>
                                </div>
                            </Card.Body>
                        </Card>
                    </Col>

                    <Col md={6}>
                        <Card className="shadow-sm h-100">
                            <Card.Header className="bg-primary text-white">
                                <h5 className="mb-0">Прогресс обучения</h5>
                            </Card.Header>
                            <Card.Body>
                                {userProfile.isExamCompleted ? (
                                    <div className="text-center mb-3">
                                        <Badge bg="success" className="fs-6 mb-2">
                                            Курс завершен!
                                        </Badge>
                                        <div className="d-flex align-items-center justify-content-center">
                                            <span className="me-2">Итоговый результат:</span>
                                            {userProfile.currentGrade}
                                        </div>
                                    </div>
                                ) : (
                                    <div className="text-center mb-3">
                                        <Badge bg="secondary" className="fs-6">
                                            Курс в процессе
                                        </Badge>
                                    </div>
                                )}
                                <div className="d-flex justify-content-between py-2 border-bottom">
                                    <span className="fw-bold">Пройдено уроков:</span>
                                    <span>{userProfile.lessonsCompleted}</span>
                                </div>
                                <div className="d-flex justify-content-between py-2">
                                    <span className="fw-bold">Последняя активность:</span>
                                    <span>{new Date().toLocaleDateString()}</span>
                                </div>
                            </Card.Body>
                        </Card>
                    </Col>
                </Row>

                <Row className="mb-4 g-3">
                    {userProfile.isExamCompleted && (
                        <Col md={6}>
                            <Button
                                variant="outline-primary"
                                size="lg"
                                className="w-100"
                                href="/course/result"
                            >
                                Итоги прохождения курса
                            </Button>
                        </Col>
                    )}
                    <Col md={userProfile.isExamCompleted ? 6 : 12}>
                        <Button
                            variant="primary"
                            size="lg"
                            className="w-100"
                            onClick={modalHandlers.lessonList}
                        >
                            Список уроков
                        </Button>
                    </Col>
                </Row>

                <Card className="shadow-sm mb-4">
                    <Card.Header className="bg-primary text-white">
                        <h5 className="mb-0">История прохождения уроков</h5>
                    </Card.Header>
                    <Card.Body>
                        {recordsLoading ? (
                            <div className="text-center py-4">
                                <Spinner animation="border" variant="primary" />
                            </div>
                        ) : lessonRecords.length > 0 ? (
                            <div className="table-responsive">
                                <Table striped bordered hover>
                                    <thead>
                                        <tr>
                                            <th>Название урока</th>
                                            <th>Оценка за пройденный урок</th>
                                            <th>Дата прохождения</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {lessonRecords.map((record) => (
                                            <tr>
                                                <td>{record.lessonName}</td>
                                                <td>
                                                    {record.mark}
                                                </td>
                                                <td>{new Date(record.createdAt).toLocaleDateString()}</td>
                                            </tr>
                                        ))}
                                    </tbody>
                                </Table>
                            </div>
                        ) : (
                            <Alert variant="info" className="text-center">
                                Нет данных о пройденных уроках
                            </Alert>
                        )}
                    </Card.Body>
                </Card>

                {!isCurrentUserProfile && (
                    <div className="text-center mb-4">
                        <Button variant="outline-secondary" href={`/profile/${currentLogin}`}>
                            Вернуться к своему профилю
                        </Button>
                    </div>
                )}
            </Container>
        </Container>
    );
}