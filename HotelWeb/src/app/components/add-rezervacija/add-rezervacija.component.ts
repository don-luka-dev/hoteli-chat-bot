import { Component, inject, input, signal } from '@angular/core';
import { RezervacijaService } from '../../services/rezervacija.service';
import {
  FormBuilder,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CalendarModule } from 'primeng/calendar';
import { CommonModule } from '@angular/common';
import { UpsertRezervacija } from '../../models/rezervacija';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-add-rezervacija',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    CalendarModule,
    ButtonModule,
    InputTextModule,
  ],
  templateUrl: './add-rezervacija.component.html',
  styleUrl: './add-rezervacija.component.css',
})
export class AddRezervacijaComponent {
  private readonly authService = inject(AuthService);
  private readonly rezervacijaService = inject(RezervacijaService);
  private readonly fb = inject(FormBuilder);

  readonly sobaId = input<string>();

  successMessage = signal<string | null>(null);
  errorMessage = signal<string | null>(null);

  form = this.fb.group({
    napomena: ['', Validators.required],
    datumOdDo: this.fb.control<Date[] | null>(null, Validators.required),
  });

  constructor() {
    this.form.patchValue({
      datumOdDo: [new Date(), new Date()],
    });
  }

  onSubmit() {
    const sobaId = this.sobaId();
    const korisnikId = this.authService.getUserId();

    if (!sobaId) return;

    const { napomena, datumOdDo } = this.form.value;

    if (!napomena || !datumOdDo || datumOdDo.length < 2) {
      this.errorMessage.set('Molimo ispunite sva polja ispravno.');
      return;
    }

    const [datumOd, datumDo] = datumOdDo;

    const upsertRezervacija: UpsertRezervacija = {
      datumOd: datumOd,
      datumDo: datumDo,
      napomena: napomena,
      korisnikId: korisnikId,
      sobaId: sobaId,
    };

    this.rezervacijaService.add(sobaId, upsertRezervacija).subscribe({
      next: () => {
        this.successMessage.set('Rezervacija je uspješna!');
        this.form.reset();
      },
      error: (err) => {
        console.error('Greška pri rezerviranju sobe', err);
        this.errorMessage.set('Došlo je do greške pri rezerviranju sobe.');
      },
    });
  }
}
