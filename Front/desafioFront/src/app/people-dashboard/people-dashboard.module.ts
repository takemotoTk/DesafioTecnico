import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PeopleDashboardComponent } from './people-dashboard.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [PeopleDashboardComponent],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports: [PeopleDashboardComponent]
})
export class PeopleDashboardModule {}
