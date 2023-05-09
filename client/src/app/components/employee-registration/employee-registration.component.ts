import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ImageService } from 'src/app/services/image.service';
import { ToastrService } from 'ngx-toastr';
import { BsModalService } from 'ngx-bootstrap/modal';

interface CreateEmployee {
  regNo: string;
  enrollNo: string;
  username: string;
  designation: string;
  dateOfJoin: Date;
  dateOfBirth: Date;
  status: number;
  file: File;
}

@Component({
  selector: 'app-employee-registration',
  templateUrl: './employee-registration.component.html',
  styleUrls: ['./employee-registration.component.scss'],
})
export class EmployeeRegistrationComponent implements OnInit {
  employeeForm: FormGroup = new FormGroup({});
  selectedFile: File | undefined;
  imageUrl = '';

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private toastr: ToastrService,
    private bsModelRef: BsModalService
  ) {}

  ngOnInit(): void {
    this.initializeForm();

    ImageService.getImage(this.http, this.imageUrl).subscribe(
      (imageData) => {
        const imageUrl = URL.createObjectURL(imageData);
        this.imageUrl = imageUrl;
      },
      (error) => {
        console.error(error);
      }
    );
  }

  initializeForm() {
    this.employeeForm = this.fb.group({
      regNo: ['', [Validators.required]],
      enrollNo: ['', [Validators.required]],
      username: ['', [Validators.required]],
      designation: ['', [Validators.required]],
      dateOfJoin: ['', [Validators.required]],
      dateOfBirth: ['', [Validators.required]],
      status: ['', [Validators.required]],
    });
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  createEmployee() {
    if (this.employeeForm.invalid || !this.selectedFile) {
      return;
    }

    const dob = this.getDateOnly(
      this.employeeForm.controls['dateOfBirth'].value
    );
    const doj = this.getDateOnly(
      this.employeeForm.controls['dateOfJoin'].value
    );

    const formData = new FormData();
    formData.append('regNo', this.employeeForm.get('regNo')?.value);
    formData.append('enrollNo', this.employeeForm.get('enrollNo')?.value);
    formData.append('username', this.employeeForm.get('username')?.value);
    formData.append('designation', this.employeeForm.get('designation')?.value);
    formData.append('dateOfJoin', this.employeeForm.get('dateOfJoin')?.value);
    formData.append('dateOfBirth', this.employeeForm.get('dateOfBirth')?.value);
    formData.append('status', this.employeeForm.get('status')?.value);
    formData.append('file', this.selectedFile);

    this.http.post('https://localhost:7228/api/employee', formData).subscribe({
      next: (response) => {
        this.toastr.success('Employee succesfully Created!');
        this.bsModelRef.hide();
      },
      error: (err) => {
        this.toastr.error('Unable to create employee');
      },
    });
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
