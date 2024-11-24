// src/app/state/effects/account.effects.ts
import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Router } from '@angular/router';
import {
    loginRequest,
    loginSuccess,
    loginFailure,
    logOut,
} from '../actions/account.actions';
import { catchError, map, mergeMap, tap } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { UserService } from '../../../services/user.service';

@Injectable()
export class AccountEffects {
    constructor(
        private actions$: Actions,
        private userService: UserService,
        private router: Router
    ) { }

    // Effect for login
    login$ = createEffect(() =>
        defer(() => {
            return this.actions$.pipe(
                ofType(loginRequest),
                tap(() => console.log('Effect: Action of type loginRequest detected')),
                mergeMap(({ username, password }) =>
                    this.userService.login(username, password).pipe(
                        map((token) => {
                            console.log('Effect: Login success:', token);
                            return loginSuccess({ token });
                        }),
                        catchError((error) => {
                            console.error('Effect: Login failure:', error);
                            return of(loginFailure({ error }));
                        })
                    )
                )
            );
        })
    );

    loginRedirect$ = createEffect(
        () =>
            defer(() => {
                console.log('Effect: Initializing loginRedirect effect');
                return this.actions$.pipe(
                    ofType(loginSuccess),
                    tap(() => {
                        console.log('Effect: Navigating to /home');
                        this.router.navigate(['/']);
                    })
                );
            }),
        { dispatch: false }
    );

    logout$ = createEffect(
        () =>
            defer(() => {
                console.log('Effect: Initializing logout effect');
                return this.actions$.pipe(
                    ofType(logOut),
                    tap(() => {
                        console.log('Effect: Logging out and navigating to /login');
                        this.userService.logout(); // Clear session storage
                        this.router.navigate(['/']); // Navigate to login page
                    })
                );
            }),
        { dispatch: false }
    );
}
