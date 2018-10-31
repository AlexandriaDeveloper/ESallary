
import { Component, OnInit } from '@angular/core';
import { User } from './_models/account.model';
import { setTheme } from 'ngx-bootstrap/utils';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'SPA';
  user: User;
  ngOnInit(): void {

  }

  constructor() {

    setTheme('bs4'); // or 'bs4'

  }
}
