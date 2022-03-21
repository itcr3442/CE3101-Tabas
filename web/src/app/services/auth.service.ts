import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor() { }
  /**
   * Remueve los datos de local storage que mantienen la sesión del usuario
   */
  public logout(): void {
    localStorage.setItem('isLoggedIn', 'false');
    localStorage.removeItem('token');
  }
  /**
   * 
   * @returns Revisa que los datos necesarios estén presentes en local storage para decir que el usuario está ingresado
   */
  public isLoggedIn(): boolean {
    if (localStorage.getItem('isLoggedIn') == "true") {
      let token = JSON.parse(localStorage.getItem('token') || '{}')
      if (token.hasOwnProperty('id') && token.hasOwnProperty('password')) {
        return true;
      }
    }
    return false;
  }
  /**
   * Obtiene la info del usuario actual de local storage, llamar después de isLoggedIn()
   * @returns Object con 'id' y 'password' fields
   */
  public getCredentials(): any {
    return JSON.parse(localStorage.getItem('token') || '{}')
  }
}