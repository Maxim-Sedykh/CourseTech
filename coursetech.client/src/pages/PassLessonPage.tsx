import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { ApiPaths } from "../constants/api-paths";
import { QuestionService } from "../services/question-service";
import { LessonPracticeDto } from "../types/dto/question/lesson-practice-dto";
import { Container, Row, Col, Alert, Button, FormCheck, FormControl, Form } from "react-bootstrap";
import { LessonTypes } from "../enums/lesson-types";
import { PracticeUserAnswersDto } from "../types/dto/question/practice-user-answer-dto";
import { TestQuestionDto } from "../types/dto/question/question-dto";
import { IQuestionDto } from "../types/dto/question/question-dto-interface";
import { PracticeCorrectAnswersDto } from "../types/dto/question/practice-correct-answer-dto";
import { IUserAnswerDto, OpenQuestionUserAnswerDto, PracticalQuestionUserAnswerDto, TestQuestionUserAnswerDto } from "../types/dto/question/user-answer-dto";
import { PracticalQuestionCorrectAnswerDto } from "../types/dto/question/correct-answer-dto";
import { FilmDbScheme } from "../components/LearningProcess/FilmDbScheme";
import { useNavigate } from 'react-router-dom';


function isTestQuestionDto(question: IQuestionDto): question is TestQuestionDto {
    return (question as TestQuestionDto).testVariants !== undefined;
}

