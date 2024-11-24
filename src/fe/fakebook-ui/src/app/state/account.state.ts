import { AuthenticatedUser } from "../../models/AuthenticatedUser.model";
import { AuthUser } from "../../models/AuthUser";

export interface AccountState {
    user: AuthenticatedUser | null;
    loading: boolean;
    error: string | null;
    token: string | null;
    authUser: AuthUser | null;
}

export const initialState: AccountState = {
    user: null,
    loading: false,
    error: null,
    token: null,
    authUser: {
        isAuthenticated: false,
        userPermissions: [],
    },
};
