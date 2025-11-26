import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { KorisnikFilter,Korisnik } from '../models/korisnik';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class KorisnikService {
  private readonly apiUrl = 'https://localhost:7299/api/Korisnici';

  constructor(private http: HttpClient) { }

  getAll(filter?: KorisnikFilter): Observable<Korisnik[]> {
    let params = new HttpParams();

    if (filter?.korisnickoIme?.trim()) {
      params = params.set('korisnickoIme', filter.korisnickoIme.trim());
    }

    return this.http.get<Korisnik[]>(this.apiUrl, { params });
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  promoteToAdmin(id: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}/promocija`, {});
  }
}
