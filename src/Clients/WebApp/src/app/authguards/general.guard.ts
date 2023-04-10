
import {   ActivatedRouteSnapshot,  RouterStateSnapshot,  Router, UrlTree} from "@angular/router";
import { Injectable } from "@angular/core";
import { ToasterService } from "../services/toaster.service";
import { AuthLoginService } from "../modules/auth/services/auth-login.service";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class GeneralAuthGuard {

  constructor(
    private authService: AuthLoginService, 
    private toastr: ToasterService, 
    private router: Router) { }

    canActivate(state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {   
      if (this.authService.isLoggedIn()) {
        return true;
      } else {
        var returnUrl = "";
        for (const iterator of state.url) {
            returnUrl += returnUrl == "" ? iterator :  "%2F" + iterator; 
        } 
        this.toastr.openToastError("Not Authorized","You are not logged in");
        this.router.navigate([`/login`], { queryParams: { returnUrl: returnUrl } });
        return false;
      }
    }

}