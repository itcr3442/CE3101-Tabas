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
export class BagCartCreationComponent implements OnInit {

  public bagcarts_list!: BagCarts[];

  registerForm = new FormGroup({
    marca: new FormControl('', [Validators.required]),
    modelo: new FormControl('', [Validators.required, Validators.pattern('[0-9]*')])
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

  get marca() {
    return this.registerForm.controls['marca'].value
  }

  get modelo() {
    return this.registerForm.controls['modelo'].value
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

