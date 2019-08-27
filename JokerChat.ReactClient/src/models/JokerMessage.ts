import JokerIdentity from "./JokerIdentity";
import UUID from "uuid/v4";

export default class JokerMessage {
    public Id: string;
    public SenderId: string;
    public SenderUsername: string;
    public DateSent: Date;
    public Content: string;

    constructor(content: string, sender: JokerIdentity | { Id: string, Username: string }) {
        this.Id = UUID();
        this.Content = content;
        this.SenderId = sender.Id;
        this.SenderUsername = sender.Username;
        this.DateSent = new Date();
    }
}