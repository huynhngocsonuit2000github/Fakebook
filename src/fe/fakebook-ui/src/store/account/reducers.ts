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
                error: null
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

