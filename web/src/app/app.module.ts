import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';

import { AuthGuard } from './guards/auth.guard';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { WorkerRegisterComponent } from './components/worker-register/worker-register.component';
import { UserRegisterComponent } from './components/user-register/user-register.component';
import { BagCreationComponent } from './components/bag-creation/bag-creation.component';
import { BagCartCreationComponent } from './components/bag-cart-creation/bag-cart-creation.component';
import { PlaneAssignmentComponent } from './components/plane-assignment/plane-assignment.component';
import { BagCartAssignmentComponent } from './components/bag-cart-assignment/bag-cart-assignment.component';
import { CloseBagCartComponent } from './components/close-bag-cart/close-bag-cart.component';
import { PdfReportComponent } from './components/pdf-report/pdf-report.component';
import { PlanesListComponent } from './components/planes-list/planes-list.component';
import { RouterModule } from '@angular/router';
import { BagcartsListComponent } from './components/bagcarts-list/bagcarts-list.component';
import { WorkersListComponent } from './components/workers-list/workers-list.component';
import { BagsListComponent } from './components/bags-list/bags-list.component';

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
    PdfReportComponent,
    PlanesListComponent,
    BagcartsListComponent,
    WorkersListComponent,
    BagsListComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
