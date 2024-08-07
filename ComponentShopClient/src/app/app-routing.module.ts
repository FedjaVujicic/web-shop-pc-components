import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MonitorsComponent } from './components/products/monitors/monitors.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { HomeComponent } from './components/home/home.component';
import { GpusComponent } from './components/products/gpus/gpus.component';
import { GpuInfoComponent } from './components/products/gpu-info/gpu-info.component';
import { MonitorInfoComponent } from './components/products/monitor-info/monitor-info.component';
import { ProfileComponent } from './components/profile/profile.component';
import { CartComponent } from './components/cart/cart.component';
import { RegisterRequestsComponent } from './components/register-requests/register-requests.component';

const routes: Routes = [
  {
    path: "",
    component: HomeComponent,
  },
  {
    path: "login",
    component: LoginComponent,
  },
  {
    path: "register",
    component: RegisterComponent,
  },
  {
    path: "monitors",
    component: MonitorsComponent,
  },
  {
    path: "gpus",
    component: GpusComponent,
  },
  {
    path: "gpu-info/:id",
    component: GpuInfoComponent,
  },
  {
    path: "monitor-info/:id",
    component: MonitorInfoComponent,
  },
  {
    path: "profile",
    component: ProfileComponent,
  },
  {
    path: "cart",
    component: CartComponent,
  },
  {
    path: "register-requests",
    component: RegisterRequestsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
