import { combineReducers } from 'redux';
import { accountReducer } from './account/reducers';
import { configureStore } from '@reduxjs/toolkit';
import { persistReducer, persistStore } from 'redux-persist';
import storage from 'redux-persist/lib/storage'; // Using localStorage for persistence

// Custom middleware for logging
const loggerMiddleware = (store: any) => (next: any) => (action: any) => {
    return next(action);
};

const anotherMiddleware = (store: any) => (next: any) => (action: any) => {
    // Custom logic here
    return next(action);
};

// Combine reducers
const rootReducer = combineReducers({
    account: accountReducer,
});

// Redux Persist configuration
const persistConfig = {
    key: 'root', // Key for the persisted state
    storage, // You can use sessionStorage instead of localStorage if preferred
    whitelist: ['account'], // Only persist the 'account' reducer, for example
};

// Correctly type the store using persisted reducer
export type AppState = ReturnType<typeof rootReducer>;

// Create a persisted reducer
const persistedReducer = persistReducer(persistConfig, rootReducer as any);


export default function configureAppStore() {
    const devTools = window.__REDUX_DEVTOOLS_EXTENSION__ && window.__REDUX_DEVTOOLS_EXTENSION__();

    const store = configureStore({
        reducer: persistedReducer, // Use persistedReducer here
        middleware: (getDefaultMiddleware) =>
            getDefaultMiddleware().concat(loggerMiddleware, anotherMiddleware), // Add custom middleware
        devTools: devTools ? devTools : undefined,
    });

    const persistor = persistStore(store);

    return { store, persistor };
}
