import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import {
  FormBuilder,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';
import { PasswordModule } from 'primeng/password';
import { LoginDto } from '../../models/korisnik';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    InputTextModule,
    ButtonModule,
    MessageModule,
    PasswordModule,
    RouterLink,
],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  private readonly router = inject(Router);
  private readonly authService = inject(AuthService);
  private readonly fb = inject(FormBuilder);

  successMessage = signal<string | null>(null);
  errorMessage = signal<string | null>(null);

  form = this.fb.group({
    username: ['', [Validators.required]],
    lozinka: ['', Validators.required],
  });

  onSubmit() {
    if (this.form.invalid) {
      this.errorMessage.set('Molimo ispunite sva polja ispravno.');
      return;
    }

    const loginDto: LoginDto = this.form.value as LoginDto;

    console.log('Prijava s podacima:', loginDto);

    this.authService.login(loginDto).subscribe({
      next: (response) => {
        //Savjet: keyjeve za localstorage bi bilo dobro definirati u neki constants file gdje ćeš ih sve definirati pa da nemaš magic stringove po komponentama
        //Također pogledaj da typesafe access kontrolama
        localStorage.setItem('accessToken', response.accessToken);
        localStorage.setItem('refreshToken', response.refreshToken);
        localStorage.setItem('userId', response.userId.toString());
        this.successMessage.set('Uspješno ste se prijavili!');
        this.form.reset();
        this.router.navigate(['/hoteli']);
      },
      error: (err) => {
        console.error('Greška pri prijavi', err);
        this.errorMessage.set('Došlo je do greške pri prijavi.');
      },
    });
  }
}
