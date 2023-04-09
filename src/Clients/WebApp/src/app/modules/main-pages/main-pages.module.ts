import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MainRouting } from './routing/main-pages.routing';
import { AngularMaterialModule } from 'src/app/common-modules/angular-material.module';
import { TranslateModule } from '@ngx-translate/core';



@NgModule({
  declarations: [
    DashboardComponent
  ],
  imports: [
    AngularMaterialModule,
    CommonModule,
    MainRouting,
    TranslateModule
  ]
})
export class MainPagesModule { }
