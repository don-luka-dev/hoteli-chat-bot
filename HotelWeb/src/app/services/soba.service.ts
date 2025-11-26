import { HttpClient,HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { Soba, UpsertSoba,SobaFilter } from '../models/soba';
import { SlikaSobe, UpsertSlikaSobe } from '../models/slikaSobe';

@Injectable({
  providedIn: 'root',
})
export class SobaService {
  private readonly apiUrl = 'https://localhost:7299/api/Hoteli';

  constructor(private http: HttpClient) {}

   getAll(filter?: SobaFilter): Observable<Soba[]> {
    let params = new HttpParams();
    if (filter?.hotelId) params = params.set('HotelId', filter.hotelId);
    if (filter?.brojKreveta != null) params = params.set('BrojKreveta', String(filter.brojKreveta));
    if (filter?.cijenaNocenjaMin != null) params = params.set('CijenaNocenjaMin', String(filter.cijenaNocenjaMin));
    if (filter?.cijenaNocenjaMax != null) params = params.set('CijenaNocenjaMax', String(filter.cijenaNocenjaMax));

    // endpoint po uzorku tvojih “slika” ruta: /api/Hoteli/sobe
    return this.http.get<Soba[]>(`https://localhost:7299/api/sobe`, { params });
  }

  getSobeByHotelId(hotelId: string): Observable<Soba[]> {
    return this.http.get<Soba[]>(`${this.apiUrl}/${hotelId}/sobe`);
  }

  getByHotelIdAndId(hotelId: string, id: string): Observable<Soba> {
    return this.http.get<Soba>(`${this.apiUrl}/${hotelId}/sobe/${id}`);
  }

   getById(id: string): Observable<Soba> {
    return this.http.get<Soba>(`https://localhost:7299/api/sobe` + `/${id}`);
  }

  add(hotelId: string, soba: UpsertSoba): Observable<UpsertSoba> {
    var sobaForm = this.dtoToFormData(soba);

    return this.http.post<UpsertSoba>(`${this.apiUrl}/${hotelId}/sobe`, sobaForm);
  }

  delete(hotelId: string, sobaId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${hotelId}/sobe/${sobaId}`);
  }

  //Slike soba

  getAllSlike(sobaId: string): Observable<SlikaSobe[]> {
    return this.http.get<SlikaSobe[]>(`${this.apiUrl}/sobe/${sobaId}/slike`);
  }

  getSlikaById(sobaId: string, slikaId: string): Observable<SlikaSobe> {
    return this.http.get<SlikaSobe>(
      `${this.apiUrl}/sobe/${sobaId}/slike/${slikaId}`
    );
  }

  addSlika(sobaId: string, dto: UpsertSlikaSobe): Observable<SlikaSobe> {
    var slikaForm = this.slikaDtoToFormData(dto);
    return this.http.post<SlikaSobe>(
      `${this.apiUrl}/sobe/${sobaId}/slike`,
      slikaForm
    );
  }

  deleteSlika(sobaId: string, slikaId: string): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/sobe/${sobaId}/slike/${slikaId}`
    );
  }

private dtoToFormData(soba: UpsertSoba): FormData {
  const formData = new FormData();

  formData.append('naziv', soba.naziv ?? '');
  formData.append('brojKreveta', soba.brojKreveta.toString());
  formData.append('cijenaNocenja', soba.cijenaNocenja.toString());
  formData.append('hotelId', soba.hotelId ?? '');

  if (soba.naslovnaSlika) {
  if (soba.naslovnaSlika.urlSlike) {
    formData.append('NaslovnaSlika.UrlSlike', soba.naslovnaSlika.urlSlike);
  }
  formData.append('NaslovnaSlika.NaslovSlike', soba.naslovnaSlika.naslovSlike ?? '');
  formData.append('NaslovnaSlika.OpisSlike', soba.naslovnaSlika.opisSlike ?? '');
}


  return formData;
}


  private slikaDtoToFormData(slika: UpsertSlikaSobe): FormData {
    const formData = new FormData();

    formData.append('sobaId', slika.sobaId);

    if (slika.urlSlike) {
      formData.append('urlSlike', slika.urlSlike); //file
    }

    formData.append('naslovSlike', slika.naslovSlike ?? '');
    formData.append('opisSlike', slika.opisSlike ?? '');

    return formData;
  }
}
