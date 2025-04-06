import { useState, useEffect, useCallback } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { ApiPaths } from "../constants/api-paths";
import { QuestionService } from "../services/question-service";
import { LessonPracticeDto } from "../types/dto/question/lesson-practice-dto";
import { Container, Row, Col, Alert, Button, FormCheck, FormControl, Form, Modal, Spinner } from "react-bootstrap";
import { LessonTypes } from "../enums/lesson-types";
import { PracticeUserAnswersDto } from "../types/dto/question/practice-user-answer-dto";
import { TestQuestionDto } from "../types/dto/question/question-dto";
import { IQuestionDto } from "../types/dto/question/question-dto-interface";
import { PracticeCorrectAnswersDto } from "../types/dto/question/practice-correct-answer-dto";
import { OpenQuestionUserAnswerDto, PracticalQuestionUserAnswerDto, TestQuestionUserAnswerDto } from "../types/dto/question/user-answer-dto";
import { PracticalQuestionCorrectAnswerDto } from "../types/dto/question/correct-answer-dto";
import { FilmDbScheme } from "../components/LearningProcess/FilmDbScheme";
import { GraphVisualizer } from "../components/Graph/GraphVisualizer";

function isTestQuestionDto(question: IQuestionDto): question is TestQuestionDto {
    return (question as TestQuestionDto).testVariants !== undefined;
}

const questionService = new QuestionService(ApiPaths.QUESTION_API_PATH);

