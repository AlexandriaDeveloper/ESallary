import { AuthService } from './../../_services/auth.service';
import { Component, OnInit, ElementRef } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  isCollapsed = true;
  menuCollapsed = true;
  isCollapsed1 = true;
  constructor(private authService: AuthService) {}

  ngOnInit() {}
  logOut() {
    this.authService.signOut();
  }
  test($event) {
    this.isCollapsed = !$event;
  }

  onClickOutside(event: object) {
    if (event && event['value'] === true) {
      this.isCollapsed1 = true;
    } else {
      this.isCollapsed1 = !this.isCollapsed1;
    }
  }
}
