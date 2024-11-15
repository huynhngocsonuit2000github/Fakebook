import { Dispatch } from 'react';
import { AccountActionTypes, LOG_OUT, LOGIN_FAILURE, LOGIN_REQUEST, LOGIN_SUCCESS } from './types';
import { userService } from '../../services/user.service';
import { history } from '../../helpers';

export const login = (username: string, password: string, from: string) => {
    return (dispatch: Dispatch<AccountActionTypes>) => {
        console.log('LOGIN_REQUEST');

        dispatch({
            type: LOGIN_REQUEST,
            payload: {
                username: username,
                password: password
            }
        })

        userService.login(username, password).then(res => {
            console.log('LOGIN_SUCCESS', res);
            dispatch({
                type: LOGIN_SUCCESS,
                payload: res
            })

            console.log('come to ' + from);

            history.push(from);
        }, (error) => {
            console.log('LOGIN_FAILURE');
            dispatch({
                type: LOGIN_FAILURE,
                payload: error
            })
        })
    }
}

export const logout = (): AccountActionTypes => {
    return {
        type: LOG_OUT
    }
}