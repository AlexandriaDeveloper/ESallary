import { Observer } from './../_models/observer';
import { Component, OnInit } from '@angular/core';
import { Observable, Subject, interval, Subscription } from 'rxjs';
@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent implements OnInit {
  constructor() {}
  loggedIn = false;
  ngOnInit() {}
  start() {
    this.loggedIn = true;
  }
  end() {
    this.loggedIn = false;
  }
}
