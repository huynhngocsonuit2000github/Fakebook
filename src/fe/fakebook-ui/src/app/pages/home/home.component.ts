import { Component } from '@angular/core';
import { AccountInfoComponent } from '../../compoments/account-info/account-info.component';
import { SharedModule } from '../../shared/shared.module';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [AccountInfoComponent, SharedModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {

}
