import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { EncryptionUtil } from 'src/app/helpers/encrptFunctions';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup = new FormGroup({});
  currentUser$: Observable<User | null> = of(null);

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private accountService: AccountService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]],
    });
  }

  login() {
    const regNoEmpControl = this.loginForm.get('password');
    const plainPassword = regNoEmpControl?.value;
    const encryptedPassword = EncryptionUtil.encryptData(plainPassword);

    this.accountService
      .login({ ...this.loginForm.value, password: encryptedPassword })
      .subscribe({
        next: (user) => {
          this.toastr.success(`Login Success!`);
          this.router.navigateByUrl('/');
          this.loginForm.reset();
        },
        error: (err) => {
          this.toastr.error('Invalid Creadentials!');
        },
      });
  }
}
