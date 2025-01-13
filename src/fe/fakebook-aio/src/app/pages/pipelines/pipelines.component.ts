import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ReportsService } from './../../services/reports.service';
import { PipelineModel } from '../../services/models/pipeline.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pipelines',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './pipelines.component.html',
  styleUrls: ['./pipelines.component.scss']
})
export class PipelinesComponent {
  jenkinsForm: FormGroup;
  jenkinsJobs: PipelineModel[] = [];
  isNewPipeline: boolean = true; // Tracks whether creating a new pipeline or editing

  constructor(private fb: FormBuilder, private reportsService: ReportsService) {
    this.jenkinsForm = this.fb.group({
      id: [''],
      jobName: ['', Validators.required],
      jobDescription: [''],
      authToken: ['', Validators.required],
      pipelineContent: ['', Validators.required],
    });

    this.loadJobs();
  }

  loadJobs() {
    this.reportsService.getAllPipelines().subscribe({
      next: (jobs) => {
        this.jenkinsJobs = jobs;
      },
      error: (error) => {
        alert('Failed to load Jenkins jobs: ' + error.message);
      }
    });
  }

  loadJob(job: PipelineModel) {
    this.isNewPipeline = false; // Editing existing pipeline
    this.jenkinsForm.patchValue({
      id: job.id,
      jobName: job.jobName,
      jobDescription: job.jobDescription,
      authToken: job.authToken,
      pipelineContent: job.pipelineContent,
    });
  }

  createNewPipeline() {
    this.isNewPipeline = true; // Creating new pipeline
    this.jenkinsForm.reset(); // Clear the form
  }

  onSubmit(event: Event) {
    event.preventDefault();

    if (this.jenkinsForm.valid) {
      const formData = this.jenkinsForm.value;

      if (this.isNewPipeline) {
        this.reportsService.createJob(formData).subscribe({
          next: () => {
            alert('Pipeline created successfully!');
            this.loadJobs();
            this.jenkinsForm.reset();
          },
          error: (error) => {
            alert('Error creating pipeline: ' + error.error.message);
          }
        });
      } else {
        this.reportsService.updateJob(formData.id, formData).subscribe({
          next: () => {
            alert('Pipeline updated successfully!');
            this.loadJobs();
          },
          error: (error) => {
            alert('Error updating pipeline: ' + error.error.message);
          }
        });
      }
    } else {
      alert('Please fill all required fields.');
    }
  }
}
