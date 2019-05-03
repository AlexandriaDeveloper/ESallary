import { Router } from '@angular/router';
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
  constructor(private accountService: AccountService, private route: Router) {}
  authUser(x) {
    console.log(x);
    const token = x.token;
    this.decodedToken = JSON.stringify(this.helper.decodeToken(token));
    localStorage.setItem('token', token);
    localStorage.setItem('user', x.user);
  }
  signOut() {
    this.decodedToken = {};
    localStorage.clear();
    this.route.navigate(['/home']);
  }

  isAuth() {
    if (localStorage.getItem('token') != null) {
      this.decodedToken = JSON.stringify(localStorage.getItem('token'));
      if (!this.helper.isTokenExpired(this.decodedToken)) {
        return true;
      }
    }
    localStorage.clear();
    return false;
  }
  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.helper.isTokenExpired(token);
  }
}
