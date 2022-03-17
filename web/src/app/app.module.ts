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
import { PlanesListComponent } from './planes-list/planes-list.component';
import { RouterModule } from '@angular/router';
import { BagcartsListComponent } from './bagcarts-list/bagcarts-list.component';
import { WorkersListComponent } from './workers-list/workers-list.component';
import { BagsListComponent } from './bags-list/bags-list.component';
import { HomePageComponent } from './home-page/home-page.component';

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
    HomePageComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(
      [
        {path: 'home_page', component: HomePageComponent},
        {path: 'planes_list', component: PlanesListComponent},
        {path: 'workers_list', component: WorkersListComponent},
        {path: 'bagcarts_list', component: BagcartsListComponent},
        {path: 'bags_list', component: BagsListComponent}
      ]
    )
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
