import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MainRouting } from './routing/main-pages.routing';



@NgModule({
  declarations: [
    DashboardComponent
  ],
  imports: [
    CommonModule,
    MainRouting
  ]
})
export class MainPagesModule { }
