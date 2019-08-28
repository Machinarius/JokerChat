import JokerIdentity from "./JokerIdentity";
import UUID from "uuid/v4";

export default class JokerMessage {
    public Id: string;
    public SenderId: string;
    public SenderUsername: string;
    public DateSent: Date;
    public Content: string;
    public ConversationId: string;

    constructor(content: string, conversationId: string,
        sender: JokerIdentity | { Id: string, Username: string }, 
        internalData?: { MessageId: string, DateSent: Date } | undefined) {
        if (internalData) {
            this.Id = internalData.MessageId;
            this.DateSent = internalData.DateSent;
        } else {
            this.Id = UUID();
            this.DateSent = new Date();
        }

        this.Content = content;
        this.ConversationId = conversationId;
        this.SenderId = sender.Id;
        this.SenderUsername = sender.Username;
        this.DateSent = new Date();
    }
}