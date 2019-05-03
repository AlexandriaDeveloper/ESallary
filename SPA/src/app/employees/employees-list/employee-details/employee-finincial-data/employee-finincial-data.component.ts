import { map, switchMap } from 'rxjs/operators';
import { FinincialAccount } from './../../../../_models/finincial-account.model';
import { EmployeeService } from './../../../../_services/employee.service';
import { ActivatedRoute } from '@angular/router';
import {
  EmployeeFinincialDto,
  EmployeeFinincialDataDto,
  EmpoloyeeAccountDto
} from './../../../../_models/employee.model';
import {
  Component,
  OnInit,
  AfterContentInit,
  AfterViewInit
} from '@angular/core';
import { FinicialAccountService } from 'src/app/_services/finicialAccount.service';

@Component({
  selector: 'app-employee-finincial-data',
  templateUrl: './employee-finincial-data.component.html',
  styleUrls: ['./employee-finincial-data.component.css']
})
export class EmployeeFinincialDataComponent
  implements OnInit, AfterContentInit, AfterViewInit {
  empFinance: EmployeeFinincialDto = new EmployeeFinincialDto();
  accounts: any[] = new Array();
  test: any[] = new Array();
  constructor(
    private router: ActivatedRoute,
    private employeeService: EmployeeService,
    private finincialAccountService: FinicialAccountService
  ) {}

  ngOnInit() {
    this.empFinance.employeeId = this.router.snapshot.params.id;
    this.empFinance.year = 2019;
    this.finincialAccountService.getAccounts().subscribe(
      x => {
        this.accounts = x;
      },
      err => {},
      () => {
        console.log(this.accounts);
      }
    );
    console.log(this.empFinance);

    this.employeeService
      .getEmpFinicial(this.router.snapshot.params.id)
      .subscribe(
        x => {
          console.log(x);
          this.empFinance.employeeFinincialDataDto = x;
        },
        err => {},
        () => {
          this.buildArray(this.accounts);
        }
      );
  }
  newRow() {
    this.empFinance.employeeFinincialDataDto.push({
      account: <EmpoloyeeAccountDto>(
        this.accounts.find(x => x.name === 'الأجر الأساسى')
      ),
      accountId: 1,
      value: 0,
      accountState: 'Credit'
    });
  }
  SubmitValue() {
    console.log(this.empFinance);

    // this.empFinance.employeeFinincialDataDto = this.empFinance.employeeFinincialDataDto.filter(
    //   x => x.value > 0
    // );

    this.employeeService
      .AddEmpFinance(this.empFinance)
      .subscribe(x => console.log(x));
  }
  ngAfterViewInit(): void {
    console.log(this.accounts);
  }
  ngAfterContentInit(): void {
    console.log(this.accounts);
  }

  buildArray(acc) {
    console.log(acc);
    this.empFinance.employeeFinincialDataDto = [
      {
        account: <EmpoloyeeAccountDto>acc.find(x => x.name === 'الأجر الأساسى'),
        accountId: <number>acc.find(x => x.name === 'الأجر الأساسى').id,
        value:
          this.empFinance.employeeFinincialDataDto.find(
            x =>
              x.accountId ===
              <number>acc.find(t => t.name === 'الأجر الأساسى').id
          ).value || 0,
        accountState:
          this.empFinance.employeeFinincialDataDto.find(
            x =>
              x.accountId ===
              <number>acc.find(x => x.name === 'الأجر الأساسى').id
          ).accountState || 'Credit'
      },
      {
        account: <EmpoloyeeAccountDto>(
          acc.find(x => x.name === 'الأساسى التأمينى')
        ),
        accountId: acc.find(x => x.name === 'الأساسى التأمينى').id,
        value:
          this.empFinance.employeeFinincialDataDto.find(
            x =>
              x.accountId ===
              <number>acc.find(t => t.name === 'الأساسى التأمينى').id
          ).value || 0,
        accountState:
          this.empFinance.employeeFinincialDataDto.find(
            x =>
              x.accountId ===
              <number>acc.find(x => x.name === 'الأساسى التأمينى').id
          ).accountState || 'Credit'
      },
      {
        account: <EmpoloyeeAccountDto>acc.find(x => x.name === 'الأجر الوظيفى'),

        accountId: acc.find(x => x.name === 'الأجر الوظيفى').id,
        value:
        this.empFinance.employeeFinincialDataDto.find(
          x =>
            x.accountId ===
            <number>acc.find(t => t.name === 'الأجر الوظيفى').id
        ).value || 0,
      accountState:
        this.empFinance.employeeFinincialDataDto.find(
          x =>
            x.accountId ===
            <number>acc.find(x => x.name  === 'الأجر الوظيفى').id
        ).accountState || 'Credit'
      },
      {
        account: <EmpoloyeeAccountDto>acc.find(x => x.name === 'الأجر المكمل'),

        accountId: acc.find(x => x.name === 'الأجر المكمل').id,
        value:
        this.empFinance.employeeFinincialDataDto.find(
          x =>
            x.accountId ===
            <number>acc.find(t => t.name === 'الأجر المكمل').id
        ).value || 0,
      accountState:
        this.empFinance.employeeFinincialDataDto.find(
          x =>
            x.accountId ===
            <number>acc.find(x => x.name  === 'الأجر المكمل').id
        ).accountState || 'Credit'
      },

      {
        account: <EmpoloyeeAccountDto>(
          acc.find(x => x.name === 'الحافز التعويضى')
        ),
        accountId: acc.find(x => x.name === 'الحافز التعويضى').id,
        value:
        this.empFinance.employeeFinincialDataDto.find(
          x =>
            x.accountId ===
            <number>acc.find(t => t.name === 'الحافز التعويضى').id
        ).value || 0,
      accountState:
        this.empFinance.employeeFinincialDataDto.find(
          x =>
            x.accountId ===
            <number>acc.find(x => x.name  === 'الحافز التعويضى').id
        ).accountState || 'Credit'
      },

      {
        account: <EmpoloyeeAccountDto>acc.find(x => x.name === 'تأمين علاجى'),
        accountId: acc.find(x => x.name === 'تأمين علاجى').id,
        value:
        this.empFinance.employeeFinincialDataDto.find(
          x =>
            x.accountId ===
            <number>acc.find(t => t.name === 'تأمين علاجى').id
        ).value || 0,
      accountState:
        this.empFinance.employeeFinincialDataDto.find(
          x =>
            x.accountId ===
            <number>acc.find(x => x.name  === 'تأمين علاجى').id
        ).accountState || 'Debit'
      }
    ];
  }
  doSelectionChanges(eve) {
    console.log(eve);
  }
  testArr() {
    console.log('clicked');
    this.test.push({ name: '1' });
  }
  print() {
    console.log(this.test);
  }
}
