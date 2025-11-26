export interface Korisnik {
  id: number;
  username: string;
  ime: string;
  prezime: string;
  email: string;
  rola:string;
}
export interface UpsertKorisnik {
  username: string;
  ime: string;
  prezime: string;
  email: string;
  lozinka?: string;
  rolaId: number;
}

export interface KorisnikFilter {
  korisnickoIme?: string;
}

export interface LoginDto {
  username: string;
  lozinka: string;
}

export interface TokenResponseDto {
  userId: number;
  accessToken: string;
  refreshToken: string;
}

export interface RefreshTokenRequestDto {
  userId: number;
  refreshToken: string;
} 