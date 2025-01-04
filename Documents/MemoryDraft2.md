<div class="case-item card mb-3" *ngFor="let i of indices">
    <div class="card-body" [formGroup]="getFormGroupAt(i)">
        <h3 class="card-title">Case {{ i + 1 }}</h3>
        <div class="row">
            <div class="col-md-6">
                <label for="name-{{ i }}">Name</label>
                <input id="name-{{ i }}" formControlName="name" class="form-control" type="text" />
            </div>
            <div class="col-md-6">
                <label for="description-{{ i }}">Description</label>
                <input id="description-{{ i }}" formControlName="description" class="form-control" type="text" />
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-md-6">
                <label for="jobName-{{ i }}">Job Name</label>
                <input id="jobName-{{ i }}" formControlName="jobName" class="form-control" type="text" />
            </div>
            <div class="col-md-3">
                <label for="numberOfSuccess-{{ i }}">Success</label>
                <input id="numberOfSuccess-{{ i }}" formControlName="numberOfSuccess" class="form-control"
                    type="number" />
            </div>
            <div class="col-md-3">
                <label for="numberOfFailed-{{ i }}">Failed</label>
                <input id="numberOfFailed-{{ i }}" formControlName="numberOfFailed" class="form-control"
                    type="number" />
            </div>
        </div>
        <div class="text-end mt-3">
            <button type="button" class="btn btn-danger btn-sm" (click)="removeCase(i)">
                Remove Case
            </button>
        </div>
    </div>
</div>
