import { Container, Row, Col } from "react-bootstrap";

export function Footer() {
    return (
        <footer className="bg-dark text-light py-4">
            <Container>
                <Row>
                    <Col className="text-center">
                        <p>&copy; 2025 CourseTech. All rights reserved.</p>
                    </Col>
                </Row>
                <Row>
                    <Col className="text-center">
                        <a href="/privacy" className="text-light">Privacy Policy</a> |
                        <a href="/terms" className="text-light"> Terms of Service</a>
                    </Col>
                </Row>
            </Container>
        </footer>
    );
}