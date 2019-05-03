import { TestChildComponent } from './test-child/test-child.component';
import {
  Component,
  OnInit,
  ContentChild,
  AfterContentInit,
  ViewChild,
  ViewContainerRef,
  ComponentFactoryResolver,
  ComponentRef
} from '@angular/core';

@Component({
  selector: 'app-test-parent',
  templateUrl: './test-parent.component.html',
  styleUrls: ['./test-parent.component.css']
})
export class TestParentComponent implements OnInit, AfterContentInit {
  @ViewChild('entry', { read: ViewContainerRef }) entry: ViewContainerRef;
  component: ComponentRef<TestChildComponent>;
  data = '';
  counter = 1;
  constructor(private resolver: ComponentFactoryResolver) {}

  ngOnInit() {}
  evalCheck(event) {
    console.log(event);
  }
  ngAfterContentInit() {}
  generateComponent() {
    const componentResolver = this.resolver.resolveComponentFactory(
      TestChildComponent
    );
    this.component = this.entry.createComponent(componentResolver);
    this.component.instance.subject = 'content will replace here ';
    this.component.instance.title = 'Child Component';
    this.component.instance.print.subscribe(x => (this.data = x));
  }

  destroyComponent() {
    this.component.destroy();
  }
}
