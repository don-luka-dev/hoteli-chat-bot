import { Component, inject, input, signal } from '@angular/core';

import { CommonModule } from '@angular/common';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import {
  FormBuilder,
  FormControl,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';
import { SelectModule } from 'primeng/select';
import { filter, switchMap, tap } from 'rxjs';
import { HotelStatus, UpsertHotel } from '../../models/hotel';
import { Mjesto } from '../../models/mjesto';
import { HotelService } from '../../services/hotel.service';
import { MjestoService } from '../../services/mjesto.service';

@Component({
  selector: 'app-add-hotel',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    ButtonModule,
    InputTextModule,
    SelectModule,
    MessageModule,
  ],
  templateUrl: './add-hotel.component.html',
  styleUrls: ['./add-hotel.component.css'],
})
export class AddHotelComponent {
  private readonly fb = inject(FormBuilder);
  private readonly hotelService = inject(HotelService);
  private readonly mjestoService = inject(MjestoService);

  readonly id = input<string | undefined>();

  mjesta = signal<Mjesto[]>([]);
  statusiHotela = signal<HotelStatus[]>([]);

  readonly initialHotel = input<UpsertHotel>();

  successMessage = signal<string | null>(null);
  errorMessage = signal<string | null>(null);

  form = this.fb.group({
    naziv: ['', Validators.required],
    kontaktBroj: ['', Validators.required],
    kontaktEmail: ['', [Validators.required, Validators.email]],
    adresa: ['', Validators.required],
    urlSlike: new FormControl<File | null>(null),
    mjestoId: ['', Validators.required],
    statusHotelaId: new FormControl<number | undefined>(undefined, {
      validators: [Validators.required],
    }),
  });

  constructor() {
    this.mjestoService.getAll().subscribe({
      next: (data) => this.mjesta.set(data),
      error: (err) => console.error('Greška pri dohvaćanju mjesta', err),
    });

    this.hotelService.getAllStatusi().subscribe({
      next: (data) => this.statusiHotela.set(data),
      error: (err) => console.error('Greška pri dohvaćanju statusa', err),
    });

    toObservable(this.id)
      .pipe(
        tap((e) => console.log(e)),
        takeUntilDestroyed(),
        filter((id) => !!id),
        switchMap((id) => this.hotelService.getById(id!))
      )
      .subscribe({
        next: (data) => {
          this.form.patchValue({
            naziv: data.naziv,
            kontaktBroj: data.kontaktBroj,
            kontaktEmail: data.kontaktEmail,
            adresa: data.adresa,
            mjestoId: data.mjestoId,
            statusHotelaId: data.statusHotelaId,
          });
        },
        error: (err) =>
          console.error('Greška pri dohvaćanju hotela za uređivanje', err),
      });
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0] ?? null;
    this.form.get('urlSlike')?.setValue(file);
  }

  onSubmit() {
    if (this.form.invalid) return;

    const upsertHotel: UpsertHotel = this.form.value as UpsertHotel;

    const id = this.id();

    const request$ = id
      ? this.hotelService.update(id, upsertHotel)
      : this.hotelService.add(upsertHotel);

    request$.subscribe({
      next: () => {
        this.successMessage.set(
          id ? 'Hotel uspješno ažuriran!' : 'Hotel uspješno dodan!'
        );
        this.errorMessage.set(null);
        this.form.reset();
      },
      error: (err: any) => {
        console.error(err);
        this.successMessage.set(null);
        this.errorMessage.set(
          id
            ? 'Greška prilikom ažuriranja hotela.'
            : 'Greška prilikom dodavanja hotela.'
        );
      },
    });
  }
}
