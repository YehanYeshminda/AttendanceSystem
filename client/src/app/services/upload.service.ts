import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class UploadService {
  constructor(private http: HttpClient) {}

  uploadFile(formData: FormData) {
    return this.http.post<any>(
      'https://localhost:7228/api/attendance/upload-data',
      formData
    );
  }
}
