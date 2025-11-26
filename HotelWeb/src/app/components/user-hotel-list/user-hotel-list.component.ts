import { Component, inject, signal } from '@angular/core';
import { HotelService } from '../../services/hotel.service';
import { Router, RouterLink } from '@angular/router';
import { Hotel } from '../../models/hotel';
import { ProgressSpinner } from "primeng/progressspinner";
import { TableModule } from "primeng/table";
import { AvatarModule } from 'primeng/avatar';
import { TagModule } from 'primeng/tag';
import { finalize } from 'rxjs';
import { ButtonModule } from 'primeng/button';


@Component({
  selector: 'app-user-hotel-list',
  imports: [RouterLink, ProgressSpinner, TableModule, AvatarModule, TagModule, ButtonModule],
  templateUrl: './user-hotel-list.component.html',
  styleUrl: './user-hotel-list.component.css'
})
export class UserHotelListComponent {

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

    this.hotelService.getAllForUser().subscribe({
      next: data => {
        this.hoteli.set(data ?? []);
        this.loading.set(false);
      },
      error: err => {
        console.error('Greška pri dohvaćanju hotela', err);
        this.error.set('Greška pri dohvaćanju hotela.');
        this.loading.set(false);
      }
    });
  }

  open(id: string) {
    this.router.navigate(['/hoteli', id]);
  }

  // Map status to tag severity (PrimeNG)
  statusSeverity(h: Hotel) {
    const s = (h.statusHotela || '').toLowerCase();
    if (['aktivan', 'active', 'otvoren'].some(x => s.includes(x))) return 'success';
    if (['neaktivan', 'inactive', 'zatvoren'].some(x => s.includes(x))) return 'danger';
    if (['u tijeku', 'pending', 'na čekanju'].some(x => s.includes(x))) return 'warning';
    return 'info';
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
}
