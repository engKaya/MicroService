import { ChangeDetectorRef, Component } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthLoginService } from 'src/app/modules/auth/services/auth-login.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: []
})
export class NavbarComponent { 
   
  IsLogged: boolean = false;
  userName: string = '';

  constructor(
    private authLoginService: AuthLoginService,
    private ref : ChangeDetectorRef
  ) {  
    this.authLoginService.IsLoggedIn$.subscribe((isLogged: boolean) => {
      this.IsLogged = isLogged;
      this.ref.detectChanges();
    });
    this.authLoginService.UserName$.subscribe((userName: string) => {
      this.userName = userName;
      this.ref.detectChanges();
    });
  }

  ngOnInit(): void { 
    this.IsLogged = this.authLoginService.IsLogged; 
    this.userName = this.authLoginService.userName; 
    this.ref.detectChanges();
  }
  
}
