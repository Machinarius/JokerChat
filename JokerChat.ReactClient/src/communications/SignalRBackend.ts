import JokerIdentity from "../models/JokerIdentity"
import JokerMessage from "../models/JokerMessage"

import * as SignalR from "@aspnet/signalr"

interface SignalRHandlers {
    OnMessageReceived: (message: JokerMessage) => void;
}

export default class SignalRBackend {
    private _connection: SignalR.HubConnection;
    private _announcementToken: { userId: string, username: string};
    private _handlers: SignalRHandlers;

    constructor(
        identity: JokerIdentity | { Id: string, Username: string },
        handlers: SignalRHandlers) {
        this._announcementToken = {
            userId: identity.Id,
            username: identity.Username
        };
        this._handlers = handlers;
        
        var tokenString = JSON.stringify(this._announcementToken);
        this._connection = new SignalR.HubConnectionBuilder()
            .withUrl("http://localhost:32460/jokerhub", {
                accessTokenFactory: () => tokenString
            })
            .build();
        this._connection.on("receiveMessage", this.onReceiveMessage);

        this.onReceiveMessage = this.onReceiveMessage.bind(this);
    }

    public async connectToServerAsync() : Promise<void> {
        await this._connection.start();
        await this._connection.send("PerformAnnouncement", this._announcementToken);
    }

    public async subscribeToConversation(conversationId: string) : Promise<void> {
        await this._connection.send("SubscribeToConversation", {
            conversationId: conversationId,
            ... this._announcementToken
        });
    }

    private onReceiveMessage(messagePayload: any) {
        var id = messagePayload.Id as string;
        var senderId = messagePayload.SenderId as string;
        var senderUsername = messagePayload.SenderUsername as string;
        var content = messagePayload.Content as string;
        var dateValue = messagePayload.Date as string;
        var conversationId = messagePayload.ConversationId as string;
        var date = new Date(Date.parse(dateValue));

        var messageObject = new JokerMessage(content, conversationId, {
            Id: senderId,
            Username: senderUsername
        }, {
            MessageId: id,
            DateSent: date
        });
        this._handlers.OnMessageReceived(messageObject);
    }
}