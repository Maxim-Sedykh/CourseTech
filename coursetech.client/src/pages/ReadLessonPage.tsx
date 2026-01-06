import { useState, useEffect } from "react";
import { Container, Row, Col, Button } from "react-bootstrap";
import { useParams } from "react-router-dom";
import { LessonService } from "../services/lesson-service";
import { LessonLectureDto } from "../types/dto/lesson/lesson-lecture-dto";
import { ApiPaths } from "../constants/api-paths";

const lessonService = new LessonService(ApiPaths.LESSON_API_PATH);

export function ReadLessonPage() {
    const { lessonId } = useParams<{ lessonId: string }>();
    const lessonIdNumber = Number(lessonId);

    const [lesson, setLesson] = useState<LessonLectureDto | undefined>(undefined);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchLesson = async () => {
            try {
                const response = await lessonService.getLessonLecture(lessonIdNumber);

                const data = response.data;

                setLesson(data);
            } catch (error) {
                console.error("Ошибка при загрузке лекции:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchLesson();
    }, [lessonIdNumber]);

    if (loading) return <p>Загрузка...</p>;
    if (!lesson) return <p>Лекция не найдена.</p>;

    const columnCountState = "col-md-6";

    return (
        <>
        <Container fluid>
            <Container>
                <p className="fs-2 text-center my-3">{lesson.name}</p>
                <Row>
                    <Col className="col-md-6 col-sm-12 text-center mb-2">
                        <Button className="d-block br-40 bd-cyan-500 text-white text-center fs-6 w-25 mx-auto" href="/user/profile">
                            Назад
                        </Button>
                    </Col>
                    <Col className={`${columnCountState} col-sm-12 text-center`}>
                            <Button className="d-block br-40 bd-blue-700 text-white text-center fs-6 w-50 mx-auto" as="a" href={`/lesson/pass/${lesson.id}`}>
                            Перейти к практике
                        </Button>
                    </Col>
                </Row>
                <div className = "text-justify" dangerouslySetInnerHTML={{ __html: lesson.lectureMarkup }} />
                <Row className="mt-3">
                    <Col className="col-md-6 col-sm-12 text-center mb-2">
                        <Button className="d-block br-40 bd-cyan-500 text-white text-center fs-6 w-25 mx-auto" href="/user/profile">
                            Назад
                        </Button>
                    </Col>
                    <Col className="col-md-6 col-sm-12 text-center mb-2">
                        <Button className="d-block br-40 bd-blue-700 text-white text-center fs-6 w-50 mx-auto" as="a" href={`/lesson/pass/${lesson.id}`}>
                            Перейти к практике
                        </Button>
                    </Col>
                </Row>
            </Container>
        </Container>
        </>
    );
}