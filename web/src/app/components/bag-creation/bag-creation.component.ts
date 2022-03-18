import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { RepositoryService } from 'src/app/services/repository.service';

@Component({
  selector: 'app-bag-creation',
  templateUrl: './bag-creation.component.html',
  styleUrls: ['./bag-creation.component.css']
})
export class BagCreationComponent implements OnInit {
  registerForm = new FormGroup({
    id: new FormControl('', [Validators.required, Validators.pattern('[0-9]*')]),
    nvuelo: new FormControl('', [Validators.required, Validators.pattern('[0-9]*')]),
    peso: new FormControl('', [Validators.required, Validators.pattern('[0-9.]*')]),
    costo: new FormControl('', [Validators.required, Validators.pattern('[0-9.]*')]),
    color: new FormControl('', [Validators.required, Validators.pattern('#[0-9a-fA-F]{6}')]),
  })
  message: string = ""

  constructor(
    private router: Router,
    private repo: RepositoryService,
    private authService: AuthService
  ) {
  }

  ngOnInit(): void {
  }

  get id() {
    return this.registerForm.controls['id'].value
  }
  get nvuelo() {
    return this.registerForm.controls['nvuelo'].value
  }
  get peso() {
    return this.registerForm.controls['peso'].value
  }
  get costo() {
    return this.registerForm.controls['costo'].value
  }

  set color(new_color: string) {
    this.registerForm.controls['color'].setValue(new_color)
  }

  get color() {
    return this.registerForm.controls['color'].value
  }

  onSubmit() {
    if (this.registerForm.valid) {

      if (!this.authService.isLoggedIn()) {
        this.router.navigate(['/login/redirect']);
        return
      }

      let token = this.authService.getCredentials()

      let registerUrl = "trabajadores?cedula=" + token.id + "&password_hash=" + token.password

      let new_bag = {
        "cedula_usuario": this.id,
        "nvuelo": this.nvuelo,
        "color": this.color,
        "peso": this.peso,
        "costo_envio": this.costo
      }

      console.log("New bag: " + JSON.stringify(new_bag))

      console.log("POST url: " + registerUrl)
      this.repo.create(
        registerUrl, new_bag).subscribe(res => {
          console.log("post result: " + JSON.stringify(res))
          if ((<any>res).success) {
            console.log("Register successful!");
            this.message = ""

          }
          else {
            this.message = "La maleta ya existe o su usuario inexistente";
          }
        }
        )



      console.log("POST url: " + registerUrl)

      let int_color = parseInt(this.color.substring(1), 16)
      console.log("Color:" + int_color)


    }
    else {
      this.message = "Por favor verifique que ingresó todos los campos en su formato correcto";
    }
  }

  generateXML() {
    const d = new Date();
    const day = d.getDay().toString()
    const month = d.getMonth().toString()
    const year = d.getFullYear().toString().substring(2)


    let xmlDoc = document.implementation.createDocument(null, "books");

    let clave = "506" + day.padStart(2, "0") + month.padStart(2, "0") + year + this.id.padStart(12, "0") + Math.random() * 10 ** 19 + "1" + Math.random() * 10 ** 8
  }

}
