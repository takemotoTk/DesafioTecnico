import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { PersonService } from '../services/person.service';
import { PersonModel } from '../models/person.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-people-dashboard',
  templateUrl: './people-dashboard.component.html',
  styleUrls: ['./people-dashboard.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class PeopleDashboardComponent implements OnInit {
  people: PersonModel[] = [];
  error?: string;

  constructor(private personService: PersonService, private router: Router) {}

  ngOnInit() {
    this.loadPeople();
  }

  loadPeople() {
    this.personService.getAllPeople().subscribe({
      next: (data) => {
        this.people = data;
        this.error = undefined;
      },
      error: (err) => this.error = err.message || 'Erro ao carregar pessoas',
    });
  }

  addPerson() {
    this.router.navigate(['/person/add']);
  }

  editPerson(id: number) {
    this.router.navigate(['/person/', id]);
  }

  deletePerson(id: number) {
    if (confirm('Tem certeza que deseja excluir esta pessoa?')) {
      this.personService.deletePerson(id).subscribe({
        next: () => this.loadPeople(),
        error: (err) => this.error = 'Erro ao excluir pessoa: ' + (err.message || '')
      });
    }
  }
}