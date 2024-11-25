import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TestAccountComponent } from './compoments/test-account/test-account.component';
import { AccountInfoComponent } from './compoments/account-info/account-info.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [TestAccountComponent, AccountInfoComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
}
