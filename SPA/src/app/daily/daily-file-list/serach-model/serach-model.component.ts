import { Component, OnInit } from '@angular/core';
import { NgForm, FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { SharedService } from 'src/app/_services/shared.service';
import { BsModalRef } from 'ngx-bootstrap/modal';

import { DailyService } from 'src/app/_services/daily.service';

@Component({
  selector: 'app-serach-model',
  templateUrl: './serach-model.component.html',
  styleUrls: ['./serach-model.component.css']
})
export class SerachModelComponent implements OnInit {
  collageName: string[];
  param = {
    collageName: [
      { name: 'كلية الطب ', selected: false },
      { name: 'كلية الصيدله ', selected: true },
      { name: 'كلية طب الأسنان ', selected: false }
    ],
    fileTypeList: []
  };
  typeList: any;

  serachFileForm: FormGroup;
  constructor(
    private fb: FormBuilder,
    public modalRef: BsModalRef,
    private shared: SharedService,
    private dailyService: DailyService
  ) {}

  ngOnInit() {
    // this.collageName = this.shared.collageName();

    // this.collageName.forEach(element => {
    //   this.param.collageName.push({ name: element, selected: false });
    // });
    this.dailyService.getFileTypeList().subscribe(x => (this.typeList = x));

    this.serachFileForm = this.fb.group({
      collagesName: this.buildSkills()
    });
  }
  onSubmit(value) {
    console.log(value);

    const formValue = Object.assign(this.serachFileForm, value, {
      collagesName: value.collagesName.map((s, i) => {
        return {
          name: this.param.collageName[i].name,
          selected: s
        };
      })
    });

    this.serachFileForm.value.collagesName = formValue;
    console.log(this.serachFileForm);
  }
  closeForm() {
    this.modalRef.hide();
  }
  buildSkills() {
    const arr = this.param.collageName.map(collageName => {
      return this.fb.control(collageName.selected);
    });

    return <FormArray>this.fb.array(arr);
  }
  get collagesName() {
    return <FormArray>this.serachFileForm.get('collagesName');
  }
}
