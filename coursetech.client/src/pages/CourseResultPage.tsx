import { Container, Row, Col, Button } from "react-bootstrap";
import { CourseResultService } from "../services/course-result-service";
import { ApiPaths } from "../constants/api-paths";
import { useEffect, useState } from "react";
import { CourseResultDto } from "../types/dto/courseResult/course-result-dto";
import { UserAnalysModalContent } from "../modal/ModalContent/UserAnalysModalContent";
import { useModal } from "../context/ModalContext";

export function CourseResultPage() {

    const { open } = useModal();

    const initialCourseResult : CourseResultDto = {
        userId: '',
        login: '',
        name: '',
        surname: '',
        currentGrade: 0,
        analys: ''
    }

    const [courseResult, setCourseResult] = useState<CourseResultDto>(initialCourseResult);
    const courseResultService = new CourseResultService(ApiPaths.COURSE_RESULT_API_PATH)

    useEffect(() => {
        const fetchCourseResult = async () => {
            try {
                const result = await courseResultService.getCourseResult();
                if (result.isSuccess && result.data) {
                    setCourseResult(result.data);
                } else {
                    console.error("Failed to fetch course result:", result.error.message);
                }
            } catch (error) {
                console.error("Error fetching course result:", error);
            }
        };
    
        fetchCourseResult();
    }, []);

    const modalHandlers = {
            userAnalys: () => open(<UserAnalysModalContent />, 'Анализ прохождения курса')
        };

    return (
    <>
        <Container fluid>
            <Container>
                <Row className="my-5 bd-pink-800 br-40">
                    <Col md={3} sm={3}></Col>
                    <Col md={6} sm={6} className="text-center py-5 result-container">
                        <p className="fs-4 fw-bolder">Поздравляем {courseResult.login}!!!</p>
                        <p className="fs-5">Уважаемый {courseResult.name} {courseResult.surname}, ваш окончательный результат:</p>
                        <p className="fs-5"> {courseResult.currentGrade} баллов из 100 !!!</p>
                    </Col>
                    <Col md={3} sm={3}></Col>
                </Row>
                <Row className="text-center">
                    <Col md={6} sm={12}>
                        <Button
                            variant="primary"
                            className="result-button"
                            onClick={modalHandlers.userAnalys}
                        >
                            Просмотреть анализ
                        </Button>
                    </Col>
                    <Col md={6} sm={12}>
                        <Button variant="secondary" className="result-button" href="/">
                            На главную
                        </Button>
                    </Col>
                </Row>
            </Container>
        </Container>
    </>
    );
};