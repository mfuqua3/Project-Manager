export type CustomEventArgs<T = undefined> = Event & { detail: T };
export type CustomEventListener<T = undefined> = (e: CustomEventArgs<T>) => void;

export class CustomEventMethods<T = undefined> {
    constructor(private readonly type: string) {
    }

    addListener(callbackFn: CustomEventListener<T>) {
        window.addEventListener(this.type, callbackFn as EventListener);
    }

    removeListener(callbackFn: CustomEventListener<T>) {
        window.removeEventListener(this.type, callbackFn as EventListener);
    }

    dispatch(eventInitDict: CustomEventInit<T>): void {
        const event = new CustomEvent<T>(this.type, eventInitDict);
        window.dispatchEvent(event);
    }
}