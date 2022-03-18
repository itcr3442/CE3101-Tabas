import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RepositoryService } from 'src/app/services/repository.service';
import { AuthService } from 'src/app/services/auth.service';
import { Users } from 'src/app/interfaces/Users.model';
@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css']
})
/**
 * Componente que contiene la página de registro de usuarios.
 */
export class UserRegisterComponent implements OnInit {

  public users_list!: Users[];

  registerForm = new FormGroup({
    id: new FormControl('', [Validators.required, Validators.pattern('[0-9]*')]),
    nombre: new FormControl('', [Validators.required]),
    apellido1: new FormControl('', [Validators.required]),
    apellido2: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
    telefono: new FormControl('', [Validators.required, Validators.pattern('[0-9]*')])
  })

  message: string = ""

  constructor(
    private router: Router,
    private repo: RepositoryService,
    private authService: AuthService
  ) {
  }

  /**
   * Método que realiza el request al servidor para obtener todos
   * los usuarios y su información de la base de datos.
   */
  public getAllUsers = () =>{
    let registerUrl = "usuarios"
    console.log(registerUrl);
    this.repo.getData(registerUrl)
    .subscribe(res => {
        console.log("Result:" + JSON.stringify(res));
        this.users_list = res as Users[];
      }
    )
  }

  ngOnInit(): void {
    this.getAllUsers();
  }
  
   /**
   * Método que se ejecuta al apretar el botón registrar.
   * Muestra en pantalla el mensaje "No implementado"
   */
  onSubmit() {
    this.message = "No implementado"
  }

}
