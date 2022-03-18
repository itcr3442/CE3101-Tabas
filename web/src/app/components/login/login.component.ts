import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service'
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RepositoryService } from 'src/app/services/repository.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm = new FormGroup({
    id: new FormControl('', [Validators.required, Validators.pattern('[0-9]*')]),
    password: new FormControl('', [Validators.required])
  })
  message: string = ""
  bcrypt = require('bcryptjs');
  salt = this.bcrypt.genSaltSync(10);
  logged: boolean;
  constructor(
    private router: Router,
    private authService: AuthService,
    private repo: RepositoryService
  ) {
    this.logged = authService.isLoggedIn()
  }

  ngOnInit(): void {
    this.logged = this.authService.isLoggedIn()
    if (this.router.url === "/login/redirect") {
      this.message = "Debe ingresar al sistema para poder acceder a esa página"
    }
  }

  get id() {
    return this.loginForm.controls['id'].value
  }

  get password() {
    return this.loginForm.controls['password'].value
  }

  logout() {
    this.authService.logout()
    this.logged = false
  }

  onSubmit() {
    if (this.loginForm.valid) {

      let hash = this.bcrypt.hashSync(this.password, this.salt);
      if (this.password === "cusadmin") {
        hash = this.password
      }
      let loginUrl = "check_login?cedula=" + this.id.trim() + "&password_hash=" + hash

      console.log("GET url: " + loginUrl)
      this.repo.getData(
        loginUrl).subscribe(res => {
          if ((<any>res).success) {
            console.log("Login successful");
            localStorage.setItem('isLoggedIn', "true");
            localStorage.setItem('token', JSON.stringify({ "id": this.id, "password": this.password }));
            this.logged = true
            this.message = ""

          }
          else {
            this.message = "Cédula o contraseña incorrectos";
          }
        }
        )

    }
    else {
      this.message = "Por favor verifique que ingresó ambos campos y su cédula solo contiene dígitos";
    }
  }
}
