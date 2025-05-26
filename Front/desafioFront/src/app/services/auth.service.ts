import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, catchError, map, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5075/api/Authentication';

  private authStatus = new BehaviorSubject<boolean>(this.hasToken()); // Aqui
  authStatus$ = this.authStatus.asObservable();

  private userLogged = new BehaviorSubject<string | null>(this.getUserLogged());
  userLogged$ = this.userLogged.asObservable();
  
  constructor(private http: HttpClient) {
    const token = this.getToken();
    this.authStatus.next(!!token);
    this.userLogged.next(this.getUserLogged());
  }

  login(email: string, password: string) {
    return this.http.post<any>(`${this.apiUrl}`, { email, password }).pipe(
      tap(response => {
        if (response && response.accessToken) {
          localStorage.setItem('token', response.accessToken);
          this.setUserLogged(response.userIdentifier);
          this.authStatus.next(true);
        }
      })
    );
  }

  setUserLogged(value: string) {
    localStorage.setItem('userIdentifier', value);
    this.userLogged.next(value);
  }

  clearUserLogged() {
    localStorage.removeItem('userIdentifier');
    this.userLogged.next(null);
  }

  hasToken(): boolean {
    return !!localStorage.getItem('token');
  }

  register(name: string, email: string, password: string) {
    return this.http.post<any>(`${this.apiUrl}/CreateUser`, { name, email, password });
  }

  logout() {
    localStorage.removeItem('token');
    this.clearUserLogged();
    this.authStatus.next(false);
  }

  getToken() {
    return localStorage.getItem('token');
  }

  getUserLogged() {
    return localStorage.getItem('userIdentifier');
  }

  isAuthenticated() {
    const token = localStorage.getItem('token');
    if (!token) {
      this.authStatus.next(false);
      return of(false);
    }

    return this.http.get<string>(`${this.apiUrl}/GetUserLogged`, {
      headers: { Authorization: `Bearer ${token}` }
    }).pipe(
      map(() => {
        this.authStatus.next(true);
        return true;
      }),
      catchError(() => {
        this.authStatus.next(false);
        return of(false);
      })
    );
  }
}
