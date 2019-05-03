import { Directive, ElementRef, Input, HostListener } from '@angular/core';

@Directive({
  selector: '[appOnlyNumber]'
})
export class OnlyNumberDirective {
  regexStr = '^[0-9]*$';
  constructor(private el: ElementRef) {}
  @Input() appOnlyNumber: boolean;
  @HostListener('keydown', ['$event']) onKeyDown(event) {
    console.log(event);
    const e = <KeyboardEvent>event;
    if (this.appOnlyNumber) {
      console.log(e);
      if (
        [
          46,
          8,
          9,
          27,
          13,
          110,
          190,
          97,
          96,
          98,
          99,
          100,
          101,
          102,
          103,
          104,
          105,
          106
        ].indexOf(e.keyCode) !== -1 ||
        // Allow: Ctrl+A
        (e.keyCode === 65 && e.ctrlKey === true) ||
        // Allow: Ctrl+C
        (e.keyCode === 67 && e.ctrlKey === true) ||
        // Allow: Ctrl+V
        (e.keyCode === 86 && e.ctrlKey === true) ||
        // Allow: Ctrl+X
        (e.keyCode === 88 && e.ctrlKey === true) ||
        // Allow: home, end, left, right
        (e.keyCode >= 35 && e.keyCode <= 39)
      ) {
        // let it happen, don't do anything
        return;
      }
      const ch = String.fromCharCode(e.keyCode);
      const regEx = new RegExp(this.regexStr);
      if (regEx.test(ch)) {
        return;
      } else {
        e.preventDefault();
      }
    }
  }
}
