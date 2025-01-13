import { ReportsService } from './../../services/reports.service';
import { CaseCreateModel, CaseUpdateModel } from './../../services/models/cases.model';
import { FormArray, FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { CasesService } from '../../services/cases.service';
import { CommonModule } from '@angular/common';
import { PipelineModel } from '../../services/models/pipeline.model';

@Component({
  selector: 'app-cases',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './cases.component.html',
  styleUrl: './cases.component.scss'
})
export class CasesComponent implements OnInit {
  caseForm!: FormGroup;
  indices: number[] = []; // Array to hold indices 
  addingNewCase = false; // Track if a new case is being added
  newCaseForm!: FormGroup; // FormGroup for the new case
  pipelines: PipelineModel[] = [];


  form: FormGroup;


  constructor(private fb: FormBuilder, private caseService: CasesService, private reportsService: ReportsService) {
    this.form = this.fb.group({
      selectedPipeline: [null], // Initialize the control
    });
  }

  ngOnInit(): void {
    this.caseForm = this.fb.group({
      cases: this.fb.array([]),
    });

    // Initialize form for the new case
    this.newCaseForm = this.fb.group({
      id: [''],
      name: ['', Validators.required],
      description: ['', Validators.required],
      jobName: ['', Validators.required],
      numberOfSuccess: [0, Validators.required],
      numberOfFailed: [0, Validators.required],
    });

    this.caseService.getAllCases().subscribe(caseList => {
      console.log(caseList);

      caseList.forEach(e => {
        this.addCase(e);
      });
    });

    this.fetchPipelines();
  }

  fetchPipelines(): void {
    this.reportsService.getAllPipelines().subscribe(
      (data) => {
        this.pipelines = data;
        if (data.length > 0) {
          this.form.patchValue({ selectedPipeline: data[0].id }); // Safely set the value
        }
      },
      (error) => {
        console.error('Error fetching pipelines:', error);
      }
    );
  }

  onPipelineChange(): void {
    console.log('Selected Pipeline:', this.form.value.selectedPipeline);
  }

  get cases(): FormArray {
    return this.caseForm.get('cases') as FormArray;
  }

  removeCase(index: number): void {
    if (confirm("Do you want to remove this case")) {
      const deletedCase = this.getFormGroupAt(index).value;

      this.caseService.deleteCases(deletedCase.id).subscribe(res => {
        console.log(res);

        this.cases.removeAt(index);

        // Update indices using a for loop
        this.updateIndices();
      })
    }
  }

  updateCase(index: number): void {

    const updatedCase = this.getFormGroupAt(index).value;
    var caseUpdateModel: CaseUpdateModel = { ...updatedCase };

    this.caseService.updateCases(updatedCase.id, caseUpdateModel).subscribe(res => {
      alert("Update case success!");
    })
  }

  runTestCase(index: number): void {
    var pipeline = this.pipelines.find(e => e.id == this.form.value.selectedPipeline)?.jobName;

    if (confirm(`Are you sure to use Pipeline ${pipeline} to run test?`)) {
      const item = this.getFormGroupAt(index).value;
      console.log('Run test case ', item.id);

      this.reportsService.triggerCases(item.id, this.form.value.selectedPipeline).subscribe(({ message }) => {
        alert(message)
      })
    }
  }

  updateIndices(): void {
    // Clear and regenerate indices using a `for` loop
    this.indices = [];
    for (let i = 0; i < this.cases.length; i++) {
      this.indices.push(i);
    }
  }

  getFormGroupAt(index: number): FormGroup {
    return this.cases.at(index) as FormGroup;
  }

  onSubmit(): void {
    console.log('Submitted Data:', this.caseForm.value);
  }

  addCase(data: any = null): void {
    const newCase = this.fb.group({
      id: [data?.id || ''],
      name: [data?.name || '', Validators.required],
      description: [data?.description || '', Validators.required],
      jobName: [data?.jobName || '', Validators.required],
      numberOfSuccess: [data?.numberOfSuccess || 0, Validators.required],
      numberOfFailed: [data?.numberOfFailed || 0, Validators.required],
    });

    this.cases.push(newCase);
    this.updateIndices();
  }

  saveNewCase(): void {
    // Extract form values and push them to the cases array
    const newCaseData = { ...this.newCaseForm.value, numberOfSuccess: 0, numberOfFailed: 0 };
    var createModelNew: CaseCreateModel = { ...newCaseData }

    this.caseService.createCases(createModelNew).subscribe(createdCase => {
      this.cases.push(this.fb.group(createdCase));
      this.updateIndices();
      this.addingNewCase = false;
      this.newCaseForm.reset();
    })
  }

  cancelNewCase(): void {
    this.newCaseForm.reset();
    this.addingNewCase = false;
  }

  showNewCaseForm(): void {
    this.addingNewCase = true;

    // Delay scrolling to ensure the element is rendered
    setTimeout(() => {
      const newCaseElement = document.querySelector('.new-case-form');
      newCaseElement?.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }, 0);
  }
}