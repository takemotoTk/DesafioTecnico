import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PersonService, PersonModel } from '../../services/person.service';

@Component({
  selector: 'app-search-vaccination-card',
  templateUrl: './search-vaccination-card.component.html',
  styleUrls: ['./search-vaccination-card.component.css']
})
export class SearchVaccinationCardComponent implements OnInit {
  people: PersonModel[] = [];
  filteredPeople: PersonModel[] = [];
  filterText: string = '';
  errorMessage: string = '';

  constructor(private router: Router, private personService: PersonService) {}

  ngOnInit(): void {
    this.personService.getAllPeople().subscribe({
      next: (people) => {
        this.people = people;
        this.filteredPeople = people;
      },
      error: () => (this.errorMessage = 'Erro ao carregar as pessoas.')
    });
  }

  applyFilter(): void {
    const searchTerm = this.filterText.toLowerCase();
    this.filteredPeople = this.people.filter(person =>
      person.name.toLowerCase().includes(searchTerm)
    );
  }

  selectPerson(person: PersonModel): void {
    this.router.navigate(['/vaccination-card', person.id]);
  }
}
