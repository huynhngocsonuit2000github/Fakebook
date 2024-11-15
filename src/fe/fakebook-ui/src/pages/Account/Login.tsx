import React, { ChangeEvent, FormEvent, Fragment, useEffect, useRef, useState } from 'react'
import logo from './../../styles/template/images/logo-64x64.png'
import authFingerprint from './../../styles/template/images/icons/auth-fingerprint.png'
import { Modal } from '../../components/Modal/Modal';
import { AccountActionTypes, AccountState, LOG_OUT } from '../../store/account/types';
import { useSelector } from 'react-redux';
import { AppState } from '../../store';
import { Navigate, useLocation } from 'react-router';
import { useDispatch } from 'react-redux';
import { login, logout } from '../../store/account/actions';
import { ThunkDispatch } from 'redux-thunk';
import { AppDispatch } from '../..';
import { history } from '../../helpers';

export const Login = () => {

    const [isFingerprintOpen, setIsFingerprintOpen] = useState<boolean>(false);
    const account: AccountState = useSelector((state: AppState) => state.account);
    const { authUser: auth } = account;
    const [input, setInput] = useState({
        username: '',
        password: ''
    })
    const [submitted, setSubmitted] = useState<boolean>(false);
    const { username, password } = input;

    const dispatch = useDispatch<AppDispatch>();


    const location = useLocation();

    const loading = useSelector<AppState>(state => state.account.loading);


    const closeFingerprintModal = () => {
        setIsFingerprintOpen(false);
        // do sth
    }

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;

        setInput(inp => ({
            ...inp, [name]: value
        }));
    }

    const handleSubmit = (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        setSubmitted(true);

        if (username && password) {
            const { from } = location.state || { from: { pathname: '/' } };
            console.log(from);

            dispatch(login(username, password, from.pathname));
        }
    };

    useEffect(() => {
        dispatch(logout());
    }, [dispatch]);

    if (auth?.isAuthenticated) {
        return <Navigate to="/" />;
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
                        <form onSubmit={handleSubmit}>
                            <div className="row">
                                <div className="col-md-12">
                                    <div className="form-group">
                                        <input type="text" className={`form-control ${submitted && !username ? 'is-invalid' : ''}`} placeholder="Username" name='username' onChange={handleChange} />
                                        {
                                            submitted && !username && (
                                                <div className='invalid-feedback'> Username is required </div>
                                            )
                                        }
                                    </div>
                                </div>
                                <div className="col-md-12">
                                    <div className="form-group">
                                        <input type="password" className={`form-control ${submitted && !password ? 'is-invalid' : ''}`} placeholder="Password" name='password' onChange={handleChange} />
                                        {
                                            submitted && !password && (
                                                <div className='invalid-feedback'> Password is required </div>
                                            )
                                        }
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
                                        <button type="submit" className="btn btn-primary sign-up">Sign In</button>
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
