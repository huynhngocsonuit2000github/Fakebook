import React, { ReactNode, useState } from 'react'

interface ModalProps {
    isOpen: boolean;        // true: the modal will be show
    onClose: () => void;    // onClose: should set the isOpen to false from parent
    children: ReactNode;    // children: the content that included inside modal
}

export const Modal: React.FC<ModalProps> = ({ isOpen, onClose, children }) => {

    const handleModalContentClick = (event: React.MouseEvent<HTMLDivElement, MouseEvent>) => {
        event.stopPropagation(); // Prevents propagation to parent container
    };

    return (
        <div className={isOpen ? 'modal-open' : ''}>
            <div className={`modal fade fingerprint-modal ${isOpen ? 'show' : ''}`}
                id="fingerprintModal" tabIndex={-1} role="dialog" aria-labelledby="fingerprintModalLabel"
                style={{ display: isOpen ? 'block' : 'none' }}
                aria-modal={isOpen ? 'true' : 'false'}
                aria-hidden={isOpen ? 'false' : 'true'}
                onClick={onClose}
            >
                <div className="modal-dialog modal-dialog-centered" role="document" onClick={handleModalContentClick}>
                    <div className="modal-content">
                        <div className="modal-body text-center">
                            {children}
                        </div>
                    </div>
                </div>
            </div>

        </div>
    )
}
