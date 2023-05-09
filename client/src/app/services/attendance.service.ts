import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Attendance } from '../models/attendance';

@Injectable({
  providedIn: 'root',
})
export class AttendanceService {
  baseUrl: string = 'https://localhost:7228/api/';

  constructor(private http: HttpClient) {}

  getAttendanceBasedOnDate(date: string): Observable<Attendance[]> {
    return this.http.post<Attendance[]>(
      this.baseUrl + 'Attendance/date-based',
      { startDate: date }
    );
  }

  getAttendanceBasedOnMonth(
    inDate: string,
    endDate: string,
    regNo: string
  ): Observable<Attendance[]> {
    return this.http.post<Attendance[]>(
      this.baseUrl + 'Attendance/month-based',
      { startDate: inDate, endDate: endDate, regNo: regNo }
    );
  }

  addAttendance(attendance: Attendance) {
    return this.http.post<Attendance>(this.baseUrl + 'Attendance', attendance);
  }
}
