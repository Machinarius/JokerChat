import JokerIdentity from "../models/JokerIdentity";
import JokerMessage from "../models/JokerMessage";
import UUID from "uuid/v4";

import * as SignalR from "@aspnet/signalr"

interface SignalRHandlers {
    OnMessageReceived: (message: JokerMessage) => void;
}

export default class SignalRBackend {
    private _connection: SignalR.HubConnection;
    private _announcementToken: { 
        userId: string, 
        username: string,
        sessionId: string
    };
    private _handlers: SignalRHandlers;

    constructor(
        identity: JokerIdentity | { Id: string, Username: string },
        handlers: SignalRHandlers) {
        this._announcementToken = {
            userId: identity.Id,
            username: identity.Username,
            sessionId: UUID()
        };
        this._handlers = handlers;
        
        var tokenString = JSON.stringify(this._announcementToken);
        this._connection = new SignalR.HubConnectionBuilder()
            .withUrl("http://localhost:32460/jokerhub", {
                accessTokenFactory: () => tokenString
            })
            .build();
            
        this.onReceiveMessage = this.onReceiveMessage.bind(this);
        this._connection.on("receiveMessage", this.onReceiveMessage);
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
        var id = messagePayload.id as string;
        var senderId = messagePayload.senderId as string;
        var senderUsername = messagePayload.senderUsername as string;
        var content = messagePayload.content as string;
        var dateValue = messagePayload.dateSent as string;
        var conversationId = messagePayload.conversationId as string;
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