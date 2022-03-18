import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RepositoryService } from 'src/app/services/repository.service';
import { AuthService } from 'src/app/services/auth.service';
import { BagCarts } from 'src/app/interfaces/BagCarts.model';

@Component({
  selector: 'app-close-bag-cart',
  templateUrl: './close-bag-cart.component.html',
  styleUrls: ['./close-bag-cart.component.css']
})
export class CloseBagCartComponent implements OnInit {

  public bagcarts_list!: BagCarts[];

  registerForm = new FormGroup({
    id: new FormControl('', [Validators.required, Validators.pattern('[0-9]*')])
  })

  message: string = ""

  constructor(
    private router: Router,
    private repo: RepositoryService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.getAllBagCarts();
  }

  get id() {
    return this.registerForm.controls['id'].value
  }

  public getAllBagCarts = () =>{
    let registerUrl = "bagcarts"
    this.repo.getData(registerUrl)
    .subscribe(res => {
        console.log("Result:" + JSON.stringify(res));
        this.bagcarts_list = res as BagCarts[];
      }
    )
  }

  onSubmit() {
    this.message = "No implementado"
  }

}
