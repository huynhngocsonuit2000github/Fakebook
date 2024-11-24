// src/app/state/reducers/account.reducer.ts
import { createReducer, on } from '@ngrx/store';
import { loginRequest, loginSuccess, loginFailure, logOut } from '../actions/account.actions';

export interface AccountState {
    token: string | null;
    loading: boolean;
    error: string | null;
}

export const initialState: AccountState = {
    token: null,
    loading: false,
    error: null,
};

export const accountReducer = createReducer(
    initialState,
    on(loginRequest, (state) => {
        console.log('Reducer: loginRequest');

        return {
            ...state,
            loading: true,
        }
    }),
    on(loginSuccess, (state, { token }) => {
        console.log('Reducer: loginSuccess');
        return {
            ...state,
            token,
            loading: false,
            error: null,
        }
    }),
    on(loginFailure, (state, { error }) => {
        console.log('Reducer: loginFailure');
        return {
            ...state,
            token: null,
            loading: false,
            error,
        }
    }),
    on(logOut, () => initialState)
);
