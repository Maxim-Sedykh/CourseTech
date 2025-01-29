import { useState, useEffect } from 'react';
import reactLogo from './assets/react.svg';
import viteLogo from '/vite.svg';
import './App.css'; // Импортируем модель
import { LessonService } from './services/lesson-service';
import { LessonNameDto } from './types/dto/lesson/lesson-name-dto';
import { CollectionResult } from './types/result/collection-result';
import { ApiPaths } from './constants/api-paths';

function App() {
    const [count, setCount] = useState(0);
    const [lessonNames, setLessonNames] = useState<LessonNameDto[]>([]); // Указываем тип состояния
    const [error, setError] = useState<string | null>(null);

    const lessonService = new LessonService(ApiPaths.LESSON_API_PATH); // Укажите базовый URL

    useEffect(() => {
        const fetchLessonNames = async () => {
            try {
                const result: CollectionResult<LessonNameDto> = await lessonService.getLessonNames();

                if (result.isSuccess) {
                    setLessonNames(result.data); // Предполагаем, что структура ответа соответствует CollectionResult<LessonNameDto>
                } else {
                    console.log('error');
                }
            } catch (error) {
                console.error('Error fetching lesson names:', error);
                setError('Failed to fetch lesson names');
            }
        };

        fetchLessonNames();
    });

    return (
        <>
            <div>
                <a href="https://vite.dev" target="_blank" rel="noopener noreferrer">
                    <img src={viteLogo} className="logo" alt="Vite logo" />
                </a>
                <a href="https://react.dev" target="_blank" rel="noopener noreferrer">
                    <img src={reactLogo} className="logo react" alt="React logo" />
                </a>
            </div>
            <h1>Vite + React</h1>
            <div className="card">
                <button onClick={() => setCount((count) => count + 1)}>
                    count is {count}
                </button>
                <p>
                    Edit <code>src/App.tsx</code> and save to test HMR
                </p>
            </div>
            <p className="read-the-docs">
                Click on the Vite and React logos to learn more
            </p>
            <div>
                <h2>Lesson Names:</h2>
                {error && <p style={{ color: 'red' }}>Error: {error}</p>}
                <ul>
                    {lessonNames.length > 0 ? (
                        lessonNames.map((lesson) => (
                            <li>{lesson.name}</li> // Используем id для ключа
                        ))
                    ) : (
                        <li>Loading...</li>
                    )}
                </ul>
            </div>
        </>
    );
}

export default App;