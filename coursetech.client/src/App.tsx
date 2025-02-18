import { Route, Routes } from 'react-router-dom';
import './App.css';
import { ModalState } from './context/ModalContext';
import { MainLayout } from './layouts/MainLayout';
import { HomePage } from './pages/HomePage';
import { PassLessonPage } from './pages/PassLessonPage';
import { ReadLessonPage } from './pages/ReadLessonPage';
import { CourseResultPage } from './pages/CourseResultPage';
import { UserProfilePage } from './pages/UserProfilePage';


export function App() {
    return (
            <ModalState>
                <MainLayout>
                    <Routes>
                        <Route path="/" element={<HomePage />} />
                        <Route path="/lesson/pass/:lessonId" element={<PassLessonPage />} />
                        <Route path="/lesson/read/:lessonId" element={<ReadLessonPage />} />
                        <Route path="/course/result" element={<CourseResultPage />} />
                        <Route path="/user/profile" element={<UserProfilePage />} />
                    </Routes>
                </MainLayout>
            </ModalState>
    );
}