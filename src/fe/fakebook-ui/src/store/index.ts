import { combineReducers } from 'redux';
import { accountReducer } from './account/reducers';
import { configureStore } from '@reduxjs/toolkit';

// Custom middleware for logging
const loggerMiddleware = (store: any) => (next: any) => (action: any) => {
    console.log('Dispatching action:', action);
    return next(action);
};

const anotherMiddleware = (store: any) => (next: any) => (action: any) => {
    console.log('Another middleware');

    // Custom logic here
    return next(action);
};

// Combine reducers
const rootReducer = combineReducers({
    account: accountReducer,
});

export type AppState = ReturnType<typeof rootReducer>;

export default function configureAppStore() {
    const devTools = window.__REDUX_DEVTOOLS_EXTENSION__ && window.__REDUX_DEVTOOLS_EXTENSION__();


    return configureStore({
        reducer: rootReducer,
        middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(loggerMiddleware, anotherMiddleware), // Add custom middleware
        devTools: devTools ? devTools : undefined,
    });
}