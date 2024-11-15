import { AccountActionTypes, AccountState, LOG_OUT, LOGIN_FAILURE, LOGIN_REQUEST, LOGIN_SUCCESS } from "./types"; // import the state model

// Define initial state
const initialState: AccountState = {
    user: null,
    loading: false,
    error: null,
    token: null,
    authUser: {
        isAuthenticated: false,
        userPermissions: []
    }
};

const accountReducer = (
    state: AccountState = initialState,
    action: AccountActionTypes
): AccountState => {
    if (state === undefined) {
        return initialState; // Return the default state if state is undefined
    }

    switch (action.type) {
        case LOGIN_REQUEST:
            return {
                ...state,
                loading: true
            };

        case LOGIN_SUCCESS:
            return {
                ...state,
                loading: false,
                token: action.payload.token,
                error: null,
                authUser: {
                    isAuthenticated: true,
                    userPermissions: ['member_read', 'member_create', 'admin_read', 'admin_create'] // fake data
                }
            };

        case LOGIN_FAILURE:
            return {
                ...state,
                loading: false,
                token: null,
                error: action.payload.error
            };

        case LOG_OUT:
            return {
                ...initialState
            }
        default:
            return state;
    }
}

export { accountReducer };

