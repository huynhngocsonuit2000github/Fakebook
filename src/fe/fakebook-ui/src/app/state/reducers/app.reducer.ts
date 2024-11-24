// src/app/state/reducers/app.reducer.ts
import { ActionReducerMap } from '@ngrx/store';
import { accountReducer, AccountState } from './account.reducer';

// Define the global app state interface
export interface AppState {
    account: AccountState;
    // another state
}

// Combine all reducers
export const appReducers: ActionReducerMap<AppState> = {
    account: accountReducer,
    // another reducer
};
