import React from 'react';
import { Navigate } from 'react-router-dom';
import { AccountState } from '../../store/account/types';
import { useSelector } from 'react-redux';
import { AppState } from '../../store';

interface PrivateRouteProps {
    requiredPermissions?: string[];
    element: JSX.Element;
}

const PrivateRoute: React.FC<PrivateRouteProps> = ({ requiredPermissions, element }) => {
    const account: AccountState = useSelector((state: AppState) => state.account);
    const { authUser: auth } = account;

    if (!auth?.isAuthenticated) {
        return <Navigate to="/login" />;
    }

    if (requiredPermissions && !requiredPermissions.every(permission => auth.userPermissions.includes(permission))) {
        return <Navigate to="/" />;
    }

    return element;
};

export default PrivateRoute;
