import { Component } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-default-layout',
  imports: [RouterOutlet, RouterLink],
  templateUrl: './default-layout.component.html',
  styleUrl: './default-layout.component.scss'
})
export class DefaultLayoutComponent {

  constructor(private router: Router) {}

  logout() {
  localStorage.removeItem('userData');
  this.router.navigate(['/login']);
}

}
