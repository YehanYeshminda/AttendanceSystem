import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { InputFormComponent } from './forms/input-form/input-form.component';
import { DateFormComponent } from './forms/date-form/date-form.component';
import { SharedModule } from './modules/shared.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { LoginComponent } from './components/login/login.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { EmployeeRegistrationComponent } from './components/employee-registration/employee-registration.component';
import { EmployeeDailyComponent } from './components/employee-daily/employee-daily.component';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { EmployeeMonthlyComponent } from './components/employee-monthly/employee-monthly.component';
import { EmployeeDashboardComponent } from './components/employee-dashboard/employee-dashboard.component';
import { EmployeeManualComponent } from './components/employee-manual/employee-manual.component';
import { TypeaheadFormComponent } from './forms/typeahead-form/typeahead-form.component';

@NgModule({
  declarations: [
    AppComponent,
    InputFormComponent,
    DateFormComponent,
    LoginComponent,
    DashboardComponent,
    NavbarComponent,
    EmployeeRegistrationComponent,
    EmployeeDailyComponent,
    EmployeeMonthlyComponent,
    EmployeeDashboardComponent,
    EmployeeManualComponent,
    TypeaheadFormComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
