import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RepositoryService } from 'src/app/services/repository.service';
import { AuthService } from 'src/app/services/auth.service';
import { BagCarts } from 'src/app/interfaces/BagCarts.model';
@Component({
  selector: 'app-bag-cart-creation',
  templateUrl: './bag-cart-creation.component.html',
  styleUrls: ['./bag-cart-creation.component.css']
})
/**
 * Componente que contiene la página del registro de Bagcarts
 */
export class BagCartCreationComponent implements OnInit {

  //Array de BagCarts a mostrar en pantalla
  public bagcarts_list!: BagCarts[];

  registerForm = new FormGroup({
    marca: new FormControl('', [Validators.required]),
    modelo: new FormControl('', [Validators.required, Validators.pattern('[0-9]*')])
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

  get marca() {
    return this.registerForm.controls['marca'].value
  }

  get modelo() {
    return this.registerForm.controls['modelo'].value
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
   * Muestra en pantalla el mensaje "No implementado"
   */
  onSubmit() {
    this.message = "No implementado"
  }
}

