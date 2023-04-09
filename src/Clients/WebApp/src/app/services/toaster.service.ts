import { Injectable } from '@angular/core';
import { NgToastService } from 'ng-angular-popup';

@Injectable({
  providedIn: 'root',
})
export class ToasterService { 
  constructor(private toast: NgToastService) {}

  openToastSuccess(title: string, message: string) {
    this.toast.success({
      detail: message,
      summary: title,
      duration: 5000,
      position: 'top-right',
      sticky: true,
    });
  }

  openToastError(title: string, message: string) {
    this.toast.error({
      detail: message,
      summary: title,
      duration: 5000,
      position: 'top-right',
      sticky: true,
    });
  }

  openToast(title: string, message: string) {
    this.toast.info({
      detail: message,
      summary: title,
      duration: 5000,
      position: 'top-right',
      sticky: true,
    });
  }

  openToastWarning(title: string, message: string) {
    this.toast.warning({
      detail: message,
      summary: title,
      duration: 5000,
      position: 'top-right',
      sticky: true,
    });
  }
}
