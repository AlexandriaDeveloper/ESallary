import { AccountService } from './account.service';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  helper = new JwtHelperService();
  decodedToken;
  constructor(private accountService: AccountService) {}
  authUser(x) {
    console.log(x);
    const token = x.token;
    this.decodedToken = JSON.stringify (this.helper.decodeToken(token));
    localStorage.setItem('token', token);
    localStorage.setItem('user', x.user);
  }
  signOut() {
    this.decodedToken = {};
    localStorage.clear();
  }

  isAuth() {
    if (localStorage.getItem('token') != null) {
      this.decodedToken = JSON.stringify(localStorage.getItem('token'));

      return true;
    }
    return false;
  }
}
