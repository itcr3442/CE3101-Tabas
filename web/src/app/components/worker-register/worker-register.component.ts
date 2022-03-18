import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RepositoryService } from 'src/app/services/repository.service';
import { AuthService } from 'src/app/services/auth.service';
import { Workers } from 'src/app/interfaces/workers.model';

@Component({
  selector: 'app-worker-register',
  templateUrl: './worker-register.component.html',
  styleUrls: ['./worker-register.component.css']
})
/**
 * Componente que contiene la página de registro de nuevos trabajadores.
 */
export class WorkerRegisterComponent implements OnInit {

  public worker_list!: Workers[];

  registerForm = new FormGroup({
    id: new FormControl('', [Validators.required, Validators.pattern('[0-9]*')]),
    nombre: new FormControl('', [Validators.required]),
    apellido1: new FormControl('', [Validators.required]),
    apellido2: new FormControl('', [Validators.required]),
    rol: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required])
  })
  roles = ['admin', 'scan', 'recepcionista', 'embarcador'];
  message: string = ""

  constructor(
    private router: Router,
    private repo: RepositoryService,
    private authService: AuthService
  ) {
  }

  ngOnInit(): void {
    this.getAllWorkers();
  }

  get id() {
    return this.registerForm.controls['id'].value
  }

  get password() {
    return this.registerForm.controls['password'].value
  }

  get role() {
    return this.registerForm.controls['rol'].value
  }

  get name() {
    return this.registerForm.controls['nombre'].value
  }

  get apellido1() {
    return this.registerForm.controls['apellido1'].value
  }
  get apellido2() {
    return this.registerForm.controls['apellido2'].value
  }
  /**
   * Método que realiza el request al servidor para obtener todos
   * los trabajadores para mostrarlos en la lista correspondiente.
   */
  public getAllWorkers = () =>{
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login/redirect']);
      return
    }
    let token = this.authService.getCredentials()
    let registerUrl = "trabajadores?cedula_admin=" + token.id + "&password_hash=" + token.password
    console.log(registerUrl);
    this.repo.getData(registerUrl)
    .subscribe(res => {
        console.log("Result:" + JSON.stringify(res));
        this.worker_list = res as Workers[];
      }
    )
  }

  changeRole(e: any) {
    this.registerForm.controls['rol'].setValue(e.target.value, {
      onlySelf: true,
    });
  }

  /**
   * Método que se ejecuta al apretar el botón registrar.
   * Verifica si el ususario ha iniciado sesión, para obtener los valores de las
   * casillas correspondientes y así registrar el nuevo trabajador en la base de datos del servidor
   */
  onSubmit() {
    if (this.registerForm.valid) {

      if (!this.authService.isLoggedIn()) {
        this.router.navigate(['/login/redirect']);
        return
      }

      let token = this.authService.getCredentials()

      let registerUrl = "trabajadores?cedula=" + token.id + "&password_hash=" + token.password

      let hash = this.password
      let new_worker = {
        "cedula": this.id,
        "password_hash": hash,
        "nombre": this.name,
        "primer_apellido": this.apellido1,
        "segundo_apellido": this.apellido2,
        "rol": this.role
      }

      console.log("New worker: " + JSON.stringify(new_worker))


      console.log("POST url: " + registerUrl)
      this.repo.create(
        registerUrl, new_worker).subscribe(res => {
          console.log("post result: " + JSON.stringify(res))
          if ((<any>res).success === 1) {
            console.log("Register successful");
            this.message = ""

          }
          else if ((<any>res).success === -1) {
            this.message = "La cédula dada ya se encuentra registrada en el sistema.";
          } else if ((<any>res).success === -2) {
            this.message = "El rol seleccionado no es válido.";
          } else if ((<any>res).success === -3) {
            this.message = "Usted no cuenta con permisos suficientes para realizar esta acción.";
          }
        }
        )

    }
    else {
      this.message = "Por favor verifique que ingresó todos los campos, la cédula solo contiene dígitos y escogió un rol";
    }
  }

}
