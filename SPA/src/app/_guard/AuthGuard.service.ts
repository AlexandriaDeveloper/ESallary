import { CustomToastrService } from './../_services/toastr.service';
import { Login } from './../_models/account.model';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from './../_services/auth.service';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {
  constructor(
    private authService: AuthService,
    private toaster: CustomToastrService,
    private router: Router
  ) {}

  canActivate(next: ActivatedRouteSnapshot): boolean {
   // const roles = next.firstChild.data['roles'] as Array<string>;
    // if (roles) {
    //   const match = this.authService.roleMatch(roles);
    //   if (match) {
    //     return true;
    //   } else {
    //     this.router.navigate(['members']);
    //     this.alertify.error('You are not authorized to access this area');
    //   }
    // }

    if (this.authService.loggedIn()) {
      return true;
    }

    this.toaster.showError('عفوا يجب ان تسجل دخول اولا !!');
    this.router.navigate(['/home']);
    return false;
  }
}
