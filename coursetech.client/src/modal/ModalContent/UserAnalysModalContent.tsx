import { useState, useEffect } from "react";
import { Container, Row } from "react-bootstrap";
import { ApiPaths } from "../../constants/api-paths";
import { UserAnalysDto } from "../../types/dto/courseResult/user-analys-dto";
import { CourseResultService } from "../../services/course-result-service";

export function UserAnalysModalContent() {

    const [userAnalys, setUserAnalys] = useState<UserAnalysDto>();
    
    const courseResultService = new CourseResultService(ApiPaths.COURSE_RESULT_API_PATH);

    useEffect(() => {
        const fetchUserAnalys = async () => {
            try {
                const result = await courseResultService.getUserAnalys();
                setUserAnalys(result.data );
            } catch (error) {
                console.error('Error fetching lessons:', error);
            }
        };

        fetchUserAnalys();
    }, []);

    if (!userAnalys) {
        return <div>Loading...</div>; // Загрузка данных
    }


    return (
        <Container fluid>
            <Row>
                <p>
                    {userAnalys?.analys}
                </p>
            </Row>
        </Container>
    );
}