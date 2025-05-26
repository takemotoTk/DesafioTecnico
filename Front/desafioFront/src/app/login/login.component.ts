import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  styleUrls: ['./login.component.css'],
  template: `
    <h2>Login</h2>
    <form (ngSubmit)="onSubmit()" #form="ngForm" novalidate>
      <label>
        Email:
        <input type="email" name="email" [(ngModel)]="email" required />
      </label>
      <br />
      <label>
        Password:
        <input type="password" name="password" [(ngModel)]="password" required />
      </label>
      <br />
      <button type="submit" [disabled]="form.invalid">Login</button>
    </form>
    <p *ngIf="error" style="color:red">{{ error }}</p>
    <p>
      Não tem conta? 
      <a routerLink="/register">Cadastre-se</a>
    </p>
  `
})
export class LoginComponent {
  email = '';
  password = '';
  error = '';

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
    this.error = '';
    this.authService.login(this.email, this.password).subscribe({
      next: () => this.router.navigate(['/people']),
      error: err => this.error = 'Login falhou: ' + (err.error?.message || err.message || err)
    });
  }
}
