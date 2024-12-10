import { createAction, props } from '@ngrx/store';

// Action Types
export const LOGIN_REQUEST = '[Account] Login Request';
export const BEFORE_LOGIN_SUCCESS = '[Account] Before Login Success';
export const AFTER_LOGIN_SUCCESS = '[Account] After Login Success';
export const LOGIN_FAILURE = '[Account] Login Failure';
export const LOG_OUT = '[Account] Logout';

// Action Definitions
export const loginRequest = createAction(
    LOGIN_REQUEST,
    props<{ username: string; password: string }>()
);

export const beforeLoginSuccess = createAction(
    BEFORE_LOGIN_SUCCESS,
    props<{ token: string }>()
);

export const afterLoginSuccess = createAction(
    AFTER_LOGIN_SUCCESS,
    props<{ permissions: string[] }>()
);

export const loginFailure = createAction(
    LOGIN_FAILURE,
    props<{ error: string }>()
);

export const logOut = createAction(LOG_OUT);
