import { Component, Input, Output, EventEmitter, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SobaService } from '../../services/soba.service';
import { UpsertSlikaSobe } from '../../models/slikaSobe';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'app-add-slika-sobe',
  standalone: true,
  imports: [CommonModule, FormsModule, ButtonModule, InputTextModule],
  templateUrl: './add-slika-sobe.component.html'
})
export class AddSlikaSobeComponent {
  private readonly sobaService = inject(SobaService);

  @Input({ required: true }) sobaId!: string;
  @Output() uploaded = new EventEmitter<void>();

  file = signal<File | null>(null);
  naslov = signal<string>('');
  opis = signal<string>('');
  loading = signal<boolean>(false);
  error = signal<string | null>(null);

  onFileSelected(ev: Event) {
    const input = ev.target as HTMLInputElement;
    this.file.set(input.files?.[0] ?? null);
    this.error.set(null);
  }

  upload() {
    const f = this.file();
    if (!f) {
      this.error.set('Odaberi sliku.');
      return;
    }

    const dto: UpsertSlikaSobe = {
      sobaId: this.sobaId,
      urlSlike: f,
      naslovSlike: this.naslov() || null,
      opisSlike: this.opis() || null
    };

    this.loading.set(true);
    this.error.set(null);

    this.sobaService.addSlika(this.sobaId, dto).subscribe({
      next: _ => {
        this.loading.set(false);
        this.file.set(null);
        this.naslov.set('');
        this.opis.set('');
        this.uploaded.emit(); // javi parentu da refresha podatke
      },
      error: err => {
        console.error(err);
        this.loading.set(false);
        this.error.set('Greška pri učitavanju slike.');
      }
    });
  }
}