export function PassLessonPage() {
    const navigate = useNavigate();
    const { lessonId } = useParams<{ lessonId: string }>();
    const lessonIdNumber = Number(lessonId);

    const [formData, setFormData] = useState<PracticeUserAnswersDto>({
        lessonId: lessonIdNumber,
        userAnswerDtos: []
    });

    const [lessonPracticeDto, setLessonPracticeDto] = useState<LessonPracticeDto>({
        lessonId: lessonIdNumber,
        lessonType: LessonTypes.Common,
        questions: []
    });

    const [practiceCorrectAnswersDto, setPracticeCorrectAnswersDto] = useState<PracticeCorrectAnswersDto>({
        lessonId: lessonIdNumber,
        questionCorrectAnswers: []
    });

    const [loading, setLoading] = useState(true);
    const [isSubmitted, setIsSubmitted] = useState(false);
    const [showLoadingModal, setShowLoadingModal] = useState(false);

    const lessonContainerColor = lessonPracticeDto.lessonType === LessonTypes.Exam ? "bd-indigo-800" : "bd-blue-700";

    const handleTestQuestionChange = useCallback((questionIndex: number, variantNumber: number) => {
        setFormData(prev => {
            const question = lessonPracticeDto.questions[questionIndex];
            if (!question) return prev;

            const existingIndex = prev.userAnswerDtos.findIndex(a => a.questionId === question.id);
            const newAnswer: TestQuestionUserAnswerDto = {
                questionId: question.id,
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
    }, [lessonPracticeDto.questions]);

    const handleOpenQuestionChange = useCallback((questionIndex: number, value: string) => {
        setFormData(prev => {
            const question = lessonPracticeDto.questions[questionIndex];
            if (!question) return prev;

            const existingIndex = prev.userAnswerDtos.findIndex(a => a.questionId === question.id);
            const newAnswer: OpenQuestionUserAnswerDto = {
                questionId: question.id,
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
    }, [lessonPracticeDto.questions]);

    const handlePracticalQuestionChange = useCallback((questionIndex: number, value: string) => {
        setFormData(prev => {
            const question = lessonPracticeDto.questions[questionIndex];
            if (!question) return prev;

            const existingIndex = prev.userAnswerDtos.findIndex(a => a.questionId === question.id);
            const newAnswer: PracticalQuestionUserAnswerDto = {
                questionId: question.id,
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
    }, [lessonPracticeDto.questions]);

    const getLevelTitle = useCallback((questionIndex: number) => {
        if (!lessonPracticeDto || questionIndex === 0) return null;

        const question = lessonPracticeDto.questions[questionIndex];
        const previousQuestion = lessonPracticeDto.questions[questionIndex - 1];

        if (question.questionType === 'OpenQuestionDto' && previousQuestion.questionType === 'TestQuestionDto') {
            return <p className="text-white mt-4 fs-4 text-center">Средний уровень</p>;
        }

        if (question.questionType === 'PracticalQuestionDto' && previousQuestion.questionType === 'OpenQuestionDto') {
            return (
                <>
                    <p className="text-white mt-4 fs-4 text-center">Высокий уровень</p>
                    <FilmDbScheme />
                </>
            );
        }

        return null;
    }, [lessonPracticeDto]);

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
        setShowLoadingModal(true);

        try {
            const sortedAnswers = [...formData.userAnswerDtos].sort((x, y) => x.questionId - y.questionId);
            const response = await questionService.passLessonQuestions({ 
                ...formData, 
                userAnswerDtos: sortedAnswers 
            });


            setPracticeCorrectAnswersDto(response.data as PracticeCorrectAnswersDto);
            
            setIsSubmitted(true);

            if (lessonPracticeDto?.lessonType === LessonTypes.Exam) {
                navigate('/course/result');
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
                const response = await questionService.getLessonQuestions(lessonIdNumber);
                setLessonPracticeDto(response.data as LessonPracticeDto);
            } catch (error) {
                console.error("Ошибка при загрузке вопросов:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchLesson();
    }, [lessonIdNumber]);

    return (
        <Container fluid>
            <Container>
                <Row>
                    <Col md={2} className="hidden-sm" />
                    <Col md={8} sm={12} className={`${lessonContainerColor} my-5 br-40 px-4`}>
                        {lessonPracticeDto.lessonType === LessonTypes.Exam ? (
                            <p className="text-center text-white text-uppercase fs-2 mt-3">
                                Э К З А М Е Н
                            </p>
                        ) : (
                            <p className="text-center text-white fs-2 mt-3">
                                Практическая часть занятия
                            </p>
                        )}

                        <Form autoComplete="off" onSubmit={handleSubmit}>
                            <input type="hidden" value={lessonPracticeDto.lessonId} />
                            <input type="hidden" value={lessonPracticeDto.lessonType} />

                            <p className="text-white mt-4 fs-4 text-center">Начальный уровень</p>

                            {lessonPracticeDto.questions.map((question, index) => {
                                const userAnswer = formData.userAnswerDtos.find(a => a.questionId === question.id);
                                const correctAnswer = practiceCorrectAnswersDto.questionCorrectAnswers?.[index];

                                return (
                                    <div key={question.id}>
                                        {getLevelTitle(index)}
                                        <p>
                                            <b>{question.number}</b>. {question.displayQuestion}
                                        </p>

                                        {correctAnswer?.answerCorrectness === true && (
                                            <Alert variant="success" className="text-center">
                                                Правильно!
                                            </Alert>
                                        )}

                                        {correctAnswer?.answerCorrectness === false && (
                                            <>
                                                <Alert variant="danger" className="text-center">
                                                    Неправильно! Правильный ответ: {correctAnswer.correctAnswer}
                                                </Alert>

                                                {correctAnswer.questionType === 'PracticalQuestionCorrectAnswerDto' && (
                                                    <div className="mt-3">
                                                        <h5 className="text-center">Анализ искусственного интеллекта:</h5>
                                                        <GraphVisualizer
                                                            data={(correctAnswer as PracticalQuestionCorrectAnswerDto)?.chatGptAnalysis}
                                                        />
                                                        {(correctAnswer as PracticalQuestionCorrectAnswerDto)?.chatGptAnalysis.UserQueryAnalys && (
                                                            <Alert variant="info" className="mt-3">
                                                                <Alert.Heading>Анализ запроса:</Alert.Heading>
                                                                <p>{(correctAnswer as PracticalQuestionCorrectAnswerDto)?.chatGptAnalysis.UserQueryAnalys}</p>
                                                            </Alert>
                                                        )}
                                                    </div>
                                                )}
                                            </>
                                        )}

                                        {isTestQuestionDto(question) ? (
                                            question.testVariants?.map(variant => (
                                                <div key={variant.variantNumber}>
                                                    <FormCheck
                                                        type="radio"
                                                        id={`input-pointer-${variant.variantNumber}-${index}`}
                                                        label={variant.content}
                                                        name={`question-${index}`}
                                                        value={`v_${variant.variantNumber}`}
                                                        required
                                                        checked={(userAnswer as TestQuestionUserAnswerDto)?.userAnswerNumberOfVariant === variant.variantNumber}
                                                        onChange={() => handleTestQuestionChange(index, variant.variantNumber)}
                                                    />
                                                </div>
                                            ))
                                        ) : (
                                            <FormControl
                                                as={question.questionType === 'PracticalQuestionDto' ? "textarea" : "input"}
                                                className="br-40 w-75"
                                                placeholder="Ответ..."
                                                required
                                                value={
                                                    (userAnswer as OpenQuestionUserAnswerDto)?.userAnswer ||
                                                    (userAnswer as PracticalQuestionUserAnswerDto)?.userCodeAnswer || ""
                                                }
                                                onChange={e => {
                                                    if (question.questionType === 'PracticalQuestionDto') {
                                                        handlePracticalQuestionChange(index, e.target.value);
                                                    } else {
                                                        handleOpenQuestionChange(index, e.target.value);
                                                    }
                                                }}
                                            />
                                        )}
                                    </div>
                                );
                            })}

                            <div className="text-center">
                                {isSubmitted ? (
                                    <Button
                                        as="a"
                                        href={lessonPracticeDto.lessonType === LessonTypes.Exam ? '/course/result' : '/user/profile'}
                                        className="br-40 bd-green-600 mt-2 text-white fs-5 mb-4"
                                        disabled={loading}
                                    >
                                        {loading ? "Отправка..." : "Выйти"}
                                    </Button>
                                ) : (
                                    <Button
                                        type="submit"
                                        className="br-40 bd-green-600 mt-2 text-white fs-5 mb-4"
                                        disabled={loading || isSubmitted}
                                    >
                                        {loading ? "Отправка..." : "Завершить"}
                                    </Button>
                                )}
                            </div>
                        </Form>
                    </Col>
                    <Col md={2} className="hidden-sm" />
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
                    <Spinner animation="border" variant="primary" className="mb-3" size="sm" />
                    <h4 className="mt-2">Подождите пожалуйста, идёт анализ от ChatGPT</h4>
                </Modal.Body>
            </Modal>
        </Container>
    );
}