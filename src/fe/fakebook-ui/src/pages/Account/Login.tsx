import React, { Fragment, useEffect, useRef, useState } from 'react'
import logo from './../../styles/template/images/logo-64x64.png'
import authFingerprint from './../../styles/template/images/icons/auth-fingerprint.png'
import { Modal } from '../../components/Modal/Modal';

export const Login = () => {
    const [isFingerprintOpen, setIsFingerprintOpen] = useState<boolean>(false);

    const closeFingerprintModal = () => {
        setIsFingerprintOpen(false);
        // do sth
    }

    return (
        <Fragment >
            <div className="row ht-100v flex-row-reverse no-gutters">
                <div className="col-md-6 d-flex justify-content-center align-items-center">
                    <div className="signup-form">
                        <div className="auth-logo text-center mb-5">
                            <div className="row">
                                <div className="col-md-2">
                                    <img src={logo} className="logo-img" alt="Logo" />
                                </div>
                                <div className="col-md-10">
                                    <p>Fakebook Social Network</p>
                                    <span>Design System</span>
                                </div>
                            </div>
                        </div>
                        <form>
                            <div className="row">
                                <div className="col-md-12">
                                    <div className="form-group">
                                        <input type="text" className="form-control" placeholder="Email Address" />
                                    </div>
                                </div>
                                <div className="col-md-12">
                                    <div className="form-group">
                                        <input type="password" className="form-control" placeholder="Password" />
                                    </div>
                                </div>
                                <div className="col-md-12 mb-3">
                                    <a href="forgot-password.html">Forgot password?</a>
                                </div>
                                <div className="col-md-6">
                                    <label className="custom-control material-checkbox">
                                        <input type="checkbox" className="material-control-input" />
                                        <span className="material-control-indicator" />
                                        <span className="material-control-description">Remember Me</span>
                                    </label>
                                </div>
                                <div className="col-md-6 text-right">
                                    <div className="form-group">
                                        <button type="button" className="btn btn-primary sign-up">Sign In</button>
                                    </div>
                                </div>
                                <div className="col-md-12 text-center mt-4">
                                    <p className="text-muted">Start using your fingerprint</p>
                                    <a href="#" className="btn btn-outline-primary btn-sm sign-up" data-toggle="modal" data-target="#fingerprintModal" onClick={e => setIsFingerprintOpen(true)}>Use Fingerprint</a>
                                </div>
                                <div className="col-md-12 text-center mt-5">
                                    <span className="go-login">Not yet a member? <a href="sign-up.html">Sign Up</a></span>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
                <div className="col-md-6 auth-bg-image d-flex justify-content-center align-items-center">
                    <div className="auth-left-content mt-5 mb-5 text-center">
                        <div className="weather-small text-white">
                            <p className="current-weather"><i className="bx bx-sun" /> <span>35°</span></p>
                            <p className="weather-city">HCM - Viet Nam</p>
                        </div>
                        <div className="text-white mt-5 mb-5">
                            <h2 className="create-account mb-3">Fakebook Application</h2>
                            <p>Thank you for joining. Updates and new features are released daily.</p>
                        </div>
                        <div className="auth-quick-links">
                            <a href="#" className="btn btn-outline-primary">Hello mấy cưng</a>
                        </div>
                    </div>
                </div>
            </div>

            {/* Fingerprint Modal */}
            <Modal isOpen={isFingerprintOpen} onClose={() => closeFingerprintModal()}>
                <h3 className="text-muted display-5">Place your Finger on the Device Now</h3>
                <img src={authFingerprint} alt="Fingerprint" />
            </Modal>

        </Fragment>
    )
}
