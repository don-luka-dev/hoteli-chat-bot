export interface Hotel {
  id: string;
  naziv: string;
  kontaktBroj: string;
  kontaktEmail: string;
  adresa: string;
  urlSlike: string;
  mjestoId?: string;
  mjestoNaziv?: string;
  statusHotelaId?: number;
  statusHotela?: string;
  upraviteljIds?: number[];
}
export interface UpsertHotel {
  naziv: string;
  kontaktBroj: string;
  kontaktEmail: string;
  adresa: string;
  urlSlike?: File;
  mjestoId?: string;
  statusHotelaId?: number;
  upraviteljIds?: number[];
}
export interface HotelStatus {
  id: number;
  naziv: string;
}

export interface HotelFilter {
  mjestoId?: string;
  statusHotelaId?: number;
  naziv?: string;
}
