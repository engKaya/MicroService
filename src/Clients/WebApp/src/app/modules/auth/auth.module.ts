import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AuthRouting } from './routing/auth.routing';
import { AngularMaterialModule } from 'src/app/common-modules/angular-material.module';
import { TranslateModule } from '@ngx-translate/core';



@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent
  ],
  imports: [
    CommonModule,
    AuthRouting,
    AngularMaterialModule,
    TranslateModule
  ]
})
export class AuthModule { }