export function PassLessonPage() {

    const navigate = useNavigate();


    const handleTestQuestionChange = (questionIndex: number, variantNumber: number) => {
        setFormData((prevFormData) => {
            const question = lessonPracticeDto.questions[questionIndex];

            if (!question) {
                console.error(`Question not found at index: ${questionIndex}`);
                return prevFormData;
            }

            const existingAnswerIndex = prevFormData.userAnswerDtos.findIndex(answer => answer.questionId === question.id);

            let updatedUserAnswerDtos: IUserAnswerDto[];

            if (existingAnswerIndex !== -1) {
                // Update existing answer
                updatedUserAnswerDtos = prevFormData.userAnswerDtos.map((answer, index) => {
                    if (index === existingAnswerIndex) {
                        return {
                            questionId: question.id,
                            userAnswerNumberOfVariant: variantNumber,
                            questionType: 'TestQuestionUserAnswerDto'
                        } as TestQuestionUserAnswerDto;
                    }
                    return answer;
                });
            } else {
                // Add new answer
                updatedUserAnswerDtos = [...prevFormData.userAnswerDtos, {
                    questionId: question.id,
                    userAnswerNumberOfVariant: variantNumber,
                    questionType: 'TestQuestionUserAnswerDto'
                } as TestQuestionUserAnswerDto];
            }

            return {
                ...prevFormData,
                userAnswerDtos: updatedUserAnswerDtos
            };
        });
    };

    const handleOpenQuestionChange = (questionIndex: number, value: string) => {
        setFormData((prevFormData) => {
            const question = lessonPracticeDto.questions[questionIndex];

            if (!question) {
                console.error(`Question not found at index: ${questionIndex}`);
                return prevFormData;
            }

            const existingAnswerIndex = prevFormData.userAnswerDtos.findIndex(answer => answer.questionId === question.id);

            let updatedUserAnswerDtos: IUserAnswerDto[];

            if (existingAnswerIndex !== -1) {
                // Update existing answer
                updatedUserAnswerDtos = prevFormData.userAnswerDtos.map((answer, index) => {
                    if (index === existingAnswerIndex) {
                        return {
                            questionId: question.id,
                            userAnswer: value,
                            questionType: 'OpenQuestionUserAnswerDto'
                        } as OpenQuestionUserAnswerDto;
                    }
                    return answer;
                });
            } else {
                // Add new answer
                updatedUserAnswerDtos = [...prevFormData.userAnswerDtos, {
                    questionId: question.id,
                    userAnswer: value,
                    questionType: 'OpenQuestionUserAnswerDto'
                } as OpenQuestionUserAnswerDto];
            }

            return {
                ...prevFormData,
                userAnswerDtos: updatedUserAnswerDtos
            };
        });
    };

    const handlePracticalQuestionChange = (questionIndex: number, value: string) => {
        setFormData((prevFormData) => {
            const question = lessonPracticeDto.questions[questionIndex];

            if (!question) {
                console.error(`Question not found at index: ${questionIndex}`);
                return prevFormData;
            }

            const existingAnswerIndex = prevFormData.userAnswerDtos.findIndex(answer => answer.questionId === question.id);

            let updatedUserAnswerDtos: IUserAnswerDto[];

            if (existingAnswerIndex !== -1) {
                // Update existing answer
                updatedUserAnswerDtos = prevFormData.userAnswerDtos.map((answer, index) => {
                    if (index === existingAnswerIndex) {
                        return {
                            questionId: question.id,
                            userCodeAnswer: value,
                            questionType: 'PracticalQuestionUserAnswerDto'
                        } as PracticalQuestionUserAnswerDto;
                    }
                    return answer;
                });
            } else {
                // Add new answer
                updatedUserAnswerDtos = [...prevFormData.userAnswerDtos, {
                    questionId: question.id,
                    userCodeAnswer: value,
                    questionType: 'PracticalQuestionUserAnswerDto'
                } as PracticalQuestionUserAnswerDto];
            }

            return {
                ...prevFormData,
                userAnswerDtos: updatedUserAnswerDtos
            };
        });
    };





    const { lessonId } = useParams<{ lessonId: string }>(); // Получаем параметр
    const lessonIdNumber = Number(lessonId);

    const getLevelTitle = (questionIndex: number) => {
        if (!lessonPracticeDto) return null;

        const question = lessonPracticeDto.questions[questionIndex];

        if (questionIndex > 0) {
            const previousQuestion = lessonPracticeDto.questions[questionIndex - 1];

            if (
                question.questionType === 'OpenQuestionDto' &&
                previousQuestion.questionType === 'TestQuestionDto'
            ) {
                return <p className="text-white mt-4 fs-4 text-center">Средний уровень</p>;
            }

            if (
                question.questionType === 'PracticalQuestionDto' &&
                previousQuestion.questionType === 'OpenQuestionDto'
            ) {
                return (
                <>
                    <p className="text-white mt-4 fs-4 text-center">Высокий уровень</p>
                    <FilmDbScheme/>
                </>)
            }
        }
        return null;
    };


    const [formData, setFormData] = useState<PracticeUserAnswersDto>({
        lessonId: lessonIdNumber,
        userAnswerDtos: []
    });

    const initialLessonPracticeDto: LessonPracticeDto = {
        lessonId: lessonIdNumber,
        lessonType: LessonTypes.Common,
        questions: []
    };

    const initialPracticeCorrectAnswersDto: PracticeCorrectAnswersDto = {
        lessonId: lessonIdNumber,
        questionCorrectAnswers: []
    };


    const [lessonPracticeDto, setLessonPracticeDto] = useState<LessonPracticeDto>(initialLessonPracticeDto);
    const [practiceCorrectAnswersDto, setPracticeCorrectAnswersDto] = useState<PracticeCorrectAnswersDto>(initialPracticeCorrectAnswersDto);
    const [loading, setLoading] = useState(true);
    const [isSubmitted, setIsSubmitted] = useState(false);
    const questionService = new QuestionService(ApiPaths.QUESTION_API_PATH); // Укажите ваш базовый URL
    
    const lessonContainerColor =
        lessonPracticeDto.lessonType === LessonTypes.Exam ? "bd-indigo-800" : "bd-blue-700";


    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();

        formData.userAnswerDtos.sort((x, y) => x.questionId - y.questionId);

        const response = await questionService.passLessonQuestions(formData);
        const practiceCorrectAnswersDto = response.data as PracticeCorrectAnswersDto

        setPracticeCorrectAnswersDto(practiceCorrectAnswersDto);
        setIsSubmitted(true);

        // if exam - redirect to FinalResult page
        if (lessonPracticeDto?.lessonType === LessonTypes.Exam) {
            navigate('/course/result');
        }
        else {
            return;
        }
    };

    useEffect(() => {
        const fetchLesson = async () => {
            try {
                const response = await questionService.getLessonQuestions(lessonIdNumber);
                const data = response.data as LessonPracticeDto;

                setLessonPracticeDto(data);
            } catch (error) {
                console.error("Ошибка при загрузке вопросов:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchLesson();
    }, [lessonIdNumber]);

    return (<>
        <Container fluid>
            <Container>
                <Row>
                    <Col md={2} className="hidden-sm" />
                    <Col md={8} sm={12} className={`${lessonContainerColor} my-5 br-40 px-4`}>
                        {lessonPracticeDto.lessonType === LessonTypes.Exam ? (
                            <p className="text-center text-white text-uppercase fs-2 mt-3">
                                э к з а м е н
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

                                // Find user answer for this question
                                const userAnswer = formData.userAnswerDtos.find(answer => answer.questionId === question.id);

                                return (
                                    <div key={question.id}>
                                        {getLevelTitle(index)}
                                        <p>
                                            <b>{question.number}</b>. {question.displayQuestion}
                                        </p>

                                        {practiceCorrectAnswersDto.questionCorrectAnswers && practiceCorrectAnswersDto.questionCorrectAnswers[index]?.answerCorrectness === true && (
                                            <Alert variant="success" className="text-center">
                                                Правильно!
                                            </Alert>
                                        )}
                                        {practiceCorrectAnswersDto.questionCorrectAnswers && practiceCorrectAnswersDto.questionCorrectAnswers[index]?.answerCorrectness === false && (
                                            <Alert variant="danger" className="text-center">
                                                Неправильно! Правильный ответ: {practiceCorrectAnswersDto.questionCorrectAnswers[index]?.correctAnswer}
                                            </Alert>
                                        )}
                                        {((isSubmitted) && (practiceCorrectAnswersDto.questionCorrectAnswers[index] as PracticalQuestionCorrectAnswerDto).userQueryAnalys) && (
                                            <Alert variant="danger" className="text-center">
                                                Анализ по вашему запросу от искусственного интеллекта: 
                                                { (practiceCorrectAnswersDto.questionCorrectAnswers[index] as PracticalQuestionCorrectAnswerDto).userQueryAnalys}
                                            </Alert>
                                        )}

                                        {isTestQuestionDto(question) ? (
                                            question.testVariants?.map((variant) => (
                                                <div key={variant.variantNumber}>
                                                    <FormCheck
                                                        type="radio"
                                                        id={`input-pointer-${variant.variantNumber}-${index}`}
                                                        label={variant.content}
                                                        name={`question-${index}`}
                                                        value={`v_${variant.variantNumber}`}
                                                        required
                                                        checked={userAnswer && (userAnswer as TestQuestionUserAnswerDto).userAnswerNumberOfVariant === variant.variantNumber}
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
                                                value={(userAnswer as OpenQuestionUserAnswerDto)?.userAnswer || (userAnswer as PracticalQuestionUserAnswerDto)?.userCodeAnswer || ""}
                                                onChange={(e) => {
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
                                        href={lessonPracticeDto.lessonType === LessonTypes.Exam ? '/course/result': '/user/profile'}
                                        className="br-40 bd-green-600 mt-2 text-white fs-5 mb-4"
                                        disabled={loading}
                                    >
                                        {loading ? "Отправка..." : "Выйти"}
                                    </Button>
                                ) : (
                                    <Button
                                        type="submit"
                                        className="br-40 bd-green-600 mt-2 text-white fs-5 mb-4"
                                        disabled={loading}
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
        </Container>
    </>);
};