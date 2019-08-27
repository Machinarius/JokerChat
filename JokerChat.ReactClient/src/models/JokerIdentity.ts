import UUID from "uuid/v4";

export default class JokerIdentity {
    public Id: string = "";
    public Username: string = "";
    public Conversations: string[] = [];

    constructor(username: string) {
        this.Username = username;
        this.Id = UUID();
        this.Conversations = [];
    }
}