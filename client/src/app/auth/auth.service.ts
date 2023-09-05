import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';
import { environment } from 'src/environments/environmet';
import  jwt_decode from "jwt-decode";
import { UserModel } from '../shared/models/user.model';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loggedIn: boolean = false;
  public userSubject: BehaviorSubject<UserModel | null> = new BehaviorSubject<UserModel | null>(null);
  public user: UserModel | null;
  constructor(
      private http: HttpClient
    ) {
      this.user = this.loadSession();
      this.userSubject.next(this.user)
    }

  login(username: string, password: string) {
    return this.http
      .post(`${environment.apiUrl}/User/Login`, { username, password })
        .pipe(
          tap((response: any) => this.saveToken(response))
        );
  }

  logout(): void {
    this.clearSession();
  }

  private saveToken(authResult: any): void {
    console.log(authResult)
    localStorage.setItem('jwtToken', authResult.token);
    localStorage.setItem('user', JSON.stringify(authResult.user));
    this.loggedIn = true;
  }

  isLoggedIn(): boolean {
    return this.loggedIn;
  }

  private loadSession(): (UserModel | null) {
    var localUser = localStorage.getItem('user');

    if (localUser == null) {
      return null;
    }

    var parsed = JSON.parse(localUser);

    var user: UserModel = {
      id: parsed.id,
      username: parsed.username
    }

    return user;
  }

  private clearSession(): void {
    localStorage.clear();
  }
}
