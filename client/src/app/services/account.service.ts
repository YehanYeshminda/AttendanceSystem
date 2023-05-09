import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../models/user';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl: string = 'https://localhost:7228/api/';
  private currentSourceUser = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentSourceUser.asObservable();

  constructor(private http: HttpClient) {
    const user = sessionStorage.getItem('user');
    if (user) {
      const parsedUser: User = JSON.parse(user);
      this.currentSourceUser.next(parsedUser);
    }
  }

  login(user: User) {
    return this.http.post<User>(this.baseUrl + 'account/login', user).pipe(
      map((res) => {
        const user = res;
        if (user) {
          this.setCurrentUser(user);
        }
        return user;
      })
    );
  }

  setCurrentUser(user: User) {
    sessionStorage.setItem('user', JSON.stringify(user));
    this.currentSourceUser.next(user);
  }

  logout() {
    sessionStorage.removeItem('user');
    this.currentSourceUser.next(null);
  }

  getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]));
  }
}
