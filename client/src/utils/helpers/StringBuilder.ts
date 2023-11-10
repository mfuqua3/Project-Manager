export class StringBuilder {
    private _array: string[];

    constructor() {
        this._array = [];
    }

    public append(value: string): StringBuilder {
        this._array.push(value);
        return this; // to allow chaining
    }

    public appendLine(value: string): StringBuilder {
        this._array.push(value + '\n');
        return this; // to allow chaining
    }

    public toString(): string {
        return this._array.join('');
    }
}