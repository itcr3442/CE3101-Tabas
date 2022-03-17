import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { NavbarComponent } from './navbar/navbar.component';
import { WorkerRegisterComponent } from './worker-register/worker-register.component';
import { UserRegisterComponent } from './user-register/user-register.component';
import { BagCreationComponent } from './bag-creation/bag-creation.component';
import { BagCartCreationComponent } from './bag-cart-creation/bag-cart-creation.component';
import { PlaneAssignmentComponent } from './plane-assignment/plane-assignment.component';
import { BagCartAssignmentComponent } from './bag-cart-assignment/bag-cart-assignment.component';
import { CloseBagCartComponent } from './close-bag-cart/close-bag-cart.component';
import { PdfReportComponent } from './pdf-report/pdf-report.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NavbarComponent,
    WorkerRegisterComponent,
    UserRegisterComponent,
    BagCreationComponent,
    BagCartCreationComponent,
    PlaneAssignmentComponent,
    BagCartAssignmentComponent,
    CloseBagCartComponent,
    PdfReportComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
