import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export class ImageService {
  constructor(private http: HttpClient) {}

  static getImage(http: HttpClient, imageUrl: string): Observable<Blob> {
    return http.get('https://localhost:7228/api/employee/images/' + imageUrl, {
      responseType: 'blob',
    });
  }
}
