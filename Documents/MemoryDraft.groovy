Reducer: to process the action and update the store, for UI state. Another hand, reducer will listen the action to update state
Effect: handle side effect: API call, navigation. The Effect will listen action to do some side effect

// Component will subscribe the store to see the state change from reducer
account$: Observable<AccountState>;

constructor(private store: Store<{ account: AccountState }>) {
// Select the 'account' slice of state from the store
this.account$ = this.store.select('account');
}

// component dispatch action
this.store.dispatch(loginRequest({ username: 'user', password: 'pass' }));