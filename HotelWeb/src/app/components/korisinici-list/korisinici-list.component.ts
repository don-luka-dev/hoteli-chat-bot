import { Component, DestroyRef, effect, inject, signal } from '@angular/core';
import { FormBuilder, FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, startWith, switchMap, tap } from 'rxjs/operators';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { TableModule } from 'primeng/table';
import { InputTextModule } from 'primeng/inputtext';
import { ToolbarModule } from 'primeng/toolbar';
import { ButtonModule } from 'primeng/button';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { KorisnikService } from '../../services/korisnik.service';
import { Korisnik } from '../../models/korisnik';
import { AuthService } from '../../services/auth.service';


@Component({
  selector: 'app-korisinici-list',
  imports: [
    ReactiveFormsModule,
    TableModule,
    InputTextModule,
    ToolbarModule,
    ButtonModule,
    ProgressSpinnerModule
  ],
  templateUrl: './korisinici-list.component.html',
  styleUrl: './korisinici-list.component.css'
})
export class KorisiniciListComponent {
  private readonly fb = inject(FormBuilder);
  public readonly korisnikService = inject(KorisnikService);
  public readonly authService = inject(AuthService);
  private readonly destroyRef = inject(DestroyRef);

  // Separate control (like your nazivControl example)
  usernameControl = new FormControl<string>('', { nonNullable: true });

  // Additional filter group if you expand later; currently empty (but present to mirror your pattern)
  filterForm = this.fb.group({});

  users = signal<Korisnik[]>([]);
  loading = signal<boolean>(false);

  constructor() {
    // initial load
    this.loadUsers({});

    // wire filter changes exactly like your pattern (adapted to korisnickoIme + loadUsers)
    this.handleFilterChanges();
  }

  private handleFilterChanges() {
    // Mirrors your "filterForm.valueChanges" branch
    this.filterForm.valueChanges
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((partialFilter) => {
        const filter = {
          ...partialFilter,
          korisnickoIme: this.usernameControl.value?.trim() || undefined
        } as { korisnickoIme?: string };
        this.loadUsers(filter);
      });

    // Mirrors your "nazivControl" branch with debounce + distinctUntilChanged
    this.usernameControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe((korisnickoIme) => {
        const filter = {
          ...this.filterForm.value,
          korisnickoIme: (korisnickoIme || '').trim() || undefined
        } as { korisnickoIme?: string };
        this.loadUsers(filter);
      });
  }

  refresh() {
    const filter = {
      ...this.filterForm.value,
      korisnickoIme: this.usernameControl.value?.trim() || undefined
    } as { korisnickoIme?: string };
    this.loadUsers(filter);
  }

  private loadUsers(filter: { korisnickoIme?: string }) {
    this.loading.set(true);
    this.korisnikService.getAll(filter)
      .pipe(
        startWith(undefined),
        tap(() => this.loading.set(true)),
        switchMap(() => this.korisnikService.getAll(filter))
      )
      .subscribe({
        next: (data) => {
          this.users.set(data);
          this.loading.set(false);
        },
        error: () => {
          this.users.set([]);
          this.loading.set(false);
        }
      });
  }

  onPromote(id: number) {
  this.korisnikService.promoteToAdmin(id).subscribe({
    next: () => console.log('User promoted'),
    error: (err) => console.error('Promote error', err)
  });
}

onDelete(id: number) {
  this.korisnikService.delete(id).subscribe({
    next: () => console.log('User deleted'),
    error: (err) => console.error('Delete error', err)
  });
}
}
