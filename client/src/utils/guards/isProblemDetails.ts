import {ProblemDetails} from "../../domain/models";

export function isProblemDetails(candidate: object): candidate is ProblemDetails {
    return "status" in candidate && "title" in candidate;
}
