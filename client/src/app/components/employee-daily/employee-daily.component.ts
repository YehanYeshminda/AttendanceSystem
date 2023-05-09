import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { Attendance } from 'src/app/models/attendance';
import { AttendanceService } from 'src/app/services/attendance.service';
import { ImageService } from 'src/app/services/image.service';

@Component({
  selector: 'app-employee-daily',
  templateUrl: './employee-daily.component.html',
  styleUrls: ['./employee-daily.component.scss'],
})
export class EmployeeDailyComponent implements OnInit {
  searchForm: FormGroup = new FormGroup({});
  attendance$: Observable<Attendance[]> | undefined;
  imageUrlMap = new Map<number, SafeUrl>();

  constructor(
    private fb: FormBuilder,
    private attendanceService: AttendanceService,
    private toastr: ToastrService,
    private http: HttpClient,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.searchForm = this.fb.group({
      searchDate: [''],
    });
  }

  loadData() {
    const dob = this.getDateOnly(this.searchForm.controls['searchDate'].value);
    if (dob) {
      this.attendance$ = this.attendanceService.getAttendanceBasedOnDate(dob);
      this.attendance$.subscribe((attendanceData) => {
        for (const attendance of attendanceData) {
          if (attendance.pictureUrl) {
            ImageService.getImage(this.http, attendance.pictureUrl).subscribe({
              next: (res) => {
                const blobUrl = URL.createObjectURL(res);
                const safeUrl = this.sanitizer.bypassSecurityTrustUrl(blobUrl);
                this.imageUrlMap.set(attendance.regNo, safeUrl);
              },
              error: (err) => {
                console.error(err);
              },
            });
          }
        }
      });
    } else {
      this.toastr.error('Please enter a date!');
    }
  }

  private getDateOnly(dob: string | undefined) {
    if (!dob) return;
    let theDob = new Date(dob);

    return new Date(
      theDob.setMinutes(theDob.getMinutes() - theDob.getTimezoneOffset())
    )
      .toISOString()
      .slice(0, 10);
  }
}
