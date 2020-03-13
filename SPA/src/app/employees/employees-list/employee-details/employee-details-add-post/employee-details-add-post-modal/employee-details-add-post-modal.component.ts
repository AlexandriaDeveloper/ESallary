import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import {
  Component,
  OnInit,
  AfterViewInit,
  ViewChild,
  Input
} from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { CustomToastrService } from 'src/app/_services/toastr.service';
import { Subject } from 'rxjs';
import { NgxSelectComponent } from 'ngx-select-ex';
import { EmployeeDetailsModel } from 'src/app/_models/employee.model';

@Component({
  selector: 'app-employee-details-add-post-modal',
  templateUrl: './employee-details-add-post-modal.component.html',
  styleUrls: ['./employee-details-add-post-modal.component.css']
})
export class EmployeeDetailsAddPostModalComponent
  implements OnInit, AfterViewInit {
  @Input() emp: EmployeeDetailsModel;
  addPostForm: FormGroup;
  postList = ['البنك الاهلى', 'البريد المصرى'];
  public onClose = new Subject<any>();
  @ViewChild('postTo',{static : false}) public ngSelect: NgxSelectComponent;
  post: { postTo: string; postAddress: string; postPhone: string };
  constructor(
    public modalRef: BsModalRef,
    private toast: CustomToastrService,
    private fb: FormBuilder
  ) {}

  ngOnInit() {
    this.createForm();
  }
  ngAfterViewInit(): void {
    this.post = {
      postTo: '',
      postAddress: '',
      postPhone: ''
    };
  }
  createForm() {
    this.addPostForm = this.fb.group({
      postTo: ['', Validators.required],
      postAddress: ['', Validators.required],
      postPhone: [
        '',
        [Validators.required, Validators.minLength(11), Validators.maxLength(11)]
      ]
    });
  }
  closeModal() {
    this.modalRef.hide();
  }
  doSelect(postName) {
    console.log(this.post.postTo);
    console.log(postName);
  }
  onSubmitPost() {
    if (this.addPostForm.valid) {
      this.post = Object.assign({}, this.addPostForm.value);
      this.emp.hasPost = true;
      this.emp.post = <any>this.post;
      this.addPostForm.reset();
      this.modalRef.hide();
    }
  }
}
