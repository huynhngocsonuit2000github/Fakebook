// src/app/state/effects/account.effects.ts
import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Router } from '@angular/router';
import {
    loginRequest,
    loginFailure,
    logOut,
    beforeLoginSuccess,
    afterLoginSuccess,
} from '../actions/account.actions';
import { catchError, delay, map, mergeMap, tap } from 'rxjs/operators';
import { defer, of } from 'rxjs';
import { UserService } from '../../services/user.service';

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
                            return beforeLoginSuccess({ token });
                        }),
                        catchError((error) => {
                            return of(loginFailure({ error }));
                        })
                    )
                )
            );
        })
    );

    beforeLoginSuccess$ = createEffect(
        () =>
            defer(() => {
                console.log('Effect: Initializing before login success effect');
                return this.actions$.pipe(
                    ofType(beforeLoginSuccess),
                    tap(() => { console.log('Effect: before login'); }),
                    mergeMap(({ token }) =>
                        this.userService.getUserPermissions(token).pipe(
                            map((permissions) => {
                                console.log('from effect: ', permissions);

                                return afterLoginSuccess({ permissions });
                            }),
                            catchError((error) => {
                                return of(loginFailure({ error }));
                            })
                        )
                    )
                );
            }),
        { dispatch: true }
    );

    afterLoginSuccess$ = createEffect(
        () =>
            defer(() => {
                console.log('Effect: Initializing loginRedirect effect');
                return this.actions$.pipe(
                    ofType(afterLoginSuccess),
                    tap(() => {
                        console.log('Effect: Navigating to /112home');
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
                        this.userService.logout(); // Clear session storage
                        // this.router.navigate(['/']); // Navigate to login page
                    })
                );
            }),
        { dispatch: false }
    );
}
