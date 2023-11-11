import {ProblemDetails} from "../../domain/models";

export function isProblemDetails(candidate: any): candidate is ProblemDetails {
    return "status" in candidate && "title" in candidate;
}
