import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service'
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
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
  message!: string;
  public loginError!: String;
  bcrypt = require('bcryptjs');
  salt = this.bcrypt.genSaltSync(10);

  constructor(
    private router: Router,
    private authService: AuthService,
    private repo: RepositoryService
  ) { }

  ngOnInit(): void {
  }

  get id() {
    return this.loginForm.controls['id'].value
  }

  get password() {
    return this.loginForm.controls['password'].value
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
          }
          else {
            this.loginError = "Please check your userid and password";
          }
        }
        )

    }
  }
}
