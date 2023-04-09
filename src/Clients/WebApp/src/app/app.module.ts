import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component'; 
import { NavbarComponent } from './layout/navbar/navbar.component';
import { NgbCollapseModule } from '@ng-bootstrap/ng-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AngularMaterialModule } from './common-modules/angular-material.module'; 
import { I18nModule } from './common-modules/i18n.module'; 
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
  ],
  providers: [  
  ],
  bootstrap: [AppComponent],
  
  schemas: []
})
export class AppModule { }
