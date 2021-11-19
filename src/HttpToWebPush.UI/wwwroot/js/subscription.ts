export class SubscriptionHelper {
    private readonly _isSupported: boolean
    private pushServiceWorkerRegistration: Promise<ServiceWorkerRegistration> | null = null

    constructor() {
        this._isSupported = "serviceWorker" in navigator && "PushManager" in window

        if (this._isSupported) {
            this.pushServiceWorkerRegistration = navigator.serviceWorker.register('/sw.js', { scope: '/' })
        } else {
            console.debug("ServiceWorker not supported")
        }
    }

    get isSupported(): boolean {
        return this._isSupported
    }

    private static urlB64ToUint8Array(base64String: String) {
        const padding = '='.repeat((4 - base64String.length % 4) % 4)
        const base64 = (base64String + padding)
            .replace(/\-/g, '+')
            .replace(/_/g, '/')

        const rawData = window.atob(base64)
        const outputArray = new Uint8Array(rawData.length)

        for (let i = 0; i < rawData.length; ++i) {
            outputArray[i] = rawData.charCodeAt(i)
        }
        return outputArray
    }

    public async getCurrentSubscription(): Promise<PushSubscription | null> {
        if (!this._isSupported) {
            return null
        }

        return this.pushServiceWorkerRegistration!!.then(r => r.pushManager.getSubscription())
    }

    public async createSubscription(publicKey: string): Promise<PushSubscription | null> {
        const currentSubscription = await this.getCurrentSubscription()
        if (currentSubscription != null) {
            return currentSubscription
        }

        const options = {
            userVisibleOnly: true,
            applicationServerKey: SubscriptionHelper.urlB64ToUint8Array(publicKey)
        }

        try {
            return await this.pushServiceWorkerRegistration!!.then(r => r.pushManager.subscribe(options))
        } catch (e) {
            console.error(e.message)
        }
    }
}

export interface SubscriptionHelperWindow extends Window {
    SubscriptionHelper: any
}

declare var window: SubscriptionHelperWindow
window.SubscriptionHelper = new SubscriptionHelper()
