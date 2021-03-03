import { DependencyList, useEffect } from "react";

export interface UseThrottleEffectProps<T> {
    action: (data: T) => Promise<void>;
    timeout: number;
    data: T;
}

export function useThrottle<T>({ action, timeout, data }: UseThrottleEffectProps<T>, deps: DependencyList) {
    return useEffect(() => {
        let timeoutId: number | null = null;
        let active = true;
        const cleanup = () => {
            active = false;
            if (timeoutId) {
                clearTimeout(timeoutId);
                timeoutId = 0;
            }
        };

        timeoutId = window.setTimeout(async () => {
            if (active) {
                try {
                    await action(data);
                } finally {
                    cleanup();
                }
            }
        }, timeout);
        return cleanup;
    }, deps);
}
