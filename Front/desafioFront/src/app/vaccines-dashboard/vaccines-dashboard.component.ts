import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { VaccineService, VaccineModel } from '../services/vaccine.service';

@Component({
  selector: 'app-vaccines-dashboard',
  templateUrl: './vaccines-dashboard.component.html',
  styleUrls: ['./vaccines-dashboard.component.css']
})
export class VaccinesDashboardComponent implements OnInit {
  vaccines: VaccineModel[] = [];
  error: string = '';

  constructor(
    private vaccineService: VaccineService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadVaccines();
  }

  loadVaccines(): void {
    this.vaccineService.getAllVaccines().subscribe({
      next: (data) => this.vaccines = data,
      error: (err) => this.error = 'Erro ao carregar vacinas.'
    });
  }

  addVaccine(): void {
    this.router.navigate(['/vaccine/add']);
  }

  editVaccine(id: number): void {
    this.router.navigate(['/vaccine', id]);
  }

  deleteVaccine(id: number): void {
    if (confirm('Tem certeza que deseja excluir esta vacina?')) {
      this.vaccineService.deleteVaccine(id).subscribe({
        next: () => this.loadVaccines(),
        error: () => this.error = 'Erro ao excluir vacina.'
      });
    }
  }
}
