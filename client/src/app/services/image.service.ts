import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ImageService {
  constructor(private http: HttpClient) {}

  static getImage(http: HttpClient, imageUrl: string): Observable<Blob> {
    return http.get('http://aruna007-002-site30.etempurl.com/api/' + imageUrl, {
      responseType: 'blob',
    });
  }
}
