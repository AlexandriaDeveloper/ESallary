import { SharedService } from './_services/shared.service';
import { ComponentsModule } from './components.module';
import { OnlyNumberDirective } from './_directives/onlyNumber.directive';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { JwtModule } from '@auth0/angular-jwt';

export function tokenGetter() {
  return localStorage.getItem('token');
}
@NgModule({
  imports: [
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ['localhost:5000'],
        blacklistedRoutes: ['localhost:5000/api/auth']
      }
    }),
    CommonModule,
    ComponentsModule
  ],
  declarations: [

  ],
  exports: [


  ],
  providers: [SharedService],
  entryComponents: []
})
export class SharedModule {}
