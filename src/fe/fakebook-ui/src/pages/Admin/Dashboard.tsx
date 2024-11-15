import React from 'react'
import { useNavigate } from 'react-router';

export const Dashboard = () => {
    const navigate = useNavigate();
    return (
        <div>
            <h2>
                Hello admin Dashboard
            </h2>

            <button onClick={e => navigate('/admin/settings')}>Settings </button>
            <button onClick={e => navigate('/')}>Home </button>
        </div>
    )
}
