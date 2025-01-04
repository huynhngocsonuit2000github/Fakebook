export interface CaseModel {
    id: string,
    createdDate: Date,
    lastModifiedDate: Date,
    name: string,
    description: string,
    jobName: string,
    numberOfSuccess: number,
    numberOfFailed: number,
}

export interface CaseCreateModel {
    name: string,
    description: string,
    jobName: string,
}

export interface CaseUpdateModel {
    name: string,
    description: string,
    jobName: string,
}