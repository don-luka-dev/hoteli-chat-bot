import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Rezervacija, UpsertRezervacija } from '../models/rezervacija';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class RezervacijaService {
  private readonly apiUrl = 'https://localhost:7299/api/sobe';//direktiva il neki kurac

  constructor(private http: HttpClient) {}

  getAll(sobaId: string): Observable<Rezervacija[]> {
    return this.http.get<Rezervacija[]>(`${this.apiUrl}/${sobaId}/rezervacije`);
  }

  getById(sobaId: string, rezervacijaId: string): Observable<Rezervacija> {
    return this.http.get<Rezervacija>(`${this.apiUrl}/${sobaId}/rezervacije/${rezervacijaId}`);
  }

  add(sobaId: string, rezervacija: UpsertRezervacija): Observable<Rezervacija> {
    return this.http.post<Rezervacija>(`${this.apiUrl}/${sobaId}/rezervacije`, rezervacija);
  }

  update(sobaId: string, rezervacijaId: string, rezervacija: UpsertRezervacija): Observable<Rezervacija> {
    return this.http.put<Rezervacija>(`${this.apiUrl}/${sobaId}/rezervacije/${rezervacijaId}`, rezervacija);
  }

  delete(sobaId: string, rezervacijaId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${sobaId}/rezervacije/${rezervacijaId}`);
  }

  getAllForUser(): Observable<Rezervacija[]> {
    return this.http.get<Rezervacija[]>(`https://localhost:7299/api/rezervacije/moje`);
  }
}
