import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ChatBotService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7299/api/ChatBot';

aIEndpoint(sessionId: string, message: string, urlSlike?: File) {
  const form = new FormData();
  form.append('sessionId', sessionId);
  form.append('message', message);
  if (urlSlike) form.append('urlSlike', urlSlike, urlSlike.name);

  // Important: don't set Content-Type header; HttpClient will set the boundary.
  return this.http.post<string>(this.apiUrl, form, { responseType: 'text' as 'json' });
}
}
