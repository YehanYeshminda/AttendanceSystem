import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { Attendance } from 'src/app/models/attendance';
import { SearchEmployeeOnRegNoInOutTime } from 'src/app/models/customSearches';
import { AttendanceService } from 'src/app/services/attendance.service';
import { EmployeeService } from 'src/app/services/employee.service';
import { ImageService } from 'src/app/services/image.service';

@Component({
  selector: 'app-employee-monthly',
  templateUrl: './employee-monthly.component.html',
  styleUrls: ['./employee-monthly.component.scss'],
})
export class EmployeeMonthlyComponent implements OnInit {
  searchForm!: FormGroup;
  filterForm!: FormGroup;
  attendance$!: Observable<Attendance[]>;
  imageUrlMap = new Map<number, SafeUrl>();
  attendance: Attendance[] = [];
  username: string = '';
  imageUrl: SafeUrl = '';
  employeeIds: string[] = [];

  constructor(
    private fb: FormBuilder,
    private attendanceService: AttendanceService,
    private employeeService: EmployeeService,
    private toastr: ToastrService,
    private http: HttpClient,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.initializeFilterForm();
    this.loadEmployees();
  }

  initializeForm() {
    this.searchForm = this.fb.group({
      inTime: [''],
      endTime: [''],
      regNo: [''],
    });
  }

  initializeFilterForm() {
    this.filterForm = this.fb.group({
      searchTerm: [''],
    });
  }

  loadEmployees() {
    this.employeeService.getEmployees().subscribe({
      next: (employees) => {
        this.employeeIds = employees.map((e) => e.regNo);
      },
    });
  }

  searchEmployees() {
    const inTime = this.getDateOnly(this.searchForm.get('inTime')?.value);
    const endTime = this.getDateOnly(this.searchForm.get('endTime')?.value);

    const searchText = this.filterForm.get('searchTerm')?.value;

    if (!this.attendance$) {
      this.toastr.error('Please load the employees before filtering!');
      return;
    } else {
      if (inTime && endTime && searchText) {
        const searchTerms: SearchEmployeeOnRegNoInOutTime = {
          inTime: inTime,
          outTime: endTime,
          regNo: searchText,
        };

        this.attendance$ =
          this.employeeService.getEmployeeBasedOnReNoInDateOutDate(searchTerms);
      } else {
        this.toastr.error(
          'Missing either search term or length of search',
          'Missing data!'
        );
      }
    }
  }

  loadData() {
    const inTime = this.getDateOnly(this.searchForm.get('inTime')?.value);
    const endTime = this.getDateOnly(this.searchForm.get('endTime')?.value);
    const regNo = this.searchForm.get('regNo')?.value;

    if (inTime && endTime && regNo) {
      this.attendanceService
        .getAttendanceBasedOnMonth(inTime, endTime, regNo)
        .pipe(
          map((attendanceData: Attendance[]) => {
            const regNo = this.searchForm.get('searchEmployee')?.value;
            return regNo
              ? attendanceData.filter((att) => att.regNo === regNo)
              : attendanceData;
          })
        )
        .subscribe(
          (attendanceData: Attendance[]) => {
            this.attendance$ = of(attendanceData); // update the observable with the filtered data
            for (const attendance of attendanceData) {
              if (attendance.userName) this.username = attendance.userName;

              if (attendance.pictureUrl) {
                ImageService.getImage(
                  this.http,
                  attendance.pictureUrl
                ).subscribe({
                  next: (res) => {
                    const blobUrl = URL.createObjectURL(res);
                    const safeUrl =
                      this.sanitizer.bypassSecurityTrustUrl(blobUrl);
                    this.imageUrl = safeUrl;
                  },
                  error: (err) => {
                    console.error(err);
                    this.toastr.error(
                      'An error occurred while loading the image for ' +
                        attendance.regNo
                    );
                  },
                });
              }
            }
          },
          (error) => {
            console.error(error);
            this.toastr.error(
              'Employee with this registration number does not exist!'
            );
          }
        );
    } else {
      this.toastr.error('Missing Input Fields!');
    }
  }

  private getDateOnly(dob: string | undefined): string | undefined {
    if (!dob) return undefined;
    const theDob = new Date(dob);
    return new Date(
      theDob.setMinutes(theDob.getMinutes() - theDob.getTimezoneOffset())
    )
      .toISOString()
      .substring(0, 10);
  }
}
