import { HttpStatusCode } from '@angular/common/http';

export class LoginRequestModel { 
  constructor(username: string, password: string) {
    this.UserName = username;
    this.Password = password;
  }
  UserName: string | undefined;
  Password: string | undefined;
}

export class LoginResponseModel {
  constructor() {}
  userName: string | undefined;
  token: string | undefined;
  expiresIn: number | undefined;
  rights: string[] | undefined;
  roleId: number | undefined;
  message: string | undefined;
  status: HttpStatusCode | undefined;
}
