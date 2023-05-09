import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class UploadService {
  constructor(private http: HttpClient) {}

  uploadFile(file: File) {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<any>(
      'https://localhost:7228/api/attendance/upload-data',
      formData
    );
  }
}
