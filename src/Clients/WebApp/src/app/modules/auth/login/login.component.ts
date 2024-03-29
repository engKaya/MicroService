import { Component, Input } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { LoginRequestModel } from '../models/loginRequest.model';
import { AuthLoginService } from '../services/auth-login.service';
import { Observable } from 'rxjs';
import { ToasterService } from 'src/app/services/toaster.service'; 
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router'; 


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent {
  IsLoading$: Observable<boolean>; 
  returnUrl: string = '';
  constructor(
    private translate: TranslateService,
    private toast: ToasterService,
    private authLoginService: AuthLoginService,
    private router: Router,
    private route: ActivatedRoute
    ) { 
    this.IsLoading$ =  this.authLoginService.IsLoading$;   
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
   }



  form: FormGroup = new FormGroup({
    username: new FormControl('',
      [
        Validators.required,
      ]),
    password: new FormControl('',
      [
        Validators.required,
      ]
    ),
    rememberMe: new FormControl(false)
  });

  submit() {
    if (this.form.valid) {
      var model = new LoginRequestModel(this.form.value.username, this.form.value.password); 
      this.authLoginService.login(model).then((response) => {  
        if(response.Status !== 200) {
          this.error = this.translate.instant(`ERROR_CODES.LOGIN.${response.Status}`);
          return;
        }

        this.router.navigate([this.returnUrl.replace("%2F", "/")]);
      })
    } else {
      this.error = this.translate.instant('AUTH.FORM_ERRORS');
    }
  }
  
  error: string = '';
  getErrorMessage(control: string) {
    if (control==="username" &&this.form.controls[control].hasError('required')) {
      return this.translate.instant('AUTH.USERNAME_REQUIRED');
    };
    if (control==="password" && this.form.controls[control].hasError('required')) {
      return this.translate.instant('AUTH.PASSWORD_REQUIRED');
    };
  }
}
