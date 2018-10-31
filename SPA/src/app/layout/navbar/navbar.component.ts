import { AuthService } from './../../_services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  isCollapsed = true;
  menuCollapsed = true;
  constructor(private authService: AuthService) {}

  ngOnInit() {}
  logOut() {
    this.authService.signOut();
  }
  test($event) {
    this.isCollapsed = !$event;
  }
}
