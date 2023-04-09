import { Component, Input } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { LoginRequestModel } from '../models/loginRequest.model';
import { AuthLoginService } from '../services/auth-login.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  IsLoading$: Observable<boolean>; 
  
  constructor(
    private translate: TranslateService,
    private authLoginService: AuthLoginService
  ) { 
    this.IsLoading$ = this.authLoginService.IsLoading$;   
   }



  form: FormGroup = new FormGroup({
    username: new FormControl(''),
    password: new FormControl(''),
  });

  submit() {
    if (this.form.valid) {
      var model = new LoginRequestModel(this.form.value.username, this.form.value.password);
      debugger
      this.authLoginService.login(model).then((response) => {
        console.log(response);
      })
    } else {
      this.error = this.translate.instant('AUTH.FORM_INVALID');
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
