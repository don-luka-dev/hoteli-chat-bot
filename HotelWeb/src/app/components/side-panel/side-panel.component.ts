import { Component, input, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { ChatBotService } from '../../services/chat-bot.service';

type ChatMessage = { author: 'user' | 'ai'; content: string; imageUrl?: string };

@Component({
  selector: 'app-side-panel',
  standalone: true,
  imports: [CommonModule, ButtonModule, InputTextModule],
  templateUrl: './side-panel.component.html',
})
export class SidePanelComponent {
  readonly title = input<string>();
  readonly description = input<string>();

  private readonly _visible = signal(false);
  readonly visible = this._visible.asReadonly();

  open()  { this._visible.set(true); }
  close() { this._visible.set(false); }

  constructor(private chatBotService: ChatBotService) {}

  readonly messages   = signal<ChatMessage[]>([]);
  readonly currentInput = signal('');
  readonly sessionId    = crypto.randomUUID();
  readonly isTyping     = signal(false);

  // --- NEW: file attachment state ---
  readonly selectedFile   = signal<File | null>(null);
  readonly filePreviewUrl = signal<string | null>(null);

  triggerFileInput() {
    (document.getElementById('chat-image-input') as HTMLInputElement)?.click();
  }

  onFileSelected(ev: Event) {
    const input = ev.target as HTMLInputElement;
    const file = input.files?.[0] ?? null;
    if (!file) return;

    this.selectedFile.set(file);

    // preview (data URL)
    const reader = new FileReader();
    reader.onload = e => this.filePreviewUrl.set(String(e.target?.result ?? ''));
    reader.readAsDataURL(file);
  }

  clearAttachment() {
    this.selectedFile.set(null);
    this.filePreviewUrl.set(null);
    const input = document.getElementById('chat-image-input') as HTMLInputElement | null;
    if (input) input.value = '';
  }

  sendMessage() {
    const text = this.currentInput().trim();
    const fileToSend = this.selectedFile();

    // ne šalji prazno ako nema ni teksta ni datoteke
    if (!text && !fileToSend) return;

    // prikaži korisničku poruku (uz mini oznaku ako je slika zakačena)
    this.messages.update(msgs => [
      ...msgs,
      { author: 'user', content: text || '(bez teksta)', imageUrl: this.filePreviewUrl() || undefined }
    ]);

    // reset UI
    this.currentInput.set('');
    this.isTyping.set(true);
    this.clearAttachment();

    // pošalji na backend kao multipart/form-data
    this.chatBotService.aIEndpoint(this.sessionId, text, fileToSend ?? undefined).subscribe({
      next: aiReply => {
        this.messages.update(msgs => [...msgs, { author: 'ai', content: aiReply }]);
        this.isTyping.set(false);
      },
      error: () => {
        this.messages.update(msgs => [...msgs, { author: 'ai', content: '⚠️ AI is unavailable right now.' }]);
        this.isTyping.set(false);
      },
    });
  }
}
