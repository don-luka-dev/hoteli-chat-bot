import { Component, inject, signal } from '@angular/core';
import { AvatarModule } from 'primeng/avatar';
import { ButtonModule } from 'primeng/button';
import { ProgressSpinner } from 'primeng/progressspinner';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { finalize } from 'rxjs';
import { Hotel } from '../../models/hotel';
import { HotelService } from '../../services/hotel.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nepotvrdeni-hoteli',
  imports: [ProgressSpinner, TableModule, AvatarModule, TagModule, ButtonModule],
  templateUrl: './nepotvrdeni-hoteli.component.html',
  styleUrl: './nepotvrdeni-hoteli.component.css'
})
export class NepotvrdeniHoteliComponent {
private readonly hotelService = inject(HotelService);
  private readonly router = inject(Router);

  hoteli = signal<Hotel[]>([]);
  loading = signal<boolean>(false);
  error = signal<string | null>(null);

  placeholder = 'https://via.placeholder.com/64x64?text=Hotel';

  constructor() {
    this.load();
  }

  reload() { this.load(); }

  private load() {
    this.loading.set(true);
    this.error.set(null);

    this.hotelService.getUnconfirmed().subscribe({
      next: data => {
        this.hoteli.set(data ?? []);
        this.loading.set(false);
      },
      error: err => {
        console.error('Nema hotela koji čekaju potvrdu.', err);
        this.error.set('Nema hotela koji čekaju potvrdu.');
        this.loading.set(false);
      }
    });
  }

  open(id: string) {
    this.router.navigate(['/hoteli', id]);
  }

  onDelete(id: string) {  
      this.hotelService.delete(id).pipe(
        finalize(() => this.load()) // refresh list after delete
      ).subscribe({
        next: () => {
          // optionally show toast/snackbar instead of navigate
          console.log(`Hotel ${id} obrisan`);
        },
        error: (err) => {
          console.error('Greška pri brisanju hotela', err);
          this.error.set('Greška pri brisanju hotela.');
        }
      });
    } 

    onConfirm(id: string) {  
      this.hotelService.confirm(id).pipe(
        finalize(() => this.load()) // refresh list after delete
      ).subscribe({
        next: () => {
          // optionally show toast/snackbar instead of navigate
          console.log(`Hotel ${id} potvrđen`);
        },
        error: (err) => {
          console.error('Greška pri potvrđivanju hotela', err);
          this.error.set('Greška pri potvrđivanju hotela.');
        }
      });
    } 
}
