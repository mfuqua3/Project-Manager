import path from "path";

export type RouteParameterTypes = string | number | boolean;

const apiBase = process.env["REACT_APP_API_ROOT"];
if (!apiBase) {
    throw new Error("An API base url has not been configured");
}

export interface ApiArea {
    baseUrl: string;

    urlForEndpoint(endpoint: string, ...routeParameters: { [key: string]: RouteParameterTypes }[]): string;
}

export function useApiArea(area: string): ApiArea {
    const baseUrl = new URL(area, apiBase);
    return {
        baseUrl: baseUrl.href,
        urlForEndpoint: (endpoint, routeParameters) => urlForEndpoint(baseUrl, endpoint, routeParameters)
    };
}

function urlForEndpoint(url: URL, endpoint: string, ...routeParameters: {
    [key: string]: RouteParameterTypes
}[]): string {
    const routeParametersObject = mergeParameters(...routeParameters);
    const endpointPath = buildTemplatedRoute(endpoint, routeParametersObject);
    const endpointUrl = new URL(url);// {...url, pathname:  path.join(url.pathname, endpointPath)};
    endpointUrl.pathname = path.join(url.pathname, endpointPath);
    return endpointUrl.href;
}

function buildTemplatedRoute(endpoint: string, routeParameters: { [key: string]: RouteParameterTypes }): string {
    const pathParams = endpoint.match(/{(.*?)}/g) || Array.of<string>();
    if (pathParams.length === 0) {
        return endpoint;
    }
    return pathParams.reduce((currentUrl: string, param: string) => {
        // Remove the braces to extract the key
        const key = param.replace(/^{(.*)}$/, '$1');
        if (routeParameters[key] === undefined) {
            throw new Error(`Missing parameter: ${key}`);
        }

        // Replace the parameter in the URL
        return currentUrl.replace(param, routeParameters[key].toString());
    }, endpoint);
}

function mergeParameters(...routeParameters: { [key: string]: RouteParameterTypes }[]): {
    [key: string]: RouteParameterTypes
} {
    const result: { [key: string]: RouteParameterTypes } = {};

    for (const params of routeParameters) {
        for (const key in params) {
            if (result.hasOwnProperty(key)) {
                throw new Error(`Key conflict: The key "${key}" is present in multiple parameter objects.`);
            }
            result[key] = params[key];
        }
    }

    return result;
}