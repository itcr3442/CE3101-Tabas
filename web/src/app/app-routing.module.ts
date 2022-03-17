import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BagcartsListComponent } from './components/bagcarts-list/bagcarts-list.component';
import { BagsListComponent } from './components/bags-list/bags-list.component';
import { LoginComponent } from './components/login/login.component';
import { PlanesListComponent } from './components/planes-list/planes-list.component';
import { WorkersListComponent } from './components/workers-list/workers-list.component';

const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'planes_list', component: PlanesListComponent },
  { path: 'workers_list', component: WorkersListComponent },
  { path: 'bagcarts_list', component: BagcartsListComponent },
  { path: 'bags_list', component: BagsListComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }