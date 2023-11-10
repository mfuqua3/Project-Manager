export function parseTemplateLikeString(str: string): string[] {
    const regex = /(?<!\{)\{([^}]+)\}(?!\})/g;
    let match;
    let lastIndex = 0;
    const result: string[] = [];

    while ((match = regex.exec(str)) !== null) {
        if (match.index !== lastIndex) {
            result.push(str.substring(lastIndex, match.index));
        }

        if (result.length === 0) {
            result.push('');
        }
        result.push(match[1]);
        lastIndex = match.index + match[0].length;
    }

    if (lastIndex !== str.length) {
        result.push(str.substring(lastIndex));
    }
    return result;
}
