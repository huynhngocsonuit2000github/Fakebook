import { Outlet, RouteProps } from "react-router";
import React from 'react';

export const MemberLayout = ({children} : RouteProps) : JSX.Element => {
  return (
    <div className="MemberLayout" style={{background: 'blue'}}>
      <h1>Member Layout</h1>
      <Outlet />
    </div>
  );
};