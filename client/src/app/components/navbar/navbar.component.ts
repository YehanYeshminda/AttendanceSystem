import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent implements OnInit {
  currentUser$: Observable<User | null> = of(null);

  constructor(private accountService: AccountService, private router: Router) {}

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$;
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/login');
  }
}
