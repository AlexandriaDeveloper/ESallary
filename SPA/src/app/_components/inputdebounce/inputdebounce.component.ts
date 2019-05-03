import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  ElementRef
} from '@angular/core';
import { fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';

@Component({
  selector: 'app-inputdebounce',
  template:
    '<input type="text" class="form-control" [placeholder]="placeholder" [(ngModel)]="inputValue">'
})
export class InputdebounceComponent implements OnInit {
  @Input() placeholder: string;
  @Input() delay = 300;
  @Output() value = new EventEmitter();
  public inputValue: string;
  constructor(private elementRef: ElementRef) {}

  ngOnInit() {
    const eventStream = fromEvent(this.elementRef.nativeElement, 'keyup').pipe(
      map(() => this.inputValue),
      debounceTime(this.delay),
      distinctUntilChanged()
    );

    eventStream.subscribe(input => this.value.emit(input));
  }
}
