import {
  Component,
  OnInit,
  Output,
  EventEmitter,
  ContentChild,
  AfterContentInit,
  ElementRef
} from '@angular/core';

@Component({
  selector: 'app-test-child',
  templateUrl: './test-child.component.html',
  styleUrls: ['./test-child.component.css']
})
export class TestChildComponent implements OnInit, AfterContentInit {
  title = '';
  subject = '';
  @ContentChild('insideNgContent') insideNgContent;
  @ContentChild('printBtn') printBtn: ElementRef;
  @Output()
  print: EventEmitter<string> = new EventEmitter<string>();
  constructor() {}

  ngOnInit() {}
  printData() {
    console.log('clicked');
    //this.print.emit('This Come From Child Component');
  }
  ngAfterContentInit() {
    // console.log(this.insideNgContent);
    console.log(this.printBtn);
    this.printBtn.nativeElement.onClick(console.log('boooh'));
  }
}
