import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';
import { PasswordModule } from 'primeng/password';
import { UpsertKorisnik } from '../../models/korisnik';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    InputTextModule,
    ButtonModule,
    MessageModule,
    PasswordModule,
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
   private readonly router = inject(Router);
  private readonly authService = inject(AuthService);
  private readonly fb = inject(FormBuilder);

  successMessage = signal<string | null>(null);
  errorMessage = signal<string | null>(null);

  form = this.fb.group({
    username: ['', [Validators.required]],
    ime: ['', Validators.required],
    prezime: ['', Validators.required],
    email: ['', Validators.required,Validators.email],
    lozinka: ['', Validators.required],
    lozinkaPotvrda: ['', Validators.required],

  });

  onSubmit() {
    if (this.form.invalid) {
      this.errorMessage.set('Molimo ispunite sva polja ispravno.');
      return;
    }

    if(this.form.value.lozinka !== this.form.value.lozinkaPotvrda) {
      this.errorMessage.set('Lozinke se ne podudaraju.');
      return;
    }

    const upsertKorisnik: UpsertKorisnik = this.form.value as UpsertKorisnik;

    console.log('Prijava s podacima:', upsertKorisnik);

    this.authService.register(upsertKorisnik).subscribe({
      next: () => {
        this.successMessage.set('Uspješno ste se registrirali! Možete se prijaviti.');
        this.router.navigate(['/login']);
      },
      error: (err) => {
        console.error('Greška pri prijavi', err);
        this.errorMessage.set('Došlo je do greške pri prijavi.');
      },
    });
  }
}
