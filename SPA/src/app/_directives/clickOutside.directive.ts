import {
  Directive,
  ElementRef,
  Output,
  EventEmitter,
  HostListener,
  OnInit
} from '@angular/core';
import { Observable, fromEvent } from 'rxjs';
import { tap } from 'rxjs/operators';

@Directive({
  selector: '[appClickOutside]'
})
export class ClickOutsideDirective implements OnInit {
  private listening: boolean;
  @Output()
  appClickOutside = new EventEmitter<Object>();

  constructor(private elementRrf: ElementRef) {
    this.listening = false;
    // this.clickOutside = new EventEmitter<Object>();
  }
  @HostListener('document:click', ['$event.target'])
  public oncClick(targetElement) {
    const clickedInside = this.elementRrf.nativeElement.contains(targetElement);
    if (!clickedInside) {
      this.appClickOutside.emit(false);
    } else {
      this.appClickOutside.emit(true);
    }
  }
  ngOnInit() {
    fromEvent(document, 'click').subscribe((event: MouseEvent) => {
      this.onGlobalClick(event);
    });
  }

  onGlobalClick(event: MouseEvent) {
    if (event instanceof MouseEvent && this.listening === true) {
      if (
        this.isDescendant(this.elementRrf.nativeElement, event.target) === true
      ) {
        console.log(event);

        this.appClickOutside.emit({
          value: false
        });
      } else {
        console.log(event);
        this.appClickOutside.emit({
          value: true
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
