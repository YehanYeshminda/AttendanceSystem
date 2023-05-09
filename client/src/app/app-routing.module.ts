import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { EmployeeDailyComponent } from './components/employee-daily/employee-daily.component';
import { AuthGuard } from './guards/auth.guard';
import { EmployeeMonthlyComponent } from './components/employee-monthly/employee-monthly.component';
import { EmployeeDashboardComponent } from './components/employee-dashboard/employee-dashboard.component';
import { EmployeeManualComponent } from './components/employee-manual/employee-manual.component';

const routes: Routes = [
  {
    path: 'registration',
    component: EmployeeDashboardComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'daily',
    component: EmployeeDailyComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'monthly',
    component: EmployeeMonthlyComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'manual',
    component: EmployeeManualComponent,
    canActivate: [AuthGuard],
  },
  {
    path: '',
    component: DashboardComponent,
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
