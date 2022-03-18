import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BagcartsListComponent } from './components/bagcarts-list/bagcarts-list.component';
import { BagsListComponent } from './components/bags-list/bags-list.component';
import { LoginComponent } from './components/login/login.component';
import { PlanesListComponent } from './components/planes-list/planes-list.component';
import { WorkersListComponent } from './components/workers-list/workers-list.component';
import { WorkerRegisterComponent } from './components/worker-register/worker-register.component';
import { BagCreationComponent } from './components/bag-creation/bag-creation.component';
import { AuthGuard } from './guards/auth.guard';
import { BagCartCreationComponent } from './components/bag-cart-creation/bag-cart-creation.component';
import { FlightListComponent } from './components/flight-list/flight-list.component';
import { UserRegisterComponent } from './components/user-register/user-register.component';
import { BagCartAssignmentComponent } from './components/bag-cart-assignment/bag-cart-assignment.component';
import { CloseBagCartComponent } from './components/close-bag-cart/close-bag-cart.component';
import { PdfReportComponent } from './components/pdf-report/pdf-report.component';

const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'login/redirect', component: LoginComponent },
  { path: 'planes_list', component: PlanesListComponent, canActivate: [AuthGuard] },
  { path: 'workers_list', component: WorkersListComponent },
  { path: 'worker_reg', component: WorkerRegisterComponent, canActivate: [AuthGuard] },
  { path: 'bagcarts_creation', component: BagCartCreationComponent },
  { path: 'bagcarts_assignement', component: BagCartAssignmentComponent },
  { path: 'close_bagcarts', component: CloseBagCartComponent },
  { path: 'bag_creation', component: BagCreationComponent },
  { path: 'flight_list', component: FlightListComponent },
  { path: 'user_register', component: UserRegisterComponent },
  { path: 'pdf_report', component: PdfReportComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }