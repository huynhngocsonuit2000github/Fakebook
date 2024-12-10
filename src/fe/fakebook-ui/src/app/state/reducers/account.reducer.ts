// src/app/state/reducers/account.reducer.ts
import { createReducer, on } from '@ngrx/store';
import { loginRequest, loginFailure, logOut, beforeLoginSuccess, afterLoginSuccess } from '../actions/account.actions';
import { initialState } from '../account.state';

export const accountReducer = createReducer(
    initialState,
    on(loginRequest, (state) => {
        return {
            ...state,
            loading: true,
        }
    }),
    on(beforeLoginSuccess, (state, { token }) => {
        console.log('reducer', token);
        return {
            ...state,
            token,
            loading: true,
            error: null,
            authUser: {
                isAuthenticated: true,
                userPermissions: []
            }
        }
    }),
    on(afterLoginSuccess, (state, { permissions }) => {
        console.log('reducer', permissions);

        return {
            ...state,
            authUser: {
                isAuthenticated: true,
                userPermissions: [...permissions]
            }
        }
    }),
    on(loginFailure, (state, { error }) => {
        return {
            ...state,
            token: null,
            loading: false,
            error,
        }
    }),
    on(logOut, () => initialState)
);
