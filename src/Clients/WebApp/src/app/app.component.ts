import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  
  constructor(
    translate: TranslateService 
  ) {
      translate.addLangs(['en', 'tr']);
      translate.setDefaultLang('tr');
      const browserLang = translate.getBrowserLang() || 'tr'; 
      translate.use((browserLang.match(/en|tr/) ? browserLang : 'tr'));
    }
  


}
