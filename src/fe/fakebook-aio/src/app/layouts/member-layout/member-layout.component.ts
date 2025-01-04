import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-member-layout',
  standalone: true,
  imports: [RouterLink, RouterOutlet],
  templateUrl: './member-layout.component.html',
  styleUrl: './member-layout.component.scss'
})
export class MemberLayoutComponent {

}
