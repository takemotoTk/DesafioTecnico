import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './user/register.component';
import { PeopleDashboardComponent } from './people-dashboard/people-dashboard.component';
import { AuthGuard } from './guards/auth.guard';
import { PersonFormComponent } from './people-dashboard/person/person-form.component';
import { VaccinesDashboardComponent } from './vaccines-dashboard/vaccines-dashboard.component';
import { VaccineFormComponent } from './vaccines-dashboard/vaccine-form/vaccine-form.component';
import { VaccinationCardComponent } from './vaccination-card/vaccination-card.component';
import { SearchVaccinationCardComponent } from './vaccination-card/search-vaccination-card/search-vaccination-card.component';


export const routes: Routes = [
  { path: '', redirectTo: 'people', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent},
  { path: 'people', component: PeopleDashboardComponent, canActivate: [AuthGuard] },
  { path: 'person/add', component: PersonFormComponent },
  { path: 'person/:id', component: PersonFormComponent },
  { path: 'vaccines', component: VaccinesDashboardComponent },
  { path: 'vaccine/add', component: VaccineFormComponent },
  { path: 'vaccine/:id', component: VaccineFormComponent },

  { path: 'search-vaccination-card', component: SearchVaccinationCardComponent },
  { path: 'vaccination-card/:idPerson', component: VaccinationCardComponent },
  { path: '', redirectTo: 'search-vaccination-card', pathMatch: 'full' },
  { path: '**', redirectTo: 'search-vaccination-card' },


  { path: '**', redirectTo: 'login' }
];