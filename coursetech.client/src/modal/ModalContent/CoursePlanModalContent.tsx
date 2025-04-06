import { useState, useEffect } from "react";
import { ApiPaths } from "../../constants/api-paths";
import { LessonService } from "../../services/lesson-service";
import { LessonNameDto } from "../../types/dto/lesson/lesson-name-dto";
import { Container } from "react-bootstrap";

export function CoursePlanModalContent() {

    const [lessonNames, setLessonNames] = useState<LessonNameDto[]>();

    const [isFetching, setIsFetching] = useState(false);

        const lessonService = new LessonService(ApiPaths.LESSON_API_PATH);
    
        useEffect(() => {

            if (isFetching) return;

            const fetchLessons = async () => {
                try {
                    const result = await lessonService.getLessonNames();
                    setLessonNames(result.data);
                } catch (error) {
                    console.error('Error fetching lessons:', error);
                } finally {
                    setIsFetching(false);
                }
            };
    
            fetchLessons();
        }, []);
    
        if (!lessonNames) {
            return <div>Loading...</div>; // Загрузка данных
        }

    return (
        <Container fluid>
            <Container>
                <ul>
                {lessonNames.map((lesson) => {
                    return (
                        <li>
                            {lesson.name}
                        </li>
                    );
                })}
                </ul>
            </Container>
        </Container>
    )}