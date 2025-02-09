import { useState, useEffect } from "react";
import { ApiPaths } from "../../constants/api-paths";
import { LessonService } from "../../services/lesson-service";
import { LessonNameDto } from "../../types/dto/lesson/lesson-name-dto";
import { Container, Button } from "react-bootstrap";

export function CoursePlanModalContent() {

    const [lessonNames, setLessonNames] = useState<LessonNameDto[]>();
    
        const lessonService = new LessonService(ApiPaths.LESSON_API_PATH);
    
        useEffect(() => {
            const fetchLessons = async () => {
                try {
                    const result = await lessonService.getLessonNames();
                    setLessonNames(result.data);
                } catch (error) {
                    console.error('Error fetching lessons:', error);
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