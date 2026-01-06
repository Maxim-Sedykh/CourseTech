import { useEffect, useState } from "react";
import { Button, Container } from "react-bootstrap";
import { UserLessonsDto } from "../../types/dto/lesson/user-lessons-dto";
import { LessonService } from "../../services/lesson-service";
import { ApiPaths } from "../../constants/api-paths";

export function LessonsListModalContent() {

    const [userLessons, setUserLessons] = useState<UserLessonsDto>();

    const lessonService = new LessonService(ApiPaths.LESSON_API_PATH);

    const [isFetching, setIsFetching] = useState(false);


    useEffect(() => {

        if (isFetching) return;

        const fetchLessons = async () => {
            try {
                const result = await lessonService.getLessonsForUser();

                setUserLessons(result.data);
            } catch (error) {
                console.error('Error fetching lessons:', error);
            } finally {
                setIsFetching(false);
            }
        };

        fetchLessons();
    }, []);

    if (!userLessons) {
        return <div>Loading...</div>;
    }

    return (
        <Container fluid>
            <Container>
                {userLessons.lessonNames.map((lesson, index) => {
                    const isDisabled = (index) > userLessons.lessonsCompleted;

                    return (
                        <Button
                            as="a"
                            href={`/lesson/read/${lesson.id}`}
                            key={lesson.id}
                            className={`bd-indigo-800 w-100 d-block text-white mx-auto border border-white w-50 br-40 ${isDisabled ? 'disabled' : ''}`}
                            disabled={isDisabled}
                        >
                            {index + 1}. {lesson.name}
                        </Button>
                    );
                })}
            </Container>
        </Container>
    );
};