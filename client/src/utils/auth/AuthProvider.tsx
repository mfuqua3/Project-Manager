import React, {createContext, ReactNode, useEffect, useState} from "react";
import SilentRefresh from "./SilentRefresh";
import AxiosConfig from "./AxiosConfig";
import {useToken} from "./TokenProvider";
import {useLogger} from "../logging";
import {ClaimsIdentity} from "./claims/ClaimsIdentity";
import {ClaimTypes} from "./claims/ClaimTypes";
import {Claim} from "./claims/Claim";
import {
    CustomEventListener,
    IdentityChangedEvent,
    TokenExpiringEvent,
    UserLoadedEvent,
    UserSignedOutEvent
} from "../events";
import {JwtPayload} from "jsonwebtoken";
import {AuthorizationsApi} from "../../api/AuthorizationsApi";
import {ProblemDetails} from "../../domain/models";


export type SignInResult = { succeeded: true } | { succeeded: false, details: ProblemDetails };

export interface AuthEvents {
    addUserLoaded(callbackFn: CustomEventListener<User>): void;

    removeUserLoaded(callbackFn: CustomEventListener<User>): void;

    addUserSignedOut(callbackFn: CustomEventListener<User>): void;

    removeUserSignedOut(callbackFn: CustomEventListener<User>): void;

    addAccessTokenExpiring(callbackFn: CustomEventListener<string>): void;

    removeAccessTokenExpiring(callbackFn: CustomEventListener<string>): void;
}

export interface AuthActions {
    silentRefresh: () => Promise<SignInResult>;
    handleGoogleSso: (idToken: string) => Promise<SignInResult>;
    signout: () => Promise<void>;
}

export type AuthState = { events: AuthEvents, actions: AuthActions } &
    ({ isReady: true } & ({ isAuthenticated: false } | { isAuthenticated: true, user: User }) |
        { isReady: false, isAuthenticated: false });
export const AuthContext = createContext<AuthState | null>(null);

export interface User {
    accessToken: string;
    claims: ClaimsIdentity;
    profile: Partial<{ id: string, name: string, email: string }>;
    metadata: Partial<{ iat: string, aud: string, iss: string, exp: string }>;
}

function AuthProvider({children}: { children: ReactNode }) {
    const {jwtPayload, token, clear, setToken} = useToken();
    const logger = useLogger(AuthProvider);
    const [isReady, setIsReady] = useState(false);
    const [identity, setIdentity] = useState<ClaimsIdentity | null>(null);
    useEffect(() => {
        if (jwtPayload === null) {
            setIdentity(null);
        } else {
            handleToken(jwtPayload);
        }
        if (!isReady) {
            setIsReady(true);
        }
    }, [jwtPayload])
    useEffect(() => {
        if (user) {
            logger.debug("User identity loaded. User Id: {id}. User Name: {name}. Email: {email}.",
                {
                    id: user.profile.id ?? '',
                    name: user.profile.name ?? '',
                    email: user.profile.email ?? ''
                });
        }
    }, [identity])
    const user: User | null = (token && identity) ? {
        accessToken: token,
        claims: identity,
        profile: {
            id: identity.findFirst(ClaimTypes.Subject)?.value,
            name: identity.findFirst(ClaimTypes.Name)?.value,
            email: identity.findFirst(ClaimTypes.Email)?.value,
        },
        metadata: {
            iat: identity.findFirst(ClaimTypes.IssuedAtTime)?.value,
            aud: identity.findFirst(ClaimTypes.Audience)?.value,
            iss: identity.findFirst(ClaimTypes.Issuer)?.value,
            exp: identity.findFirst(ClaimTypes.ExpirationTime)?.value,
        },
    } : null;
    const events: AuthEvents = {
        addUserLoaded: UserLoadedEvent.addListener,
        removeUserLoaded: UserLoadedEvent.removeListener,
        addUserSignedOut: UserSignedOutEvent.addListener,
        removeUserSignedOut: UserSignedOutEvent.removeListener,
        addAccessTokenExpiring: TokenExpiringEvent.addListener,
        removeAccessTokenExpiring: TokenExpiringEvent.removeListener,
    }
    const actions: AuthActions = {
        silentRefresh,
        handleGoogleSso,
        signout
    }
    const state: AuthState = isReady ? (user ?
            {events, actions, isReady, isAuthenticated: true, user: user} :
            {events, actions, isReady, isAuthenticated: false}) :
        {events, actions, isReady, isAuthenticated: false};

    function handleToken(token: JwtPayload) {
        logger.debug("Access token loaded, building new user profile")
        const claims = Object.keys(token).map(key => new Claim(key, token[key].toString()));
        const claimsIdentity = new ClaimsIdentity(claims);
        setIdentity(claimsIdentity);
        IdentityChangedEvent.dispatch({detail: claimsIdentity});
    }

    async function handleGoogleSso(idToken: string): Promise<SignInResult> {
        const response = await AuthorizationsApi.authorizeGoogleSignin(idToken);
        if (!response.isSuccessStatusCode) {
            clear();
            logger.error(response.data, 'Signin through Google SSO failed');
            return {succeeded: false, details: response.data};
        }
        const token = response.data.accessToken;
        setToken(token);
        return {
            succeeded: true
        };
    }

    async function signout(): Promise<void> {
        try {
            const response = await AuthorizationsApi.signout();
            if (!response.isSuccessStatusCode) {
                logger.error(response.data, "Server error while signing out. See error for details.");
                return;
            }
        } catch (e) {
            logger.error(e as Error, 'An unexpected error occurred during the signout sequence');
            throw e;
        } finally {
            clear();
        }
    }

    async function silentRefresh(): Promise<SignInResult> {
        const response = await AuthorizationsApi.refreshToken();
        if (!response.isSuccessStatusCode) {
            clear();
            logger.error(response.data, 'Attempt to sign in user silently has failed');
            return {succeeded: false, details: response.data};
        }
        const token = response.data.accessToken;
        setToken(token);
        return {
            succeeded: true
        };
    }

    return (
        <AuthContext.Provider value={state}>
            <AxiosConfig/>
            <SilentRefresh/>
            {children}
        </AuthContext.Provider>
    );
}

export default React.memo(AuthProvider);
