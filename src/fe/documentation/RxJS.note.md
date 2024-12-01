# note

- after calling .error() and .complete() the stream is finished

# [Subject]

- when call this.observable$.next(value), the code subscribe to this subject will be execute with this value.

- When call the complete() method, it will stop the process, so even we call the next() method, this will prevent it from emitting any futher values
- When the error is called, the stream is stop

- just complete when specific comdition
<!--
    if (this.i === 10) {
    // Complete only when i reaches 10
        this.observable$.complete();
    }
-->

# [BehaviorSubject]

- When init the object we need to assign the initial value
- When subscribe, it will execute with the last value
- When the error is called, the stream is stop

# [ReplaySubject]

- use to store the fixed number of latest values, for example we always want the subscriber always have 3 number values of emit, even it had just subscribed the observable
  new ReplaySubject<number>(3)
- we can use ReplaySubject to implement the cache

# [AsyncSubject]

- emit the latest value when the observable is completed, just emit when completed

# [from]

    from([1,2,5])

- It will emit each element, 1, 2, 5. Each element is a number
  this.stateService.getData().subscribe((response) => {
  console.log(response);
  this.data = response;
  });
    <!-- 1 -->
    <!-- 2 -->
    <!-- 3 -->

- We can use from to convert fetch() to Observable
  const promise$ = from(fetch('https://jsonplaceholder.typicode.com/posts').then((res) => res.json()));

promise$.subscribe({
next: (data) => console.log('Promise Resolved:', data),
complete: () => console.log('Promise Completed'),
});

==>> Converts arrays, promises, or iterables into an Observable.

# [of]

    of([1,2,5], [12,22,53])

- It will emit the data separated by ',' at a time
- It will log:
  this.stateService.getData().subscribe((response) => {
  console.log(response);
  this.data = response;
  });
    <!-- (3) [1, 2, 5] -->
    <!-- (3) [12,22,53] -->

==>> Emits a sequence of static values.

# [takeUntil]

- ensure the observable is destroyed when it reach specific condition
  ==>> use when the component is destroyed, we can unsubscribe the Observables. When user logout, we can unsubscribe something, use to cancel the HTTP request

# [unsubscribe]

- will manual unsubscribe the observable, subscribe method will return the object, we can use that object to unsubscribe behavior

  this.subscription = data$.subscribe(data => {
  console.log(data);
  });

  ngOnDestroy() {
  this.subscription.unsubscribe(); // Unsubscribe manually
  }

# [switchMap]

- use to switch the current steam to another new stream. It will destroy the current stream (observable) and in the switchMap it will return new stream (observable), then the subscribe will execute the new value of new stream

- without using switchMap, we need to implement the code to keep track the object of this subscribe and manually unsubscribe this

# [debounceTime]

- After we call next() to emit the value to the subscriber, debounceTime is really helpful when it prevent emit the value immediately, actually waiting for a while to emit to the subscriber

# [distinctUntilChanged]

- ensure the old and new value should be different to be able to emit data

# [map]

- use to transform the source emitted value to new value

  const numbers$ = of(1, 2, 3, 4);

  numbers$.pipe(
  map(n => n \* 2)  
  ).subscribe(result => {
  console.log(result);
  });

# [mergeMap]

- change one emitted value to another observable, no care about order, the emission can be come anytime
- when it perform the observable completely, it will return the emission result into subscribe in any order

  const numbers$ = of(1, 2, 3);

  numbers$.pipe(
  mergeMap(n => of(n \* 2))
  ).subscribe(result => {
  console.log(result);
  });

# [concatMap]

- change one emitted value to another observable, and return new observable, care about order, need to wait for the previous observable to be completed

- when it perform the observable completely, it will return the emission result into subscribe in correct order

- map the emitted value to new observable and waiting for those completed.
  ==>> useful for case of asynchronous programming

  const numbers$ = of(1, 2, 3);

  numbers$.pipe(
  concatMap(n => of(n \* 2))
  ).subscribe(result => {
  console.log(result);
  });

# [filter]

- use to filter out the value which do not meet the certain condition, filter function will receive the predicate function and return true/false value

# [take]

- get specific number of first emissions, and the observable will automatically complete,

# [skip]

- skip specific number of first emissions

# [combineLatest]

- combine many different observable into one. Just run when at least one emitted value for each observable. and run whenever one of observable had emitted new value, only one

  combineLatest([userInput$, apiResponse$]).pipe(
  map(([input, response]) => `${input} ${response}`)
  )

# [forkJoin]

- for example, we have 3 observable, it will get 3 latest value from 3 observable and emit to the next at once
  forkJoin([request1$, request2$, request3$])

# [zip]

- join 2 observable together and Ob1 have 2 values, Ob2 have 3 values, it will emit 2 times
- if one observable is take long time to emit, another observable will wait for the long observable complete to be a pair of emission and emit at once

  const observable1$ = of('A', 'B', 'C');
  const observable2$ = of(1, 2);

  zip(observable1$, observable2$).subscribe({
  next: ([val1, val2]) => console.log(`Combined: ${val1} and ${val2}`),
  complete: () => console.log('All pairs emitted')
  });

  Combined: A and 1
  Combined: B and 2
  All pairs emitted

# [catchError]

- use to catch the error, if there is any error while perform the stream, this method will catch the error to ensure the stream is perform without any block
  observable$.pipe(
  catchError((err) => {
  return of(0);
  })
  )

# [retry]

- cause the observable to retry up to n times, if n + 1 still failed, it will get the latest value even it is throw error
  observable$.pipe(
  retry(1),
  )

# [retryWhen]

- the same idea of retry, but the parameter is a pipe, then we can do it flexible
