import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RepositoryService } from 'src/app/services/repository.service';
import { AuthService } from 'src/app/services/auth.service';
import { Flights } from '../../interfaces/Fligths.model';

@Component({
  selector: 'app-flight-list',
  templateUrl: './flight-list.component.html',
  styleUrls: ['./flight-list.component.css']
})
/**
 * Componente que contiene la página con la lista de todos los vuelos registrados.
 */
export class FlightListComponent implements OnInit {

  public flights_list!: Flights[];

  constructor(
    private repo: RepositoryService
  ) { }

  ngOnInit(): void {
    this.getAllFlights();
  }

 /**
   * Método que realiza el request al servidor para obtener todos
   * los vuelos disponibles.
   */
  public getAllFlights = () =>{
    let loginUrl = "vuelos";
    this.repo.getData(loginUrl)
    .subscribe(res => {
        console.log("Result:" + JSON.stringify(res));
        this.flights_list = res as Flights[];
      }
    )
  }

}
