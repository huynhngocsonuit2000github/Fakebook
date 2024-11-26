// src/app/state/reducers/account.reducer.ts
import { createReducer, on } from '@ngrx/store';
import { loginRequest, loginSuccess, loginFailure, logOut } from '../actions/account.actions';
import { initialState } from '../account.state';

export const accountReducer = createReducer(
    initialState,
    on(loginRequest, (state) => {
        return {
            ...state,
            loading: true,
        }
    }),
    on(loginSuccess, (state, { token }) => {
        return {
            ...state,
            token,
            loading: false,
            error: null,
            authUser: {
                isAuthenticated: true,
                userPermissions: ['member_read', 'member_create', 'admin_read', 'admin_create'] // fake data
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
