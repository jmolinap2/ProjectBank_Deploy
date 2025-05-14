import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';

import { CreateCreditRequestComponent } from './create-credit-request/create-credit-request.component';
import { MyCreditRequestsComponent } from './my-credit-requests/my-credit-requests.component';
import { AnalystCreditRequestsComponent } from './analyst-credit-requests/analyst-credit-requests.component';

const routes = [
  {
    path: 'create',
    component: CreateCreditRequestComponent,
    canActivate: [AppRouteGuard],
  },
  {
    path: 'mine',
    component: MyCreditRequestsComponent,
    canActivate: [AppRouteGuard],
  },
  {
  path: 'analyst',
  component: AnalystCreditRequestsComponent,
  canActivate: [AppRouteGuard],
  data: { permission: 'Pages.Analyst' }
  },
  {
  path: 'create/:id',
  component: CreateCreditRequestComponent,
  canActivate: [AppRouteGuard]
}

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreditRequestRoutingModule {}
