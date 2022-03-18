import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor() { }
  public logout(): void {
    localStorage.setItem('isLoggedIn', 'false');
    localStorage.removeItem('token');
  }
  public isLoggedIn(): boolean {
    if (localStorage.getItem('isLoggedIn') == "true") {
      let token = JSON.parse(localStorage.getItem('token') || '{}')
      if (token.hasOwnProperty('id') && token.hasOwnProperty('password')) {
        return true;
      }
    }
    return false;
  }
  public getCredentials(): any {
    return JSON.parse(localStorage.getItem('token') || '{}')
  }
}