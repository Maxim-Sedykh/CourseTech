import React, { createContext, useState, useContext } from 'react';

interface IModalContext {
    modal: boolean;
    open: (content: React.ReactNode, title: string) => void;
    close: () => void;
    modalContent: React.ReactNode | null;
    modalTitle: string
}

export const ModalContext = createContext<IModalContext | undefined>(undefined);

export const ModalState: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [modal, setModal] = useState(false);
    const [modalContent, setModalContent] = useState<React.ReactNode | null>(null);
    const [modalTitle, setModalTitle] = useState('');
    const open = (content: React.ReactNode, title: string) => {
        setModalContent(content);
        setModalTitle(title);
        setModal(true);
    };

    const close = () => {
        setModalContent(null);
        setModalTitle('')
        setModal(false);
    };

    return (
        <ModalContext.Provider value={{ modal, open, close, modalContent, modalTitle }}>
            {children}
        </ModalContext.Provider>
    );
};

export const useModal = (): IModalContext => {
    const context = useContext(ModalContext);
    if (!context) {
        throw new Error('useModal must be used within a ModalProvider');
    }
    return context;
};