import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-pipelines',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './pipelines.component.html',
  styleUrl: './pipelines.component.scss'
})
export class PipelinesComponent implements OnInit {

  jenkinsForm!: FormGroup;

  constructor(private fb: FormBuilder, private http: HttpClient) {
  }

  ngOnInit(): void {
    this.jenkinsForm = this.fb.group({
      jobName: ['', Validators.required],
      jobDescription: [''],
      branch: ['', Validators.required],
      buildCommands: ['', Validators.required],
    });
  }

  onSubmit(event: Event) {
    console.log('aa');
    console.log(this.jenkinsForm.value);
    event.preventDefault();

    return;

    if (this.jenkinsForm.valid) {
      const jobData = this.jenkinsForm.value;
      this.http.post('https://your-api-endpoint/jenkins/jobs', jobData).subscribe({
        next: (response) => {
          alert('Jenkins job created successfully!');
        },
        error: (error) => {
          alert('Error creating Jenkins job: ' + error.message);
        }
      });
    } else {
      alert('Please fill all required fields.');
    }
  }
}
