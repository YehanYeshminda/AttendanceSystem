import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { EmployeeRegistrationComponent } from '../employee-registration/employee-registration.component';
import { Employee } from 'src/app/models/employee';
import { Observable } from 'rxjs';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { ToastrService } from 'ngx-toastr';
import { HttpClient } from '@angular/common/http';
import { EmployeeService } from 'src/app/services/employee.service';
import { ImageService } from 'src/app/services/image.service';

@Component({
  selector: 'app-employee-dashboard',
  templateUrl: './employee-dashboard.component.html',
  styleUrls: ['./employee-dashboard.component.scss'],
})
export class EmployeeDashboardComponent implements OnInit {
  bsModalRef: BsModalRef | undefined;
  employee$: Observable<Employee[]> | undefined;
  imageUrlMap = new Map<string, SafeUrl>();

  constructor(
    private modalService: BsModalService,
    private toastr: ToastrService,
    private http: HttpClient,
    private sanitizer: DomSanitizer,
    private employeeService: EmployeeService
  ) {}

  ngOnInit(): void {
    this.loadEmployees();
  }

  openModalWithComponent() {
    this.bsModalRef = this.modalService.show(EmployeeRegistrationComponent);

    this.bsModalRef?.onHidden?.subscribe(() => {
      this.loadEmployees();
    });
  }

  loadEmployees() {
    this.employee$ = this.employeeService.getEmployees();

    this.employee$.subscribe((employees) => {
      for (const employee of employees) {
        if (employee.pictureUrl) {
          ImageService.getImage(this.http, employee.pictureUrl).subscribe({
            next: (res) => {
              if (res instanceof Blob) {
                const blobUrl = URL.createObjectURL(res);
                const safeUrl = this.sanitizer.bypassSecurityTrustUrl(blobUrl);
                this.imageUrlMap.set(employee.regNo, safeUrl);
              } else {
                console.log('Invalid response type, expected Blob');
              }
            },
            error: (err) => {
              console.error(err);
            },
          });
        }
      }
    });
  }
}
