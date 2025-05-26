import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-register',
  styleUrls: ['./register.component.css'],
  template: `
    <h2>Cadastro</h2>
    <form (ngSubmit)="onSubmit()" #form="ngForm" novalidate>
      <label>
        Nome:
        <input type="text" name="name" [(ngModel)]="name" required />
      </label>
      <br />
      <label>
        Email:
        <input type="email" name="email" [(ngModel)]="email" required />
      </label>
      <br />
      <label>
        Password:
        <input type="password" name="password" [(ngModel)]="password" required minlength="6" />
      </label>
      <br />
      <button type="submit" [disabled]="form.invalid">Registrar</button>
    </form>
    <p *ngIf="error" style="color:red">{{ error }}</p>
    <p *ngIf="success" style="color:green">{{ success }}</p>
    <p>
      Já tem conta? 
      <a routerLink="/login">Login</a>
    </p>
  `
})
export class RegisterComponent {
  name = '';
  email = '';
  password = '';
  error = '';
  success = '';

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
    this.error = '';
    this.success = '';
    this.authService.register(this.name, this.email, this.password).subscribe({
      next: () => {
        this.success = 'Usuário criado com sucesso! Faça login.';
        setTimeout(() => this.router.navigate(['/login']), 2000);
      },
      error: (err: Error) => {
        this.error = 'Erro no cadastro: ' + err.message;
      }
    });
  }
}
