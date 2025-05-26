import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VaccinationCardService, VaccinationCardByPersonModel, VaccinationDetailsModel } from '../services/vaccination-card.service';

@Component({
  selector: 'app-vaccination-card',
  templateUrl: './vaccination-card.component.html',
  styleUrls: ['./vaccination-card.component.css']
})
export class VaccinationCardComponent implements OnInit {

  idPerson!: number;
  cardData?: VaccinationCardByPersonModel;
  errorMessage: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private vaccinationCardService: VaccinationCardService
  ) {}

  ngOnInit(): void {
    this.idPerson = Number(this.route.snapshot.paramMap.get('idPerson'));

    if (!this.idPerson || this.idPerson <= 0) {
      this.router.navigate(['/search-vaccination-card']);
      return;
    }

    this.vaccinationCardService.getVaccinationCardByPerson(this.idPerson).subscribe({
      next: (data: any) => {
        this.cardData = data;
      },
      error: (err: any) => {
        console.error('Erro ao buscar o cartão:', err);
        this.errorMessage = 'Cartão de vacinação não encontrado.';
        setTimeout(() => this.router.navigate(['/search-vaccination-card']), 2000);
      }
    });
  }

  formatDate(dateStr: string | null | undefined): string {
    if (!dateStr) {
      return '-';
    }
    const date = new Date(dateStr);
    return date.toLocaleDateString('pt-BR');
  }


  getCellClass(doseOrReinforcement: any): string {
    if (doseOrReinforcement === null || doseOrReinforcement === undefined) {
      // Não existe o objeto => cinza
      return 'gray-cell';
    }

    if (doseOrReinforcement.idVaccinationCard === null) {
      // Existe, mas não aplicado
      return 'not-applied';
    }

    if (doseOrReinforcement.idVaccinationCard > 0) {
      // Aplicado
      return 'applied';
    }

    return '';
  }

  getCellText(doseOrReinforcement: any): string {
    if (doseOrReinforcement === null || doseOrReinforcement === undefined) {
      return '';
    }

    if (doseOrReinforcement.idVaccinationCard === null) {
      return 'Ainda não Aplicado';
    }

    if (doseOrReinforcement.idVaccinationCard > 0) {
      const dataFormatada = this.formatDate(doseOrReinforcement.appliedDoseDateTime);
      return `Aplicada em ${dataFormatada}`;
    }

    return '';
  }

  findVaccinationCard(){
    this.router.navigate(['/search-vaccination-card']);
    return;
  }

  addVaccination(idVaccine: number, doseType: number): void {
    const command = {
      idPerson: this.idPerson,
      idVaccine: idVaccine,
      appliedDoseType: doseType
    };

    this.vaccinationCardService.addVaccination(command).subscribe({
      next: () => {
        alert('Vacinação adicionada com sucesso!');
        this.reloadCard();
      },
      error: (err) => {
        console.error('Erro ao adicionar vacinação:', err);
        alert('Erro ao adicionar vacinação.');
      }
    });
  }

  removeVaccination(idVaccinationCard: number | null | undefined): void {
    if (!idVaccinationCard || idVaccinationCard <= 0) return;

    this.vaccinationCardService.deleteVaccination(idVaccinationCard).subscribe({
      next: () => {
        alert('Vacinação removida com sucesso!');
        this.reloadCard();
      },
      error: (err) => {
        console.error('Erro ao remover vacinação:', err);
        alert('Erro ao remover vacinação.');
      }
    });
  }

  isVaccinationRegistered(vaccineDose: VaccinationDetailsModel | null | undefined): boolean {
    return vaccineDose?.idVaccinationCard != null && vaccineDose.idVaccinationCard > 0;
  }

  private reloadCard(): void {
    this.vaccinationCardService.getVaccinationCardByPerson(this.idPerson).subscribe({
      next: (data: any) => {
        this.cardData = data;
      },
      error: (err: any) => {
        console.error('Erro ao buscar o cartão:', err);
        this.errorMessage = 'Cartão de vacinação não encontrado.';
      }
    });
  }
}
