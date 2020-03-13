export class EmployeeList {
  id: string;
  Name: string;
  collage: string;
  department: string;
  grade: string;
  gender: string;
  paymentTypeToReturnDto: PaymentType[];
}

export class PaymentType {
  id: number;
  paymentTypeName: string;
  employeePaymentTypeToReturnDto: EmployeePaymentType[];
}
export class EmployeePaymentType {
  employeeId: string;
  paymentDataId: number;
  paymentDataName: string;
  value: string;
}
export class EmployeeToAdd {
  id: string;
  name: string;
  phone: string;
  collage: string;
  section: string;
  department: string;
  email: string;
  bankName: string;
  branchName: string;
  accountNum: string;
  address: string;
  phoneNum: string;
}
export class EmployeeDetailsModel {
  id: number;
  nationalId: string;
  name: string;
  knownAs?: string;
  gender?: string;
  phone: string;
  code?: string;
  collage: string;
  section: string;
  departmentId?: number;
  department?: DepartmentModel;
  grade: string;
  email: string;
  bankName: string;
  branchName: string;
  accountNum: string;
  address: string;
  phoneNum: string;
  bankOption: string;
  atmOption: string;
  hasATM: boolean;
  hasBank: boolean;
  hasOrder: boolean;
  hasPost: boolean;
  bank: EmployeBankModel;
  order: EmployeeOrderModel;
  post: EmployeePostModel;
}
export class EmployeBankModel {
  id: number;
  bankCode: string;
  bankName: string;
  branchCode: string;
  branchName: string;
}
export class EmployeeOrderModel {
  id: number;
  orderBankCode: number;
  orderBankName: string;
  orderBranchCode: number;
  orderBranchName: string;
  orderAccountNum: string;
}
export class EmployeePostModel {
  id: number;
  postTo: string;
  postAddress: string;
  postPhone: string;
}
export class DepartmentModel {
  id: number;
  name: string;
}
/*
 @ViewChild('name') nameInputRef: ElementRef;
  @ViewChild('id') idInputRef: ElementRef;
  @ViewChild('department') departmentInputRef: ElementRef;
  @ViewChild('collage') collageInputRef: ElementRef;
  @ViewChild('code') codeInputRef: ElementRef;
  @ViewChild('grade') gradeInputRef: ElementRef;
  @ViewChild('payment') paymentInputRef: ElementRef;

  */

export class EmpParams {
  id = '';
  name = '';
  department = '';
  collage = '';
  section = '';
  nationalId = '';
  code = '';
  grade = '';
  payment = '';
  deleted = false;
  male = true;
  female = true;
  hasATM = true;
  hasBank = true;
  paymentType = '';
  pageNumber;
  pageSize;
}
/*    public class EmployeeFinincialDto {
        public string EmployeeId { get; set; }
        public short Year { get; set; }

        public ICollection<EmployeeFinincialDataDto> EmployeeFinincialDataDto { get; set; }
        public EmployeeFinincialDto()
        {
            this.EmployeeFinincialDataDto= new Collection<EmployeeFinincialDataDto>();

        }

    }

    public class EmployeeFinincialDataDto {
        public string Key { get; set; }
        public float Value { get; set; } = 0;
        public AccountStatus AccountState { get; set; } = AccountStatus.None; //Crediti- Debit -Debit
        public string AdditionalValue { get; set; }
    }
    */

export class EmployeeFinincialDto {
  employeeId: string;
  year: number;
  employeeFinincialDataDto: EmployeeFinincialDataDto[];
}

export class EmployeeFinincialDataDto {
  account: EmpoloyeeAccountDto;
  accountId: number;
  value: number;
  accountState: 'Credit' | 'Debit' | 'None';
}
export class EmpoloyeeAccountDto {
  id: number;
  name: string;
}
