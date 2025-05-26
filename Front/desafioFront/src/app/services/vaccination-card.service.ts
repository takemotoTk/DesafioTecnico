import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface VaccinationDetailsModel {
  idVaccinationCard?: number | null;
  appliedDoseDateTime?: string | null; // ISO string
  situation: string; // VaccineSituationEnum as string
}

export interface VaccinationModel {
  id: number;
  vaccineName: string;
  dose1: VaccinationDetailsModel;
  dose2?: VaccinationDetailsModel | null;
  dose3?: VaccinationDetailsModel | null;
  reinforcement1?: VaccinationDetailsModel | null;
  reinforcement2?: VaccinationDetailsModel | null;
}

export interface VaccinationCardByPersonModel {
  idPerson: number;
  name: string;
  vaccines: VaccinationModel[];
}

export interface AddVaccinationCardCommand {
  idPerson: number;
  idVaccine: number;
  appliedDoseType: number;
}

@Injectable({
  providedIn: 'root'
})
export class VaccinationCardService {

  private baseUrl = 'http://localhost:5075/api/VaccinationCard';

  constructor(private http: HttpClient) { }

  getVaccinationCardByPerson(idPerson: number): Observable<VaccinationCardByPersonModel> {
    return this.http.get<VaccinationCardByPersonModel>(`${this.baseUrl}/${idPerson}`);
  }

  addVaccination(command: AddVaccinationCardCommand): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/AddVaccination`, command);
  }

  deleteVaccination(idVaccination: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${idVaccination}`);
  }
}
