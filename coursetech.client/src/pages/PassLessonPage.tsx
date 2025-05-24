/* eslint-disable @typescript-eslint/no-unused-vars */
import { useState, useEffect, useCallback } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { ApiPaths } from "../constants/api-paths";
import { QuestionService } from "../services/question-service";
import { LessonPracticeDto } from "../types/dto/question/lesson-practice-dto";
import { Container, Row, Col, Alert, Button, FormCheck, FormControl, Form, Modal, Spinner, Dropdown } from "react-bootstrap";
import { LessonTypes } from "../enums/lesson-types";
import { PracticeUserAnswersDto } from "../types/dto/question/practice-user-answer-dto";
import { TestQuestionDto } from "../types/dto/question/question-dto";
import { IQuestionDto } from "../types/dto/question/question-dto-interface";
import { PracticeCorrectAnswersDto } from "../types/dto/question/practice-correct-answer-dto";
import { OpenQuestionUserAnswerDto, PracticalQuestionUserAnswerDto, TestQuestionUserAnswerDto } from "../types/dto/question/user-answer-dto";
import { FilmDbScheme } from "../components/LearningProcess/FilmDbScheme";
import { GraphVisualizer } from "../components/Graph/GraphVisualizer";
import { PracticalQuestionCorrectAnswerDto } from "../types/dto/question/correct-answer-dto";

function isTestQuestionDto(question: IQuestionDto): question is TestQuestionDto {
    return (question as TestQuestionDto).testVariants !== undefined;
}

const questionService = new QuestionService(ApiPaths.QUESTION_API_PATH);

