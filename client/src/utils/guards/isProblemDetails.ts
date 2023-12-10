import {ProblemDetails} from "../../domain/models";

export function isProblemDetails(candidate: any): candidate is ProblemDetails {
    return candidate && typeof candidate.status === 'number'
        && typeof candidate.title === 'string';
}
