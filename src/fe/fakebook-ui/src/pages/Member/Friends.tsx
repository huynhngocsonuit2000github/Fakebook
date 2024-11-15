import React from 'react'
import { useNavigate } from 'react-router';

export const Friends = () => {
    const navigate = useNavigate();
    return (
        <div>
            <h2>
                Hello member friends
            </h2>

            <button onClick={e => navigate('/')}>Home </button>
            <button onClick={e => navigate('/admin')}>admin </button>
        </div>
    )
}
