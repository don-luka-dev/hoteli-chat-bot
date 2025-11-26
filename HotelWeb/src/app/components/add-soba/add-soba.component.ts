import { Component, inject, input, signal } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { UpsertSoba } from '../../models/soba';
import { SobaService } from '../../services/soba.service';
import { MessageModule } from 'primeng/message';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import {
  UpsertNaslovnaSlikaSobe,
  UpsertSlikaSobe,
} from '../../models/slikaSobe';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { filter, switchMap, tap } from 'rxjs';

@Component({
  selector: 'app-add-soba',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    InputTextModule,
    ButtonModule,
    MessageModule,
  ],
  templateUrl: './add-soba.component.html',
  styleUrl: './add-soba.component.css',
})
export class AddSobaComponent {
  private readonly fb = inject(FormBuilder);
  private readonly sobaService = inject(SobaService);
  readonly hotelId = input<string>();

  successMessage = signal<string | null>(null);
  errorMessage = signal<string | null>(null);

  form = this.fb.group({
    naziv: ['', Validators.required],
    brojKreveta: [1, [Validators.required, Validators.min(1)]],
    cijenaNocenja: [0, [Validators.required, Validators.min(0)]],
    urlSlike: new FormControl<File | null>(null),
    naslovSlike: ['', Validators.required],
    opisSlike: [''],
  });


  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0] ?? null;
    this.form.get('urlSlike')?.setValue(file);
  }

  onSubmit() {
    const hotelId = this.hotelId();

    if (!hotelId) return;

    if (this.form.invalid) {
      this.errorMessage.set('Molimo ispunite sva polja ispravno.');
      return;
    }
    const upsertSoba: UpsertSoba = this.form.value as UpsertSoba;
    upsertSoba.hotelId = hotelId;

    const upsertSlikaSobe: UpsertNaslovnaSlikaSobe = {
      urlSlike: this.form.value.urlSlike,
      naslovSlike: this.form.value.naslovSlike,
      opisSlike: this.form.value.opisSlike,
    };

    upsertSoba.naslovnaSlika = upsertSlikaSobe;

    this.sobaService.add(hotelId, upsertSoba).subscribe({
      next: () => {
        this.successMessage.set('Soba uspješno dodana!');
        this.form.reset();
      },
      error: (err) => {
        console.error('Greška pri dodavanju sobe', err);
        this.errorMessage.set('Došlo je do greške pri dodavanju sobe.');
      },
    });
  }
}
