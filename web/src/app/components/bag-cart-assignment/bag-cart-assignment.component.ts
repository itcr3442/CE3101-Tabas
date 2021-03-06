import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RepositoryService } from 'src/app/services/repository.service';
import { AuthService } from 'src/app/services/auth.service';
import { BagCarts } from 'src/app/interfaces/BagCarts.model';
@Component({
  selector: 'app-bag-cart-assignment',
  templateUrl: './bag-cart-assignment.component.html',
  styleUrls: ['./bag-cart-assignment.component.css']
})

/**
 * Componente que contiene la página de asignación de Bagcarts a los aviones
 */
export class BagCartAssignmentComponent implements OnInit {

  //Array de BagCarts a mostrar en pantalla
  public bagcarts_list!: BagCarts[];

  registerForm = new FormGroup({
    idvuelo: new FormControl('', [Validators.required, Validators.pattern('[0-9]*')]),
    idbagcart: new FormControl('', [Validators.required, Validators.pattern('[0-9]*')])
  })

  //Mensaje que parece si ocurre algún error 
  message: string = ""

  constructor(
    private router: Router,
    private repo: RepositoryService,
    private authService: AuthService
  ) { }
  
  ngOnInit(): void {
    this.getAllBagCarts();
  }

  get idVuelo() {
    return this.registerForm.controls['idvuelo'].value
  }

  get idBagCart() {
    return this.registerForm.controls['idbagcart'].value
  }

  /**
   * Método que realiza el request al servidor para obtener todos
   * los BagCarts para mostrarlos en la lista correspondiente.
   */
  public getAllBagCarts = () =>{
    let registerUrl = "bagcarts"
    this.repo.getData(registerUrl)
    .subscribe(res => {
        console.log("Result:" + JSON.stringify(res));
        this.bagcarts_list = res as BagCarts[];
      }
    )
  }

  /**
   * Método que se ejecuta al apretar el botón registrar.
   * Verifica si el ususario ha iniciado sesión, para obtener los valores de las
   * casillas correspondientes y así registrar la relación entre el Bagcart 
   * y el vuelo indicados en la base de datos
   */
  onSubmit() {
    if (this.registerForm.valid) {

      if (!this.authService.isLoggedIn()) {
        this.router.navigate(['/login/redirect']);
        return
      }

      let token = this.authService.getCredentials()

      let registerUrl = "rel/vuelo_bagcart?cedula=" + token.id + "&password_hash=" + token.password

      let new_flight_bagcart_relation = {
        "id_vuelo": this.idVuelo,
        "id_bagcart": this.idBagCart
      }

      console.log("New flight/bagcart relation: " + JSON.stringify(new_flight_bagcart_relation))


      console.log("POST url: " + registerUrl)
      this.repo.create(
        registerUrl, new_flight_bagcart_relation).subscribe(res => {
          console.log("post result: " + JSON.stringify(res))
          if ((<any>res).success === 1) {
            console.log("Register successful");
            this.message = ""
          }
          else if ((<any>res).success === 0) {
            this.message = "Sus credenciales no son válidos, o este bag cart ya está registrado en el sistema";
          } 
        }
        )
    }
    else {
      this.message = "Por favor verifique que ingresó todos los campos correctamente";
    }
  }

}
