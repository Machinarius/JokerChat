import JokerMessage from "../models/JokerMessage";

interface StreamSubscription {
    ReceiverFunc: (message: JokerMessage) => void;
    Handle: number;
}

export default class ConversationStream {
    private _conversationId: string;
    private _subscriptions: StreamSubscription[] = [];
    private _subsPointer: number = 0;

    constructor(conversationId: string) {
        this._conversationId = conversationId;
    }

    public connectToServer() {
        //this.beginDummyMessageStream();
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

    public sendMessage(message: JokerMessage) {
        this._subscriptions.forEach(subscription => {
            subscription.ReceiverFunc(message);
        });
    }

    /*
    private beginDummyMessageStream() {
        this.sendDummyMessage = this.sendDummyMessage.bind(this);
        setTimeout(this.sendDummyMessage, 1500);
    }

    private sendDummyMessage() {
        var dummyMessage = new JokerMessage();
        dummyMessage.Content = "Dummy debug message";
        dummyMessage.Id = "DummyId";
        dummyMessage.SenderId = "Dummy";
        dummyMessage.SenderUsername = "DummyUser";
        dummyMessage.DateSent = new Date();
        this.sendMessage(dummyMessage);

        setTimeout(this.sendDummyMessage, 1500);
    }
    */
}