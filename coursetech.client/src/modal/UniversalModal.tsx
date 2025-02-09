import { Modal } from 'react-bootstrap';
import { useModal } from '../context/ModalContext';
import './UniversalModal.css';

interface UniversalModalProps {
    modalSize?: 'sm' | 'lg' | 'xl' | undefined
}
export function UniversalModal({ modalSize }: UniversalModalProps) {
    const { modal, close, modalContent, modalTitle } = useModal();

    return (
        <Modal show={modal} onHide={close} centered size={modalSize}>
            <Modal.Header closeButton>
                <Modal.Title className="w-100"><div className="text-center">{modalTitle}</div></Modal.Title>
            </Modal.Header>
            <Modal.Body>
                {modalContent}
            </Modal.Body>
        </Modal>
    );
}