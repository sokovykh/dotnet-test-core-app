import { Component, Inject, SecurityContext } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DomSanitizer, SafeResourceUrl, SafeUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})

export class HomeComponent {
  public url: SafeResourceUrl;
  public documentId = 0;
  public sanitizer: DomSanitizer;
  public base_url: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, sanitizer: DomSanitizer) {
    this.sanitizer = sanitizer;
    this.Update(0);
    this.base_url = baseUrl;
  }

  public Update(id: number) {
    // this.url = this.sanitizer.bypassSecurityTrustResourceUrl('https://localhost:5001/injectablesite?id=' + id);
    // this.url = this.sanitizer.bypassSecurityTrustResourceUrl('http://localhost:5000/injectablesite?id=' + id);
    this.url = this.sanitizer.bypassSecurityTrustResourceUrl('https://display-office-app.herokuapp.com/injectablesite?id=' + id);
  }
}
