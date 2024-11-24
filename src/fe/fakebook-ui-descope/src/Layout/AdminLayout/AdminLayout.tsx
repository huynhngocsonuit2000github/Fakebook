import { Outlet, RouteProps } from "react-router";
import React from 'react';

export const AdminLayout = ({children} : RouteProps) : JSX.Element => {
  return (
    <div className="AdminLayout" style={{background: 'red'}}>
      <h1>Admin Layout</h1>
      <Outlet />
    </div>
  );
};