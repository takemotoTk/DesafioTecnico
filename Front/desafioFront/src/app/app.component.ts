import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(public authService: AuthService, private router: Router) {}
  
  isLoggedIn = false;
  userLogged?: string | null;

  ngOnInit() {
    this.authService.authStatus$.subscribe(isAuth => {
      this.isLoggedIn = isAuth;
    });

    this.authService.userLogged$.subscribe(user => {
      this.userLogged = user;
    });
  }
  
  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
