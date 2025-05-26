import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface VaccineModel {
  id: number;
  name: string;
  maxDose: number;
  maxReinforcement: number;
  registerNumber: string;
}

@Injectable({
  providedIn: 'root'
})
export class VaccineService {
  private baseUrl = 'http://localhost:5075/api/Vaccine';

  constructor(private http: HttpClient) {}

  getAllVaccines(): Observable<VaccineModel[]> {
    return this.http.get<VaccineModel[]>(`${this.baseUrl}/GetAllVaccines`);
  }

  addVaccine(vaccine: { name: string; maxDose: number; maxReinforcement: number; registerNumber: string }): Observable<number> {
    return this.http.post<number>(`${this.baseUrl}/AddVaccine`, vaccine);
  }

  updateVaccine(vaccine: { id: number; name: string; maxDose: number; maxReinforcement: number }): Observable<VaccineModel> {
    const payload = {
      idVaccine: vaccine.id,
      name: vaccine.name,
      maxDose: vaccine.maxDose,
      maxReinforcement: vaccine.maxReinforcement
    };
    return this.http.patch<VaccineModel>(`${this.baseUrl}/UpdateVaccine`, payload);
  }

  deleteVaccine(idVaccine: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${idVaccine}`);
  }
}
