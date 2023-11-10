import {CustomEventMethods} from "./CustomEventMethods";
import {ClaimsIdentity, User} from "../auth";

export const UserLoadedEvent = new CustomEventMethods<User>("userLoaded");
export const TokenExpiringEvent = new CustomEventMethods<string>("tokenExpiring");
export const UserSignedOutEvent = new CustomEventMethods<User>("userSignedOut");
export const IdentityChangedEvent = new CustomEventMethods<ClaimsIdentity | null>("identityChanged");