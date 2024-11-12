import React from 'react';
import { Navigate, Route, RouteProps } from 'react-router-dom';
import { AuthUser } from '../../models/AuthUser';

interface PrivateRouteProps {
    requiredPermissions?: string[];
    element: JSX.Element;
}

const PrivateRoute: React.FC<PrivateRouteProps> = ({ requiredPermissions, element }) => {

    // get from redux in the future
    const auth: AuthUser = {
        isAuthenticated: false,
        userPermissions: ['admin_create', 'admin_read', 'member_read']
    };

    if (!auth.isAuthenticated) {
        return <Navigate to="/login" />;
    }

    if (requiredPermissions && !requiredPermissions.every(permission => auth.userPermissions.includes(permission))) {
        return <Navigate to="/" />;
    }

    return element;
};

export default PrivateRoute;
