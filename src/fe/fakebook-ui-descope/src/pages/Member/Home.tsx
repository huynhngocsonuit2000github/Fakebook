import React from 'react'
import { useNavigate } from 'react-router';

export const Home = () => {

    const navigate = useNavigate();

    return (
        <div>
            <h2>
                Hello member home
            </h2>

            <button onClick={e => navigate('/friends')}>Friends </button>
            <button onClick={e => navigate('/admin')}>admin </button>
        </div>
    )
}
