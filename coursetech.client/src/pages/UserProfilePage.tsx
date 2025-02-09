import { useState, useEffect } from "react";
import { Button, Card, Col, Container, Row } from "react-bootstrap";
import { UserProfileDto } from "../types/dto/userProfile/user-profile-dto";
import { UserProfileService } from "../services/user-profile-service";
import { ApiPaths } from "../constants/api-paths";
import { AuthService } from "../services/auth-service";
import { LessonsListModalContent } from "../modal/ModalContent/LessonsListModalContent";
import { useModal } from "../context/ModalContext";

export function UserProfilePage() {

    const authService = new AuthService(ApiPaths.AUTH_API_PATH);

    const { open } = useModal();

    const [userProfile, setUserProfile] = useState<UserProfileDto | null>(null);
    const [isCurrentUserProfile, setIsCurrentUserProfile] = useState(true);
    const [profileCardColor, setProfileCardColor] = useState('bd-cyan-500');
    const [examRowStyle, setExamRowStyle] = useState('col-md-12');
    const currentLogin = authService.getUsername();
    let currentUserId = authService.getUserId();

    const modalHandlers = {
            lessonList: () => open(<LessonsListModalContent />, 'Список уроков'),
        };

    useEffect(() => {
        const fetchUserProfile = async () => {
            try {
                const userProfileService = new UserProfileService(ApiPaths.USER_PROFILE_API_PATH);

                if(!currentUserId) {
                    currentUserId = "костыль, позже будет пофикшено"
                }

                const response = await userProfileService.getUserProfileAsync(currentUserId);

                if (response.data) {
                    setUserProfile(response.data);
                } else {
                    console.error("Error fetching user profile:", response.error);
                    // Handle error appropriately (e.g., display an error message)
                }
            } catch (error) {
                console.error("Error fetching user profile:", error);
                // Handle error appropriately (e.g., display an error message)
            }
        };

        if (currentLogin) {
            fetchUserProfile();
        }
    }, [currentLogin]);

    useEffect(() => {
        if (userProfile) {
            setIsCurrentUserProfile(userProfile.login === currentLogin);

            if (userProfile.login !== currentLogin) {
                setProfileCardColor("bd-cyan-800");
            } else {
                setProfileCardColor("bd-cyan-500");
            }
            setExamRowStyle(userProfile.isExamCompleted ? "col-md-6" : "col-md-12");
        }
    }, [userProfile, currentLogin]);

    if (!userProfile) {
        return <div>Loading...</div>; // Or display a loading spinner
    }

    return (<>
        <Container fluid>
            <Container>
                <div className="text-center my-2 fs-3">
                    {isCurrentUserProfile ? (<p>Ваш профиль</p>
                    ) : (
                        <>
                            <p>Профиль пользователя {userProfile.login}</p>
                            <br />
                            <a
                                href={`/profile/${currentLogin}`} // Adjust route
                                className="my-3 fs-5 text-black"
                            >
                                Вернуться к себе
                            </a>
                        </>
                    )}
                </div>
                <div className={`personalArea-userinfo ${profileCardColor} text-white p-3 me-0 my-3`} style={{ fontSize: '20px', borderRadius: '40px' }}>
                    {userProfile.isExamCompleted && (
                        <div className="w-25 border border-white br-40 mx-auto mb-3 fs-5 bd-cyan-800">
                            <p className="fs-6 text-center mx-auto my-1">Вы закончили этот курс!</p>
                        </div>
                    )}

                    <Row>
                        <Col md={6} sm={12} className="text-white">
                            <Card className="personalProfileCard bd-cyan-500 pt-4 p-3 mb-2 border border-white br-40">
                                <Card.Body>
                                    <Row className="mb-3">
                                        <Col><label>Логин :</label></Col>
                                        <Col>{userProfile.login}</Col>
                                    </Row>
                                    <Row className="mb-3">
                                        <Col><label>Имя :</label></Col>
                                        <Col>{userProfile.name}</Col>
                                    </Row>
                                    <Row className="mb-3">
                                        <Col><label>Фамилия :</label></Col>
                                        <Col>{userProfile.surname}</Col>
                                    </Row>
                                    <Row className="mb-3">
                                        <Col><label>Возраст :</label></Col>
                                        <Col>{userProfile.age}</Col>
                                    </Row>
                                </Card.Body>
                            </Card>
                        </Col>
                        <Col md={6} sm={12}>
                            <Card className="personalProfileCard bd-cyan-500 ps-4 pt-4 pb-0 mb-2 border border-white br-40">
                                <Card.Body>
                                    <p className="mb-3">
                                        Окончательный результат:
                                        {userProfile.isExamCompleted ? (
                                            <>{userProfile.currentGrade} баллов из 100</>
                                        ) : (
                                            <> не получен</>
                                        )}
                                    </p>
                                    <p>Уроков пройдено: {userProfile.lessonsCompleted}</p>
                                </Card.Body>
                            </Card>
                        </Col>
                    </Row>
                </div>
                <Row className="mx-auto mb-4 mt-0 align-content-center w-75 bd-cyan-500 br-40" style={{ minHeight: '75px' }}>
                    <Col md={6} className={`${examRowStyle} col-sm-12 text-center`}>
                        <p>Данные о прохождении курса недоступны</p>
                    </Col>
                    <Col md={6} className={`${examRowStyle} col-sm-12 text-center`}>
                        <Button onClick={modalHandlers.lessonList}>Список уроков</Button>
                    </Col>
                </Row>
            </Container>
        </Container>
    </>);
};