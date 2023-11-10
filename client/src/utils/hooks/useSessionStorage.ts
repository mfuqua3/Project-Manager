import {Dispatch, useState} from "react";

export type SessionStorage = [(string | null), Dispatch<string | null>]

export function useSessionStorage(key: string): SessionStorage {
    const [value, setValueInternal] = useState<string | null>(() => getValue());

    function setValue(value: string | null): void {
        if (value === null) {
            clearValue();
            return;
        }
        sessionStorage.setItem(key, value);
        setValueInternal(value);
    }

    function getValue(): string | null {
        return sessionStorage.getItem(key);
    }

    function clearValue(): void {
        sessionStorage.removeItem(key);
        setValueInternal(null);
    }

    return [value, setValue];
}
