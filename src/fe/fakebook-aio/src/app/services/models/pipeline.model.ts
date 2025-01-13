export interface PipelineCreateModel {
    jobName: string;
    jobDescription: string;
    authToken: string;
    pipelineContent: string;
}

export interface PipelineUpdateModel {
    jobName: string;
    jobDescription: string;
    authToken: string;
    pipelineContent: string;
}

export interface PipelineModel {
    id: string,
    createdDate: Date,
    lastModifiedDate: Date,
    jobName: string,
    jobDescription: string,
    authToken: string,
    pipelineContent: string,
}
