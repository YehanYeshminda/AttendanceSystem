import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Observable, map, of, startWith } from 'rxjs';
import { Employee } from 'src/app/models/employee';
import { AttendanceService } from 'src/app/services/attendance.service';
import { EmployeeService } from 'src/app/services/employee.service';
import { UploadService } from 'src/app/services/upload.service';

@Component({
  selector: 'app-employee-manual',
  templateUrl: './employee-manual.component.html',
  styleUrls: ['./employee-manual.component.scss'],
})
export class EmployeeManualComponent implements OnInit {
  form!: FormGroup;
  employees$: Observable<Employee[]> = of([]);
  employeeIds: string[] = [];
  employeeName: string = '';
  selectedFile: File | null = null;

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private attendanceService: AttendanceService,
    private toastr: ToastrService,
    private uploadService: UploadService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.loadEmployees();
  }

  initializeForm() {
    this.form = this.fb.group({
      regNo: ['', [Validators.required]],
      enrollNo: ['', [Validators.required]],
      userName: ['', [Validators.required]],
      inTimeDate: ['', [Validators.required]],
      reason: ['', [Validators.required]],
      attendanceDate: ['', [Validators.required]],
    });

    this.form.controls['regNo'].valueChanges.subscribe({
      next: (response) => {
        if (response == null || response == undefined || response.length >= 3) {
          this.getEmployeeNameAndEnrollNo(response);
        }
      },
    });
  }

  loadEmployees() {
    this.employeeService.getEmployees().subscribe({
      next: (employees) => {
        this.employeeIds = employees.map((e) => e.regNo);
      },
    });
  }

  click() {
    const inTime = this.getTimeOnly(this.form.controls['inTimeDate'].value);
    const inTimeDate = this.getDateOnly(
      this.form.controls['attendanceDate'].value
    );
    const setDateTime = new Date(`${inTimeDate} ${inTime}:00`);
    const { date, time } = this.getDateAndTime(setDateTime);
    const setDateTimeInTime = `${date} ${time}`;

    const values = { ...this.form.value, inTime: setDateTimeInTime };

    this.attendanceService.addAttendance(values).subscribe({
      next: (response) => {
        this.toastr.success('Attendance added successfully');
        this.form.reset();
      },
    });
  }

  getEmployeeNameAndEnrollNo(regNo: string) {
    if (!regNo) return;

    this.employeeService.getEmployee(regNo).subscribe({
      next: (employee) => {
        this.employeeName = employee.username;
        this.form.patchValue({
          userName: employee.username,
          enrollNo: employee.enrollNo,
        });
      },
    });
  }

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0] as File;
  }

  onUpload(): void {
    if (this.selectedFile) {
      const formData = new FormData();
      formData.append('file', this.selectedFile);

      if (this.selectedFile) {
        this.uploadService.uploadFile(this.selectedFile).subscribe({
          next: (reponse) => {
            this.toastr.error('File upload failed');
          },
          error: (error) => {
            this.toastr.success('File uploaded successfully');
          },
        });
      }
    }
  }

  private getTimeOnly(dateString: string | undefined) {
    if (!dateString) return;

    const date = new Date(dateString);
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');
    const seconds = date.getSeconds().toString().padStart(2, '0');

    return `${hours}:${minutes}:${seconds}`;
  }

  private getDateOnly(dateString: string | undefined) {
    if (!dateString) return;

    const date = new Date(dateString);
    const year = date.getFullYear().toString().padStart(4, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  private getDateAndTime(date: Date) {
    const year = date.getFullYear();
    const month = date.getMonth() + 1; // Note: month is zero-indexed
    const day = date.getDate();
    const hours = date.getHours();
    const minutes = date.getMinutes();
    const seconds = date.getSeconds();
    return {
      date: `${year}-${month}-${day}`,
      time: `${hours}:${minutes}:${seconds}`,
    };
  }
}
