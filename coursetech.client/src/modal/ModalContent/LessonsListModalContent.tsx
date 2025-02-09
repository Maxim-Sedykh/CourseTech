import { useEffect, useState } from "react";
import { Button, Container } from "react-bootstrap";
import { UserLessonsDto } from "../../types/dto/lesson/user-lessons-dto";
import { LessonService } from "../../services/lesson-service";
import { ApiPaths } from "../../constants/api-paths";



export function LessonsListModalContent() {
    const [userLessons, setUserLessons] = useState<UserLessonsDto>();

    const lessonService = new LessonService(ApiPaths.LESSON_API_PATH);

    useEffect(() => {
        const fetchLessons = async () => {
            try {
                const result = await lessonService.getLessonsForUser();
                setUserLessons(result.data );
            } catch (error) {
                console.error('Error fetching lessons:', error);
            }
        };

        fetchLessons();
    }, []);

    if (!userLessons) {
        return <div>Loading...</div>; // Загрузка данных
    }

    return (
        <Container fluid>
            <Container>
                {userLessons.lessonNames.map((lesson, index) => {
                    const isDisabled = index >= userLessons.lessonsCompleted;

                    return (
                        <a className={`w-100 lesson-number-${lesson.id}`} href={`/lesson/read/${lesson.id}`}>
                            <Button
                                as="a"
                                
                                key={lesson.id}
                                className={`bd-indigo-800 w-100 d-block text-white mx-auto border border-white w-50 br-40 ${userLessons.lessonsCompleted < 4 ? 'disabled' : ''}`}
                                disabled={isDisabled}
                            >
                                {index + 1}. {lesson.name}
                            </Button>
                        </a>
                        
                    );
                })}
            </Container>
        </Container>
    );
};