import { Component, OnInit } from '@angular/core';
import { Plane_type } from 'src/app/interfaces/plane_type.model';
import { RepositoryService } from 'src/app/services/repository.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
@Component({
  selector: 'app-planes-list',
  templateUrl: './planes-list.component.html',
  styleUrls: ['./planes-list.component.css']
})
/**
 * Componente que contiene la página donde se realiza la asignación de
 * aviones a lops vuelos
 */
export class PlanesListComponent implements OnInit {

  registerForm = new FormGroup({
    idavion: new FormControl('', [Validators.required, Validators.pattern('[0-9]*')]),
    idvuelo: new FormControl('', [Validators.required, Validators.pattern('[0-9]*')])
  })

  message: string = ""

  public plane_types!: Plane_type[];

  constructor(
    private repo: RepositoryService
  ) { }

  
  ngOnInit(): void {
    this.getAllPlaneTypes();
  }

  /**
   * Método que realiza el request al servidor para obtener todos
   * los tipos de aviones disponibles.
   */
  public getAllPlaneTypes = () =>{
    let loginUrl = "tipo_avion";
    this.repo.getData(loginUrl)
    .subscribe(res => {
        console.log("Result:" + JSON.stringify(res));
        this.plane_types = res as Plane_type[];
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
