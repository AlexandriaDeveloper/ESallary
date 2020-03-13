import { Pipe, PipeTransform } from '@angular/core';


@Pipe({
  name: 'searchFilter'
})
export class SearchPipe implements PipeTransform {
  transform(
    items: any,
    value?: any,
    value2?: any,
    value3?: any,
    value4?: any,
    value5?: any,
    value6?: any,
    value7?: any
  ): any {
    let result = items;
    if (!items) {
      return [];
    }
    if (
      value === '' &&
      value2 === '' &&
      value3 === '' &&
      value4 === '' &&
      value5 === '' &&
      value6 === '' &&
      value7 === ''
    ) {
      return items;
    }

    if (value !== '') {
      result = result.filter(it => it.employeeData.name.startsWith(value));
    }
    if (value2 !== '') {
      result = result.filter(it => {
        return this.checkNestedNullMethod(it, 'code', value2);
      });
    }
    if (value3 !== '') {
      result = result.filter(it => {
        return this.checkNullMethod(it, 'selectedPaymentMethod', value3);
      });
    }
    if (value4 !== '') {
      result = result.filter(it => {
        return this.checkNestedNullMethod(it, 'collage', value4);
      });
    }
    if (value5 !== '') {

      result = result.filter(it => {
        if (it.employeeData.department !== null) {
          return it.employeeData.department.name.toString().includes(value5);
        }
      });
    }
    if (value6 !== '') {
      result = result.filter(it => {
        return this.checkNestedNullMethod(it, 'grade', value6);
      });
    }
    if (value7 !== '') {
      result = result.filter(it => {
        return this.checkNullMethod(it, 'net', value7);
      });
    }
    return result;
  }

  private checkNullMethod(data: any, prop: string, value) {
    if (data[prop] !== null) {
      return data[prop].toString().includes(value);
    }
  }
    private checkNestedNullMethod(data: any, prop: string, value) {
      if (data.employeeData[prop] !== null) {
        return data.employeeData[prop].toString().includes(value);
      }
  }
}
