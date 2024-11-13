import { AuthUser } from "../../models/AuthUser";

// Define all the case of this state
export const LOGIN_REQUEST = 'LOGIN_REQUEST';
export const LOGIN_SUCCESS = 'LOGIN_SUCCESS';
export const LOGIN_FAILURE = 'LOGIN_FAILURE';

export const LOG_OUT = 'LOG_OUT';

// Define the interface for the request and response
export interface AuthenticatedUser {
    id: string;
    firstName: string;
    lastName: string;
    avatar: string;
}

interface LoginRequest {
    type: typeof LOGIN_REQUEST;
    payload: {
        email: string;
        password: string;
    }
}

interface LoginSuccess {
    type: typeof LOGIN_SUCCESS;
    payload: {
        token: string;
    }
}

interface LoginFailure {
    type: typeof LOGIN_FAILURE;
    payload: {
        error: string;
    }
}

interface Logout {
    type: typeof LOG_OUT;
}

// Define the state model
export interface AccountState {
    user: AuthenticatedUser | null;
    loading: boolean;
    error: string | null;
    token: string | null;
    authUser: AuthUser | null;
}

// Define the enum for all of the const
export type AccountActionTypes =
    | LoginRequest
    | LoginSuccess
    | LoginFailure
    | Logout;