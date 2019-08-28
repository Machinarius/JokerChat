import SignalRBackend from "./SignalRBackend"
import ConversationStream from "./ConversationStream"
import JokerIdentity from "../models/JokerIdentity";
import JokerMessage from "../models/JokerMessage";

export default class StreamsManager {
    private _streams: { [conversationId: string] : ConversationStream };
    private _backend: SignalRBackend;
    private _initialized: boolean = false;

    constructor(identity: JokerIdentity) {
        this.onMessageReceived = this.onMessageReceived.bind(this);

        this._streams = {};
        this._backend = new SignalRBackend(identity, {
            OnMessageReceived: this.onMessageReceived
        });
    }

    public async initialize() : Promise<void> {
        await this._backend.connectToServerAsync();
        this._initialized = true;
    }

    public getConversationStream(conversationId: string): ConversationStream {
        if (!this._initialized) {
            throw "initialize() must be called before retrieving any streams";
        }

        var stream = this._streams[conversationId];
        if (!stream) {
            stream = new ConversationStream(conversationId);
            this._streams[conversationId] = stream;
        }

        return stream;
    }

    private onMessageReceived(message: JokerMessage) {
        var targetStream = this._streams[message.ConversationId];
        if (!targetStream) {
            targetStream = new ConversationStream(message.ConversationId);
            this._streams[message.ConversationId] = targetStream;

            console.warn("Received a message for an unknown stream - created stream implicitly");
        }

        targetStream.receiveMessage(message);
    }
}