import { Component, OnInit } from '@angular/core';
import { Flights } from 'src/app/interfaces/Fligths.model';
import { RepositoryService } from 'src/app/services/repository.service';

@Component({
  selector: 'app-flight-list',
  templateUrl: './flight-list.component.html',
  styleUrls: ['./flight-list.component.css']
})
export class FlightListComponent implements OnInit {

  public flights_list!: Flights[];

  constructor(
    private repo: RepositoryService
  ) { }

  ngOnInit(): void {
    this.getAllFlights();
  }

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
