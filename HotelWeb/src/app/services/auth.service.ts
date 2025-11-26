import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  LoginDto,
  TokenResponseDto,
  UpsertKorisnik,
  RefreshTokenRequestDto,
  Korisnik,
} from '../models/korisnik';
import { jwtDecode } from 'jwt-decode';
import { Roles } from '../enums/Roles';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = 'https://localhost:7299/api/Auth';

  constructor(private http: HttpClient) {}

  register(korisnik: UpsertKorisnik): Observable<Korisnik> {
    return this.http.post<Korisnik>(`${this.apiUrl}/register`, korisnik);
  }

  login(request: LoginDto): Observable<TokenResponseDto> {
    return this.http.post<TokenResponseDto>(`${this.apiUrl}/login`, request);
  }

  logout(): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/logout`, {});
  }
  
  refreshTokens(request: RefreshTokenRequestDto): Observable<TokenResponseDto> {
    return this.http.post<TokenResponseDto>(
      `${this.apiUrl}/refresh-tokens`,
      request
    );
  }

  getAccessToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  getDecodedToken(): any | null {
    const token = this.getAccessToken();
    if (!token) return null;

    try {
      return jwtDecode(token);
    } catch {
      return null;
    }
  }
  getNormalizedToken(): any | null {
    const decoded = this.getDecodedToken();
    if (!decoded) return null;

    return {
      name: decoded[
        'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'
      ],
      givenName:
        decoded[
          'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname'
        ],
      userId:
        decoded[
          'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
        ],
      role: decoded[
        'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
      ],
      email:
        decoded[
          'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'
        ],
      exp: decoded.exp,
      iss: decoded.iss,
      aud: decoded.aud,
    };
  }
  getUserId(): number | null {
    const normalized = this.getNormalizedToken();
    return normalized ? +normalized.userId : null;
  }

  getUserRole(): string | undefined {
    const normalized = this.getNormalizedToken();
    return normalized ? normalized.role : undefined;
  }

  getUsername(): string | null {
    const normalized = this.getNormalizedToken();
    return normalized ? normalized.name : null;
  }

  getFullName(): string | null {
    const normalized = this.getNormalizedToken();
    return normalized ? normalized.givenName : null;
  }

  getEmail(): string | null {
    const normalized = this.getNormalizedToken();
    return normalized ? normalized.email : null;
  }

   isAdmin(): boolean {
    const role = this.getUserRole();
    return role === Roles.Admin || role === Roles.SuperAdmin;
  }

  isSuperAdmin(): boolean {
    const role = this.getUserRole();
    return role === Roles.SuperAdmin;
  }

  hasRole(...roles: string[]): boolean {
    const role = this.getUserRole();
    return !!role && roles.includes(role);
  }
}
