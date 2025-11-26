export interface SlikaSobe {
  id: string;
  naslovSlike: string;
  urlSlike: string;
  opisSlike: string;
  sobaId?: string;
  sobaNaziv?: string;
}
export interface UpsertSlikaSobe {
  naslovSlike?: string | null;
  urlSlike?: File | null;
  opisSlike?: string | null;
  sobaId: string;
}
export interface UpsertNaslovnaSlikaSobe {
  naslovSlike?: string | null;
  urlSlike?: File | null;
  opisSlike?: string | null;
}


