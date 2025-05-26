import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VaccineService } from '../../services/vaccine.service';

@Component({
  selector: 'app-vaccine-form',
  templateUrl: './vaccine-form.component.html',
  styleUrls: ['./vaccine-form.component.css']
})
export class VaccineFormComponent implements OnInit {
  vaccine = {
    id: 0,
    name: '',
    maxDose: 0,
    maxReinforcement: 0,
    registerNumber: ''
  };

  isEditMode = false;
  error: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private vaccineService: VaccineService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.params['id']);
    if (!isNaN(id) && id > 0) {
      this.isEditMode = true;
      this.vaccineService.getAllVaccines().subscribe({
        next: (data) => {
          const found = data.find(v => v.id === id);
          if (found) {
            this.vaccine = { ...found, registerNumber: found.registerNumber || '' };
          } else {
            this.error = 'Vacina não encontrada.';
          }
        },
        error: () => this.error = 'Erro ao carregar vacina.'
      });
    }
  }

  isValidGuid(guid: string): boolean {
    const guidRegex = /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;
    return guidRegex.test(guid);
  }

  onSubmit(): void {
    this.error = '';

    if (this.vaccine.maxDose < 0 || this.vaccine.maxReinforcement < 0) {
      this.error = 'Doses e reforços máximos não podem ser menores que 0.';
      return;
    }

    if (!this.isEditMode) {
      if (!this.isValidGuid(this.vaccine.registerNumber)) {
        this.error = 'O número de registro deve ser um GUID válido.';
        return;
      }
    }

    if (this.isEditMode) {
      this.vaccineService.updateVaccine(this.vaccine).subscribe({
        next: () => this.router.navigate(['/vaccines']),
        error: () => this.error = 'Erro ao atualizar vacina.'
      });
    } else {
      const payload = {
        name: this.vaccine.name,
        maxDose: this.vaccine.maxDose,
        maxReinforcement: this.vaccine.maxReinforcement,
        registerNumber: this.vaccine.registerNumber
      };
      this.vaccineService.addVaccine(payload).subscribe({
        next: () => this.router.navigate(['/vaccines']),
        error: () => this.error = 'Erro ao adicionar vacina.'
      });
    }
  }

  cancel() {
    this.router.navigate(['/vaccines']);
  }
}
