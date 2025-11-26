import { SlikaSobe, UpsertNaslovnaSlikaSobe } from './slikaSobe';

export interface Soba {
  id: string;
  naziv: string;
  brojKreveta: number;
  cijenaNocenja: number;
  urlSlike: string;
  hotelNaziv: string;
}
export interface UpsertSoba {
  naziv: string;
  brojKreveta: number;
  cijenaNocenja: number;
  naslovnaSlika: UpsertNaslovnaSlikaSobe | null;
  hotelId: string;
  slike?: SlikaSobe[];
}

export interface SobaFilter {
  brojKreveta?: number;
  cijenaNocenjaMin?: number;
  cijenaNocenjaMax?: number;
  hotelId?: string;
}