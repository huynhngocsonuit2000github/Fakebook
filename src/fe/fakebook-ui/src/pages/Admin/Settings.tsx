import React from 'react'
import { useNavigate } from 'react-router';

export const Settings = () => {
    const navigate = useNavigate();
    return (
        <div>
            <h2>
                Hello admin settings
            </h2>

            <button onClick={e => navigate('/admin')}>Dashboard </button>
            <button onClick={e => navigate('/')}>Home </button>
        </div>
    )
}
