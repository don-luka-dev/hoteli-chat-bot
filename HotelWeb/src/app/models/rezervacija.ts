export interface Rezervacija {
  datumKreiranja: Date;
  id: string;   
  datumOd: Date;
  datumDo: Date;
  napomena: string;
  sobaId: string;
}
export interface UpsertRezervacija {
  datumOd: Date;
  datumDo: Date;
  napomena: string;
  korisnikId: number | null;
  sobaId: string;
}