export function PassLessonPage() {
    const navigate = useNavigate();
    const { lessonId, isDemoMode } = useParams<{ lessonId: string, isDemoMode: string }>();
    const lessonIdNumber = Number(lessonId);
    const isDemoModeConvert = isDemoMode === "false" ? false : true;

    const [formData, setFormData] = useState<PracticeUserAnswersDto>({
        lessonId: lessonIdNumber,
        userAnswerDtos: [],
        isDemoMode: isDemoModeConvert
    });

    const [lessonPracticeDto, setLessonPracticeDto] = useState<LessonPracticeDto>({
        lessonId: lessonIdNumber,
        lessonType: LessonTypes.Common,
        questions: [],
        isDemoMode: isDemoModeConvert
    });

    const [practiceCorrectAnswersDto, setPracticeCorrectAnswersDto] = useState<PracticeCorrectAnswersDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [isSubmitted, setIsSubmitted] = useState(false);
    const [showLoadingModal, setShowLoadingModal] = useState(false);
    const [showDbSchemeModal, setShowDbSchemeModal] = useState(false);

    const lessonContainerColor = lessonPracticeDto.lessonType === LessonTypes.Exam 
        ? "bg-indigo-900" 
        : "bg-blue-800";

    // Группируем вопросы по типам
    const testQuestions = lessonPracticeDto.questions.filter(q => q.questionType === 'TestQuestionDto');
    const openQuestions = lessonPracticeDto.questions.filter(q => q.questionType === 'OpenQuestionDto');
    const practicalQuestions = lessonPracticeDto.questions.filter(q => q.questionType === 'PracticalQuestionDto');

    const handleTestQuestionChange = useCallback((questionId: number, variantNumber: number) => {
        setFormData(prev => {
            const existingIndex = prev.userAnswerDtos.findIndex(a => a.questionId === questionId);
            const newAnswer: TestQuestionUserAnswerDto = {
                questionId,
                userAnswerNumberOfVariant: variantNumber,
                questionType: 'TestQuestionUserAnswerDto'
            };

            return {
                ...prev,
                userAnswerDtos: existingIndex !== -1
                    ? prev.userAnswerDtos.map((a, i) => i === existingIndex ? newAnswer : a)
                    : [...prev.userAnswerDtos, newAnswer]
            };
        });
    }, []);

    const handleOpenQuestionChange = useCallback((questionId: number, value: string) => {
        setFormData(prev => {
            const existingIndex = prev.userAnswerDtos.findIndex(a => a.questionId === questionId);
            const newAnswer: OpenQuestionUserAnswerDto = {
                questionId,
                userAnswer: value,
                questionType: 'OpenQuestionUserAnswerDto'
            };

            return {
                ...prev,
                userAnswerDtos: existingIndex !== -1
                    ? prev.userAnswerDtos.map((a, i) => i === existingIndex ? newAnswer : a)
                    : [...prev.userAnswerDtos, newAnswer]
            };
        });
    }, []);

    const handlePracticalQuestionChange = useCallback((questionId: number, value: string) => {
        setFormData(prev => {
            const existingIndex = prev.userAnswerDtos.findIndex(a => a.questionId === questionId);
            const newAnswer: PracticalQuestionUserAnswerDto = {
                questionId,
                userCodeAnswer: value,
                questionType: 'PracticalQuestionUserAnswerDto'
            };

            return {
                ...prev,
                userAnswerDtos: existingIndex !== -1
                    ? prev.userAnswerDtos.map((a, i) => i === existingIndex ? newAnswer : a)
                    : [...prev.userAnswerDtos, newAnswer]
            };
        });
    }, []);

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
        setShowLoadingModal(true);

        try {
            const sortedAnswers = [...formData.userAnswerDtos].sort((x, y) => x.questionId - y.questionId);
            const response = await questionService.passLessonQuestions({
                ...formData,
                userAnswerDtos: sortedAnswers
            });

            const correctAnswers = response.data as PracticeCorrectAnswersDto;
            setPracticeCorrectAnswersDto(correctAnswers);
            setIsSubmitted(true);

            if (lessonPracticeDto.isDemoMode) {
                navigate(`/lesson/read/${lessonPracticeDto.lessonId}`);
            }

            if (lessonPracticeDto?.lessonType === LessonTypes.Exam) {
                setTimeout(() => {
                    navigate('/course/result');
                }, 3000);
            }
        } catch (error) {
            console.error("Ошибка при отправке ответов:", error);
        } finally {
            setShowLoadingModal(false);
        }
    };

    useEffect(() => {
        const fetchLesson = async () => {
            try {
                const response = await questionService.getLessonQuestions(lessonIdNumber, isDemoModeConvert);
                setLessonPracticeDto(response.data as LessonPracticeDto);
            } catch (error) {
                console.error("Ошибка при загрузке вопросов:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchLesson();
    }, [lessonIdNumber]);

    const renderQuestionSection = (questions: IQuestionDto[], title: string, level: string) => {
        if (questions.length === 0) return null;

        const isPracticalSection = title === "Практические задания";

        return (
            <div className="mb-4 p-3 bg-dark rounded">
                <div className="d-flex justify-content-between align-items-center mb-3">
                    <div>
                        <h4 className="text-white mb-0">{title}</h4>
                        <p className="text-info fs-5 mb-0">{level}</p>
                    </div>
                    {isPracticalSection && (
                        <Dropdown>
                            <Dropdown.Toggle variant="outline-info">
                                Схема базы данных
                            </Dropdown.Toggle>
                            <Dropdown.Menu>
                                <Dropdown.Item onClick={() => setShowDbSchemeModal(true)}>
                                    Показать схему
                                </Dropdown.Item>
                            </Dropdown.Menu>
                        </Dropdown>
                    )}
                </div>
                {isPracticalSection && (
                    <Alert variant="info" className="mb-3">
                        Для выполнения заданий ознакомьтесь со схемой базы данных
                    </Alert>
                )}
                
                {questions.map(question => {
                    const userAnswer = formData.userAnswerDtos.find(a => a.questionId === question.id);
                    const correctAnswer = practiceCorrectAnswersDto?.questionCorrectAnswers?.find(a => a.id === question.id);
                    const practicalAnswer = correctAnswer && 'chatGptAnalysis' in correctAnswer
                        ? correctAnswer as PracticalQuestionCorrectAnswerDto
                        : null;

                    return (
                        <div key={question.id} className="mb-4 p-3 bg-secondary rounded">
                            <p className="fs-5 text-white">
                                <b>{question.number}</b>. {question.displayQuestion}
                            </p>
                            
                            {isTestQuestionDto(question) ? (
                                <div className="ps-3">
                                    {question.testVariants?.map(variant => (
                                        <div key={variant.variantNumber} className="mb-2">
                                            <FormCheck
                                                type="radio"
                                                id={`test-${question.id}-${variant.variantNumber}`}
                                                label={variant.content}
                                                name={`question-${question.id}`}
                                                required
                                                checked={(userAnswer as TestQuestionUserAnswerDto)?.userAnswerNumberOfVariant === variant.variantNumber}
                                                onChange={() => handleTestQuestionChange(question.id, variant.variantNumber)}
                                                className="text-white"
                                                disabled={isSubmitted}
                                            />
                                        </div>
                                    ))}
                                </div>
                            ) : (
                                <>
                                    {question.questionType === 'PracticalQuestionDto' ? (
                                        <FormControl
                                            as="textarea"
                                            className="bg-light text-dark p-2 rounded"
                                            placeholder="Введите ваш SQL-запрос здесь..."
                                            required
                                            style={{ minHeight: '150px' }}
                                            value={(userAnswer as PracticalQuestionUserAnswerDto)?.userCodeAnswer || ""}
                                            onChange={e => handlePracticalQuestionChange(question.id, e.target.value)}
                                            disabled={isSubmitted}
                                        />
                                    ) : (
                                        <FormControl
                                            type="text"
                                            className="bg-light text-dark p-2 rounded"
                                            placeholder="Введите ваш ответ здесь..."
                                            required
                                            value={(userAnswer as OpenQuestionUserAnswerDto)?.userAnswer || ""}
                                            onChange={e => handleOpenQuestionChange(question.id, e.target.value)}
                                            disabled={isSubmitted}
                                        />
                                    )}
                                </>
                            )}

                            {isSubmitted && (
                                <div className="mt-3">
                                    {correctAnswer?.answerCorrectness === true ? (
                                        <Alert variant="success" className="text-center">
                                            <strong>Правильно!</strong> {correctAnswer?.correctAnswer && `Правильный ответ: ${correctAnswer.correctAnswer}`}
                                        </Alert>
                                    ) : (
                                        <Alert variant="danger" className="text-center">
                                            <strong>Неправильно!</strong> Правильный ответ: {correctAnswer?.correctAnswer}
                                        </Alert>
                                    )}

                                    {practicalAnswer && correctAnswer?.answerCorrectness === false && (
                                        <div className="mt-3">
                                            <h5 className="text-center text-white">Анализ искусственного интеллекта:</h5>
                                            <GraphVisualizer
                                                data={practicalAnswer.chatGptAnalysis}
                                            />

                                            <div className="mt-4">
                                                <Alert variant="info" className="text-center">
                                                    Время выполнения вашего запроса {practicalAnswer.userQueryTime} секунд
                                                </Alert>
                                                <Alert variant="info" className="text-center">
                                                    Время выполнения корректного запроса {practicalAnswer.correctQueryTime} секунд
                                                </Alert>
                                            </div>

                                            {practicalAnswer.chatGptAnalysis?.UserQueryAnalys && (
                                                <Alert variant="info" className="mt-3">
                                                    <Alert.Heading>Анализ запроса:</Alert.Heading>
                                                    <p>{practicalAnswer.chatGptAnalysis?.UserQueryAnalys}</p>
                                                </Alert>
                                            )}
                                        </div>
                                    )}
                                </div>
                            )}
                        </div>
                    );
                })}
            </div>
        );
    };

    return (
        <Container fluid className="p-0">
            <div className={`${lessonContainerColor} py-4`}>
                <Container>
                    <h1 className="text-dark text-center mb-0">
                        {lessonPracticeDto.lessonType === LessonTypes.Exam 
                            ? "ЭКЗАМЕН" 
                            : "Практическая часть занятия"}

                        
                    </h1>
                    <h1 className="text-dark text-center mb-0">
                        {lessonPracticeDto.isDemoMode ? "Демо-режим, проверочная оценка знаний" : ""}
                    </h1>
                </Container>
            </div>

            <Container className="pb-4">
                <Row>
                    <Col md={2} className="d-none d-md-block" />
                    <Col md={8} sm={12}>
                        <Form onSubmit={handleSubmit} className="bg-dark rounded p-4">
                            {renderQuestionSection(testQuestions, "Тестовые вопросы", "Начальный уровень")}
                            {renderQuestionSection(openQuestions, "Открытые вопросы", "Средний уровень")}
                            {renderQuestionSection(practicalQuestions, "Практические задания", "Высокий уровень")}

                            <div className="d-flex justify-content-end mt-4">
                                {isSubmitted ? (
                                    <Button
                                        as="a"
                                        href={lessonPracticeDto.lessonType === LessonTypes.Exam ? '/course/result' : '/user/profile'}
                                        variant="success"
                                        className="px-4 py-2"
                                    >
                                        {lessonPracticeDto.lessonType === LessonTypes.Exam ? 'Посмотреть результаты' : 'Выйти'}
                                    </Button>
                                ) : (
                                    <Button
                                        type="submit"
                                        variant="primary"
                                        className="px-4 py-2"
                                        disabled={loading || isSubmitted}
                                    >
                                        {loading ? <Spinner animation="border" size="sm" /> : "Завершить"}
                                    </Button>
                                )}
                            </div>
                        </Form>
                    </Col>
                    <Col md={2} className="d-none d-md-block" />
                </Row>
            </Container>

            <Modal
                show={showLoadingModal}
                backdrop="static"
                keyboard={false}
                centered
                onHide={() => setShowLoadingModal(false)}
            >
                <Modal.Body className="text-center p-4">
                    <Spinner animation="border" variant="primary" className="mb-3" />
                    {lessonPracticeDto.lessonType === LessonTypes.Common ?
                     <h4 className="mt-2">Подождите пожалуйста</h4> :
                     <h4 className="mt-2">Подождите пожалуйста, идёт анализ от ChatGPT</h4>}
                </Modal.Body>
            </Modal>

            <Modal show={showDbSchemeModal} onHide={() => setShowDbSchemeModal(false)} size="xl">
                <Modal.Header closeButton className="bg-dark text-white">
                    <Modal.Title>Схема базы данных фильмов</Modal.Title>
                </Modal.Header>
                <Modal.Body className="p-0">
                    <FilmDbScheme />
                </Modal.Body>
                <Modal.Footer className="bg-dark">
                    <Button variant="secondary" onClick={() => setShowDbSchemeModal(false)}>
                        Закрыть
                    </Button>
                </Modal.Footer>
            </Modal>
        </Container>
    );
}