import {
  Directive,
  ElementRef,
  Output,
  EventEmitter,
  HostListener,
  OnInit
} from '@angular/core';
import { Observable, fromEvent } from 'rxjs';
import { tap, delay, map, debounce } from 'rxjs/operators';

@Directive({
  selector: '[appClickOutside]'
})
export class ClickOutsideDirective implements OnInit {
  private listening: boolean;
  private globalClick: Observable<MouseEvent>;
  @Output()
  appClickOutside = new EventEmitter<object>();

  constructor(private elementRrf: ElementRef) {
    this.listening = false;
    this.appClickOutside = new EventEmitter();
  }
  @HostListener('document:click', ['$event.target'])
  // public oncClick(targetElement) {
  //   const clickedInside = this.elementRrf.nativeElement.contains(targetElement);
  //   if (!clickedInside) {
  //     this.appClickOutside.emit(targetElement);
  //   } else {
  //     this.appClickOutside.emit(targetElement);
  //   }
  // }
  ngOnInit() {
    fromEvent(document, 'click')
      .pipe(
        tap(() => (this.listening = true))
      )
      .subscribe((event: MouseEvent) => {
        this.onGlobalClick(event);
      });
  }

  onGlobalClick(event: MouseEvent) {
    if (event instanceof MouseEvent && this.listening === true) {
      if (
        this.isDescendant(this.elementRrf.nativeElement, event.target) === true
      ) {
        this.appClickOutside.emit({
          value: false,
          element: event.target
        });
      } else {
        this.appClickOutside.emit({
          value: true,
          element: event.target
        });
      }
    }
  }
  isDescendant(parent, child) {
    let node = child;
    while (node !== null) {
      if (node === parent) {
        return true;
      } else {
        node = node.parentNode;
      }
    }
    return false;
  }
}
