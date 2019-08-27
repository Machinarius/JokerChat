import JokerIdentity from "../models/JokerIdentity";

export default class IdentityStore {
    private readonly IdentityStoreKey: string = "JokerChat.Identity";
    private readonly MiscStoreKey: string = "JokerChat.MiscData";

    public identityExists(): boolean {
        return localStorage.getItem(this.IdentityStoreKey) != null;
    }

    public getStoredIdentity(): JokerIdentity {
        var identityJson = localStorage.getItem(this.IdentityStoreKey);
        if (!identityJson) {
            throw "No identity has been stored";
        }

        var identityObject = JSON.parse(identityJson) as JokerIdentity;
        return identityObject;
    }

    public storeIdentity(identity: JokerIdentity) {
        if (!identity) {
            this.clearStoredIdentity();
            return;
        }

        var identityJson = JSON.stringify(identity);
        localStorage.setItem(this.IdentityStoreKey, identityJson);
    }

    public clearStoredIdentity() {
        localStorage.removeItem(this.IdentityStoreKey);
    }

    public getLastActiveConversationId(): string | null {
        return localStorage.getItem(this.MiscStoreKey);
    }

    public setLastActiveConversationId(conversationId: string) {
        localStorage.setItem(this.MiscStoreKey, conversationId);
    }
}