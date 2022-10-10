import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NewCustomerComponent } from 'features/customer/components/newcustomer/newcustomer.component';

import { CustomerComponent } from './features/invoice/components/customer/customer.component';
import { StepperComponent } from './features/invoice/components/stepper/stepper.component';
import { InvoicesComponent } from './features/invoices/components/invoices/invoices.component';
import { RegisterComponent } from './features/organisation/components/register/register.component';
import { HomeComponent } from 'core/components/home/home.component';

import { AuthorizeGuard } from 'core/api-authorization/guard/authorize.guard';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'customer', component: CustomerComponent, canActivate: [AuthorizeGuard]},
  { path: 'stepper', component: StepperComponent, canActivate: [AuthorizeGuard]},
  { path: 'invoices', component: InvoicesComponent, canActivate: [AuthorizeGuard]},
  { path: 'organisation', component: RegisterComponent, canActivate: [AuthorizeGuard]},
  { path: 'invoicing', component: InvoicesComponent, pathMatch: 'full', canActivate: [AuthorizeGuard]},
  { path: 'newcustomer', component: NewCustomerComponent, canActivate: [AuthorizeGuard]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
