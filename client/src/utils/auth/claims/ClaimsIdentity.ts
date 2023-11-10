import {Claim} from "./Claim";
import {ClaimTypes} from "./ClaimTypes";
import jwt from "jsonwebtoken";

export type MatchClaim = (claim: Claim) => boolean;

export class ClaimsIdentity {
    nameClaimType: string = ClaimTypes.Name;
    rolesClaimType: string = ClaimTypes.Roles;
    readonly issuer: string;

    constructor(claims: Claim[]) {
        if (claims) {
            Array.isArray(claims) ? this._claims.push(...claims) : this._claims.push(claims);
        }
        this.issuer = this.findFirst(ClaimTypes.Issuer)?.value ?? "unknown";
    }

    private _claims: Claim[] = [];

    public get claims(): ReadonlyArray<Claim> {
        return this._claims;
    }

    public get name(): string | null {
        return this.claims.find((x) => x.key === this.nameClaimType)?.value ?? null;
    }

    public get id(): string | null {
        return this.claims.find((x) => x.key === ClaimTypes.Subject)?.value ?? null;
    }

    public get email(): string | null {
        return this.claims.find((x) => x.key === ClaimTypes.Email)?.value ?? null;
    }

    public get roles(): string[] {
        return this.claims.filter((x) => x.key === this.rolesClaimType).map((x) => x.value);
    }

    static fromJwt(token: string): ClaimsIdentity {
        const decoded = jwt.decode(token, {json: true});
        if (decoded === null) {
            throw new Error("Provided JWT could not be decoded");
        }
        const claims = Object.keys(decoded).map(key => new Claim(key, decoded[key].toString()));
        return new ClaimsIdentity(claims);
    }

    public addClaim(claim: Claim): void {
        this._claims.push(claim);
    }

    public addClaims(claims: Claim[]): void {
        this._claims.push(...claims);
    }

    public hasClaim(typeOrMatch: string | MatchClaim, value?: string) {
        const match = this.normalizeTypeOrMatch(typeOrMatch);
        for (const claim of this.claims) {
            if (match(claim) && (!value || claim.value === value)) {
                return true;
            }
        }
        return false;
    }

    public findFirst(typeOrMatch: string | MatchClaim): Claim | null {
        const match = this.normalizeTypeOrMatch(typeOrMatch);
        for (const claim of this.claims) {
            if (match(claim)) {
                return claim;
            }
        }
        return null;
    }

    public findAll(typeOrMatch: string | MatchClaim): Claim[] {
        const result: Claim[] = [];
        const match = this.normalizeTypeOrMatch(typeOrMatch);
        for (const claim of this.claims) {
            if (match(claim)) {
                result.push(claim);
            }
        }
        return result;
    }

    private normalizeTypeOrMatch(typeOrMatch: string | MatchClaim): MatchClaim {
        let matchNormalized: (claim: Claim) => boolean;
        typeof typeOrMatch === "function"
            ? (matchNormalized = typeOrMatch)
            : (matchNormalized = (claim) => claim.key.toUpperCase() === typeOrMatch.toUpperCase());
        return matchNormalized;
    }
}