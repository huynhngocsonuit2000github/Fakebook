import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { AccountState } from '../../state/account.state';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { beforeLoginSuccess } from '../../state/actions/account.actions';

@Component({
  selector: 'app-callback',
  standalone: true,
  imports: [],
  templateUrl: './callback.component.html',
  styleUrl: './callback.component.scss'
})
export class CallbackComponent implements OnInit {

  constructor(private route: ActivatedRoute,
    private userService: UserService,
    private router: Router,
    private store: Store<{ account: AccountState }>
  ) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const email = params['email'];
      const idPToken = params['idPToken'];

      this.exchangeIdPToken(email, idPToken);
    });
  }

  exchangeIdPToken(email: string, token: string) {
    this.userService.exchangeIdPToken(email, token).subscribe(token => {
      this.store.dispatch(beforeLoginSuccess({ token }));
    })
  }
}