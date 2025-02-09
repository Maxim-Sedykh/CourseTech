import { Footer } from "../components/Footer";
import { Header } from "../components/Header";

export interface MainLayoutProps {
    children: React.ReactNode;
}

export function MainLayout(props: MainLayoutProps) {
    return (
        <div className="main-layout d-flex flex-column min-vh-100">
            <Header />
            <main className='main-layout-content'>
                {props.children}
            </main>
            <Footer />
        </div>
    );
};