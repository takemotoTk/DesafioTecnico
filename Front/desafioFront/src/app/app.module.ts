// src/app/app.module.ts
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { LoaderComponent } from './loader/loader.component';
import { RegisterComponent } from './user/register.component';
import { PeopleDashboardComponent } from './people-dashboard/people-dashboard.component';

import { AuthInterceptor } from './services/auth.interceptor';
import { LoadingInterceptor } from './services/loading.interceptor';
import { ErrorInterceptor } from './services/error.interceptor';
import { routes } from './app.routes';
import { PersonFormComponent } from './people-dashboard/person/person-form.component';
import { VaccineFormComponent } from './vaccines-dashboard/vaccine-form/vaccine-form.component';
import { VaccinesDashboardComponent } from './vaccines-dashboard/vaccines-dashboard.component';
import { SearchVaccinationCardComponent } from './vaccination-card/search-vaccination-card/search-vaccination-card.component';
import { VaccinationCardComponent } from './vaccination-card/vaccination-card.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    PeopleDashboardComponent,
    PersonFormComponent,
    VaccinesDashboardComponent,
    VaccineFormComponent,
    SearchVaccinationCardComponent,
    VaccinationCardComponent,

    LoaderComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule.forRoot(routes)
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoadingInterceptor,
      multi: true
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
