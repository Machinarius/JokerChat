import JokerMessage from "../models/JokerMessage";
import Axios, { AxiosInstance } from "axios"

interface StreamSubscription {
    ReceiverFunc: (message: JokerMessage) => void;
    Handle: number;
}

export default class ConversationStream {
    private _conversationId: string;
    private _subscriptions: StreamSubscription[] = [];
    private _subsPointer: number = 0;
    private _httpClient: AxiosInstance;
    private _userId: string;

    constructor(conversationId: string, userId: string) {
        this._conversationId = conversationId;        
        this._userId = userId;
        this._httpClient = Axios.create({
            baseURL: "http://localhost:2205/"
        });
    }

    public get ConversationId(): string {
        return this._conversationId;
    }

    public async connectToServer() {
        await this._httpClient.post("/Subscriptions/SubscribeToConversation", null, {
            params: {
                conversationId: this._conversationId,
                userId: this._userId
            }
        });
    }

    public subscribe(receiverFunc: (message: JokerMessage) => void): number {
        var handle = this._subsPointer;
        var subscription = {
            ReceiverFunc: receiverFunc,
            Handle: handle
        };
        this._subscriptions.push(subscription);

        this._subsPointer = this._subsPointer + 1;
        return handle;
    }

    public async sendMessage(message: JokerMessage) : Promise<void> {
        await this._httpClient.post("/Messages/SendMessage", message);
    }

    public receiveMessage(message: JokerMessage) {
        this._subscriptions.forEach(subscription => {
            subscription.ReceiverFunc(message);
        });
    }
}