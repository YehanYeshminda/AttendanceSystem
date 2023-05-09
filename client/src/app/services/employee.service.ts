import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CreateEmployee, Employee } from '../models/employee';
import { Observable } from 'rxjs';
import { Attendance } from '../models/attendance';
import { SearchEmployeeOnRegNoInOutTime } from '../models/customSearches';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  baseUrl: string = 'https://localhost:7228/api/';

  constructor(private http: HttpClient) {}

  addEmployees(employee: CreateEmployee) {
    return this.http.post<CreateEmployee>(this.baseUrl + 'employee', employee);
  }

  getEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.baseUrl + 'employee');
  }

  getEmployeeBasedOnReNoInDateOutDate(
    searchTerm: SearchEmployeeOnRegNoInOutTime
  ): Observable<Attendance[]> {
    return this.http.post<Attendance[]>(
      this.baseUrl + 'employee/searchRegno',
      searchTerm
    );
  }

  getEmployee(regNo: string) {
    return this.http.get<Employee>(this.baseUrl + 'employee/' + regNo);
  }
}
