import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface PersonModel {
  id: number;
  name: string;
  fiscalDocument: number;
}

@Injectable({
  providedIn: 'root'
})
export class PersonService {
  private baseUrl = 'http://localhost:5075/api/Person';

  constructor(private http: HttpClient) {}

  getAllPeople(): Observable<PersonModel[]> {
    return this.http.get<PersonModel[]>(`${this.baseUrl}/GetAllPeople`);
  }

  getPerson(id: number): Observable<PersonModel> {
    return this.http.get<PersonModel>(`${this.baseUrl}/${id}`);
  }

  addPerson(name: string, fiscalDocument: number): Observable<number> {
    return this.http.post<number>(`${this.baseUrl}/AddPerson`, { name, fiscalDocument });
  }

  updatePerson(idPerson: number, name: string): Observable<PersonModel> {
    return this.http.patch<PersonModel>(`${this.baseUrl}/UpdatePerson`, { idPerson, name });
  }

  deletePerson(idPerson: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${idPerson}`);
  }
}
