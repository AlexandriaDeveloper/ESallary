import { CustomToastrService } from './../../_services/toastr.service';
import { AuthService } from './../../_services/auth.service';
import { AccountService } from './../../_services/account.service';
import { Login } from './../../_models/account.model';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  user = new Login();
  constructor(
    private account: AccountService,
    private auth: AuthService,
    private router: Router,
    private toast: CustomToastrService
  ) {}

  ngOnInit() {}
  login() {
    const result = this.account.loginUser(this.user).subscribe(
      x => {
        this.auth.authUser(x);
        this.router.navigate(['/home']);
      },
      err => {
        console.log(err);
        this.toast.showError('اسم المستخدم او كلمه المرور غير صحيحة ');
      },
      () => this.toast.showSuccess(`${this.user.userName} اهلا بك`)
    );
  }
}
