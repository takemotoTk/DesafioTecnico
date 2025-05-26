import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PersonService, PersonModel } from '../../services/person.service';

@Component({
  selector: 'app-person-form',
  templateUrl: './person-form.component.html',
  styleUrls: ['./person-form.component.css']
})
export class PersonFormComponent implements OnInit {
  form!: FormGroup;
  isEditMode = false;
  personId?: number;
  error?: string;

  constructor(
    private fb: FormBuilder,
    private personService: PersonService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      fiscalDocument: ['', [Validators.required, Validators.pattern('^[0-9]+$')]]
    });

    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.personId = +params['id'];
        this.loadPerson(this.personId);
      }
    });
  }

  loadPerson(id: number) {
    this.personService.getPerson(id).subscribe({
      next: (person) => {
        this.form.patchValue({
          name: person.name,
          fiscalDocument: person.fiscalDocument
        });
      },
      error: () => this.error = 'Erro ao carregar dados da pessoa.'
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      this.error = 'Por favor, preencha corretamente todos os campos.';
      return;
    }

    this.error = undefined;

    const { name, fiscalDocument } = this.form.value;

    if (this.isEditMode && this.personId) {
      this.personService.updatePerson(this.personId, name).subscribe({
        next: () => this.router.navigate(['/people']),
        error: () => this.error = 'Erro ao atualizar pessoa.'
      });
    } else {
      this.personService.addPerson(name, +fiscalDocument).subscribe({
        next: () => this.router.navigate(['/people']),
        error: () => this.error = 'Erro ao adicionar pessoa.'
      });
    }
  }

  cancel() {
    this.router.navigate(['/people']);
  }
}
