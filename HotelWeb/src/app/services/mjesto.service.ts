import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Mjesto } from '../models/mjesto';

@Injectable({
  providedIn: 'root'
})
export class MjestoService {
  private readonly apiUrl = 'https://localhost:7299/api/Mjesta';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Mjesto[]> {
    return this.http.get<Mjesto[]>(this.apiUrl);
  }

  getById(id: string): Observable<Mjesto> {
    return this.http.get<Mjesto>(`${this.apiUrl}/${id}`);
  }
}
