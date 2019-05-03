import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import {
  trigger,
  transition,
  query,
  animate,
  style,
  group,
  useAnimation
} from '@angular/animations';
import { bounce, bounceOutRight, fadeOutLeft, fadeInRight } from 'ngx-animate';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';


@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css'],
  animations: [
    // trigger('routeAnimation', [
    //   transition('void <=> *', [
    //     useAnimation(fadeInRight, {
    //       params: {
    //         timing: 5
    //       }
    //     })
    //   ]),
    //   transition('* <=> void', [useAnimation(fadeOutLeft, {
    //     params: {
    //       timing: 5
    //     }
    //   })])
    // ])
  ] // register the animations
})
export class LayoutComponent implements OnInit {
  modalRef: BsModalRef;
  constructor(private modalService: BsModalService) {}

  ngOnInit() {}

}
