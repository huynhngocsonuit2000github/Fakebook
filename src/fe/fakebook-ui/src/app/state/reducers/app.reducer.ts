// src/app/state/reducers/app.reducer.ts
import { ActionReducerMap } from '@ngrx/store';
import { accountReducer } from './account.reducer';
import { AccountState } from '../account.state';
import { localStorageMetaReducer } from './app.meta-reducer';


// Define the global app state interface
export interface AppState {
    account: AccountState;
    // another state
}

// Combine all reducers
export const appReducers: ActionReducerMap<AppState> = {
    account: localStorageMetaReducer(accountReducer), // Use accountReducer directly here
    // Add more reducers for other slices of the AppState
};