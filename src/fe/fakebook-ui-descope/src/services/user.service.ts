import env from 'react-dotenv'

const login = (username: string, password: string) => {
    console.log('execute login');

    const requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            username, password
        }),
    }

    return fetch(`${env.API_URL}/user/login`, requestOptions)
        .then(handleResponse)
        .then((response) => {
            sessionStorage.setItem('token', response);
            return response;
        })
}

const logout = () => {
    sessionStorage.removeItem('user');
}

const handleResponse = (response: any) => {
    console.log('res', response);

    return response.text().then((text: string) => {
        if (!response.ok) {
            if (response.status === 401) {
                logout();
            }

            const error = text || response.statusText;

            return Promise.reject(error);
        }
        return text;
    })

}

export const userService = {
    login,
    logout
}