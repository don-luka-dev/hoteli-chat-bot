import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Hotel, UpsertHotel, HotelStatus, HotelFilter } from '../models/hotel';

@Injectable({
  providedIn: 'root',
})
export class HotelService {
  private readonly apiUrl = 'https://localhost:7299/api/Hoteli';

  constructor(private http: HttpClient) {}

  getAll(filter?: HotelFilter): Observable<Hotel[]> {
  let params = new HttpParams();

  if (filter?.naziv) params = params.set('Naziv', filter.naziv);
  if (filter?.mjestoId) params = params.set('MjestoId', filter.mjestoId);
  if (filter?.statusHotelaId) params = params.set('StatusHotelaId', filter.statusHotelaId.toString());

  return this.http.get<Hotel[]>(this.apiUrl, { params });
}


  getById(id: string): Observable<Hotel> {
    return this.http.get<Hotel>(`${this.apiUrl}/${id}`);
  }

  add(hotel: UpsertHotel): Observable<UpsertHotel> {
    var hotelForm = this.dtoToFormData(hotel);

    return this.http.post<UpsertHotel>(this.apiUrl, hotelForm);
  }

  update(id: string, hotel: UpsertHotel): Observable<UpsertHotel> {
    var hotelForm = this.dtoToFormData(hotel);

    return this.http.put<UpsertHotel>(`${this.apiUrl}/${id}`, hotelForm);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getAllStatusi(): Observable<HotelStatus[]> {
    return this.http.get<HotelStatus[]>(`${this.apiUrl}/statusi`);
  }

  getAllForUser(): Observable<Hotel[]> {
    return this.http.get<Hotel[]>(`${this.apiUrl}/moji`);
  }

  getUnconfirmed(): Observable<Hotel[]> {
    return this.http.get<Hotel[]>(`${this.apiUrl}/nepotvrdeni`);
  }

  confirm(id: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}/potvrda`, {});
  }

 private dtoToFormData(hotel: UpsertHotel): FormData {
  const formData = new FormData();

  formData.append('naziv', hotel.naziv ?? '');
  formData.append('kontaktBroj', hotel.kontaktBroj ?? '');
  formData.append('kontaktEmail', hotel.kontaktEmail ?? '');
  formData.append('adresa', hotel.adresa ?? '');

  // ako su brojevi, pretvori u string (FormData prima string/Blob)
  if (hotel.mjestoId != null)      formData.append('mjestoId', String(hotel.mjestoId));
  if (hotel.statusHotelaId != null)formData.append('statusHotelaId', String(hotel.statusHotelaId));

  if (hotel.urlSlike) {
    formData.append('urlSlike', hotel.urlSlike);
  }

  return formData; 
}

}
