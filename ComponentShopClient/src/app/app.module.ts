import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { MonitorsComponent } from './components/products/monitors/monitors.component';
import { MonitorsFormComponent } from './components/products/monitors/monitors-form/monitors-form.component';
import { FormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { HomeComponent } from './components/home/home.component';
import { GpusComponent } from './components/products/gpus/gpus.component';
import { GpusFormComponent } from './components/products/gpus/gpus-form/gpus-form.component';

@NgModule({
  declarations: [
    AppComponent,
    MonitorsComponent,
    MonitorsFormComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    GpusComponent,
    GpusFormComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
