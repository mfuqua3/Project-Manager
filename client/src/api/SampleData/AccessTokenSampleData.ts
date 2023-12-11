import { TokenModel } from "../../domain/models";
import crypto from 'crypto';
import moment from "moment";

function base64UrlEncode(str: Buffer): string {
    return str.toString('base64')
        .replace(/\+/g, '-')
        .replace(/\//g, '_')
        .replace(/=/g, '');
}

function getSampleToken(): TokenModel {
    const sub = "202ab618-74fb-41dc-a18d-c0b9096adcb9";
    const name = "Test User";
    const email = "admin@fake.com";
    const issuer = "offline issuer";
    const audience = "offline project manager app";
    const iat = moment().unix();
    const exp = moment().add(5, 'minutes').unix();

    const payload = {
        sub,
        name,
        email,
        iss: issuer,
        aud: audience,
        iat,
        exp
    };

    const header = {
        alg: "HS256",
        typ: "JWT"
    };

    const secret = "4e9920f8b4f7ae0b8f746aa07a6e312";

    const encodedHeader = base64UrlEncode(Buffer.from(JSON.stringify(header), 'utf8'));
    const encodedPayload = base64UrlEncode(Buffer.from(JSON.stringify(payload), 'utf8'));

    const signature = crypto.createHmac('sha256', secret).update(encodedHeader + '.' + encodedPayload).digest('base64');
    const accessToken = `${encodedHeader}.${encodedPayload}.${base64UrlEncode(Buffer.from(signature, 'utf8'))}`;
    return { accessToken: accessToken } as TokenModel;
}

export default getSampleToken;