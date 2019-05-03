import { browser } from 'protractor';
import {
  Component,
  OnInit,
  ViewChild,
  ViewContainerRef,
  TemplateRef,
  AfterContentInit
} from '@angular/core';

@Component({
  selector: 'app-homepage',
  templateUrl: './homePage.component.html',
  styleUrls: ['./homePage.component.css']
})
export class HomePageComponent implements OnInit, AfterContentInit {
  cards = [
    {
      link: '/emps/list',
      title: 'موظفين',
      image: './../../../assets/img/icons/man.svg',
      content: [
        { msg: 'الموظفين :20' },
        { msg: 'ذكر :10' },
        { msg: 'أنثى :10' }
      ]
    },
    {
      link: '/daily/files',
      title: 'الأستمارات',
      image: './../../../assets/img/icons/man.svg',
      content: [
        { msg: 'عدد الأقسام المسجله :20' }
      ]
    }, {
      link: '/emps/list',
      title: 'موظفين',
      image: './../../../assets/img/icons/man.svg',
      content: [
        { msg: 'الموظفين :20' },
        { msg: 'ذكر :10' },
        { msg: 'أنثى :10' }
      ]
    }
  ];

  ngOnInit() {}
  ngAfterContentInit() {}
}
