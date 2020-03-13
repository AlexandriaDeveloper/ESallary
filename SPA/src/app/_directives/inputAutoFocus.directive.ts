import {
  Directive,
  ElementRef,
  Renderer2,

  AfterViewInit,
  OnDestroy
} from '@angular/core';
import { Subscription, fromEvent } from 'rxjs';

@Directive({
  selector: '[appInputAutoFocus]'
})
export class InputAutoFocusDirective implements AfterViewInit, OnDestroy {
  btn: ElementRef;
  inputText: ElementRef;
  btnSubcribe: Subscription;
  textSubcribe: Subscription;
  constructor(private el: ElementRef, private renderer: Renderer2) {}
  ngAfterViewInit(): void {
    const btn2 = this.el.nativeElement.querySelector('button');
    const inputText = this.el.nativeElement.querySelector('input');
    const btn$ = fromEvent(btn2, 'click');
    const text$ = fromEvent(inputText, 'blur');

    this.btnSubcribe = btn$.subscribe(x => {
      inputText.disabled = !inputText.disabled;

      if (inputText.disabled === false) {
        inputText.focus();
        inputText.classList.add('texteditable');
      } else {
        inputText.disabled = true;
        inputText.classList.remove('texteditable');
      }
    });
    this.textSubcribe = text$.subscribe(x => {
      if (inputText.disabled === false) {
        inputText.disabled = true;
         inputText.classList.remove('texteditable');
      }
      console.log('blured');
    });
  }

  ngOnDestroy(): void {
    this.btnSubcribe.unsubscribe();
  }
  
}
