import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router'; 
import { DashboardComponent } from '../dashboard/dashboard.component';
import { CartComponent } from '../cart/cart.component';
import { GeneralAuthGuard } from 'src/app/authguards/general.guard';

const routes: Routes = [
    {
        path: '',
        component: DashboardComponent
    },
    {
        path: 'cart',
        component: CartComponent,
        canActivate: [GeneralAuthGuard]
    }
  ];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MainRouting { }