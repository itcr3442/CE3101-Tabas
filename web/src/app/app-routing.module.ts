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

const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'login/redirect', component: LoginComponent },
  { path: 'planes_list', component: PlanesListComponent, canActivate: [AuthGuard] },
  { path: 'workers_list', component: WorkersListComponent },
  { path: 'worker_reg', component: WorkerRegisterComponent, canActivate: [AuthGuard] },
  { path: 'worker_reg', component: WorkerRegisterComponent, canActivate: [AuthGuard] },
  { path: 'bagcarts_list', component: BagcartsListComponent },
  { path: 'bags_list', component: BagsListComponent },
  { path: 'bag_creation', component: BagCreationComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }