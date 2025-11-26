import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ButtonModule } from 'primeng/button';
import { catchError, finalize, of } from 'rxjs';
import { RezervacijaService } from '../../services/rezervacija.service';
import { Rezervacija } from '../../models/rezervacija';

type RezervacijaVM = {
  id: string;
  sobaId: string;
  dateRange: string;   // e.g. 12.09.2025 – 15.09.2025
  nights: number;      // 3
  created: string;     // 10.09.2025 14:22
  napomena: string;
  status: 'upcoming' | 'ongoing' | 'past';
};

@Component({
  selector: 'app-user-rezervacije-list',
  imports: [TableModule, TagModule, ProgressSpinnerModule, ButtonModule],
  templateUrl: './user-rezervacije-list.component.html',
  styleUrl: './user-rezervacije-list.component.css'
})

export class UserRezervacijeListComponent {
  private readonly rezervacijaService = inject(RezervacijaService);
  private readonly router = inject(Router);

  rezervacije = signal<RezervacijaVM[]>([]);
  loading = signal<boolean>(false);
  error = signal<string | null>(null);

  constructor() { this.load(); }
  reload() { this.load(); }

  private load() {
    this.loading.set(true);
    this.error.set(null);

    this.rezervacijaService.getAllForUser().pipe(
      catchError(err => {
        // Treat 404 "nema rezervacija" as empty list; show message for others
        if (err?.status === 404) return of<Rezervacija[]>([]);
        this.error.set(err?.error?.message ?? err?.error ?? 'Greška pri dohvaćanju rezervacija.');
        return of<Rezervacija[]>([]);
      }),
      finalize(() => this.loading.set(false))
    ).subscribe(list => {
      const vms = (list ?? []).map(this.toVM);
      this.rezervacije.set(vms);
    });
  }

  private toVM = (r: Rezervacija): RezervacijaVM => {
    const dOd = new Date(r.datumOd);
    const dDo = new Date(r.datumDo);
    const created = new Date(r.datumKreiranja);

    const fmtDate = (d: Date) =>
      new Intl.DateTimeFormat('hr-HR', { day: '2-digit', month: '2-digit', year: 'numeric' }).format(d);

    const fmtDateTime = (d: Date) =>
      new Intl.DateTimeFormat('hr-HR', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' }).format(d);

    const nights = Math.max(1, Math.round((stripTime(dDo).getTime() - stripTime(dOd).getTime()) / 86400000));
    const today = stripTime(new Date());
    let status: RezervacijaVM['status'] = 'upcoming';
    if (stripTime(dOd) <= today && stripTime(dDo) > today) status = 'ongoing';
    else if (stripTime(dDo) <= today) status = 'past';

    return {
      id: r.id,
      sobaId: r.sobaId,
      dateRange: `${fmtDate(dOd)} – ${fmtDate(dDo)}`,
      nights,
      created: fmtDateTime(created),
      napomena: r.napomena,
      status
    };
  };

  labelFor(s: RezervacijaVM['status']) {
    switch (s) {
      case 'upcoming': return 'Nadolazeća';
      case 'ongoing': return 'U tijeku';
      case 'past': return 'Završena';
    }
  }
  severityFor(s: RezervacijaVM['status']) {
    switch (s) {
      case 'upcoming': return 'info';
      case 'ongoing': return 'success';
      case 'past': return 'secondary';
    }
  }

  onDelete(sobaId: string, rezervacijaId: string) {
    this.rezervacijaService.delete(sobaId, rezervacijaId).pipe(
      finalize(() => this.load()) // refresh list after delete
    ).subscribe({
      next: () => {
        // optionally show toast/snackbar instead of navigate
        console.log(`Rezervacija ${rezervacijaId} obrisana`);
      },
      error: (err) => {
        console.error('Greška pri brisanju rezervacije', err);
        this.error.set('Greška pri brisanju rezervacije.');
      }
    });
  } 
}

function stripTime(d: Date) {
  return new Date(d.getFullYear(), d.getMonth(), d.getDate());
}

