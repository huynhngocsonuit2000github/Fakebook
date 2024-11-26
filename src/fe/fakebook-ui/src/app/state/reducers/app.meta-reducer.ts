import { ActionReducer, Action } from '@ngrx/store';
import { AppState } from '../reducers/app.reducer';

export function localStorageMetaReducer(reducer: ActionReducer<any>): ActionReducer<any> {
    return function (state: any, action: Action): any {
        // Get the current state from localStorage if available
        let currentState = state;

        // Load persisted state from localStorage
        const persistedState = localStorage.getItem('appState');

        if (persistedState && !state) {
            // If there's no state and persisted state exists, set it
            currentState = JSON.parse(persistedState);

        }

        // Call the actual reducer with the current state
        const nextState = reducer(currentState, action);

        // Persist the updated state to localStorage
        localStorage.setItem('appState', JSON.stringify(nextState));

        return nextState;
    };
}
