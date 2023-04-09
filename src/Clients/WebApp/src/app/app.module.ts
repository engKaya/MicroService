import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component'; 
import { NavbarComponent } from './layout/navbar/navbar.component';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AngularMaterialModule } from './common-modules/angular-material.module'; 
import { I18nModule } from './common-modules/i18n.module'; 
import { MainPagesModule } from './modules/main-pages/main-pages.module';
import { AuthModule } from './modules/auth/auth.module'; 
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './interceptors/httpinterceptor.interceptor';
@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent, 
  ],
  imports: [ 
    
    BrowserModule,
    NgbCollapseModule,
    AppRoutingModule, 
    AngularMaterialModule,
    BrowserAnimationsModule,
    I18nModule,
    MainPagesModule,
    AuthModule
  ],
  providers: [  
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent],
  
  schemas: []
})
export class AppModule { }
