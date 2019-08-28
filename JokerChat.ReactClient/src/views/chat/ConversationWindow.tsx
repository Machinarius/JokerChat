import JokerMessage from "../../models/JokerMessage";
import JokerIdentity from "../../models/JokerIdentity";
import ConversationStream from "../../communications/ConversationStream"

import MessageRow from "./MessageRow";

import React, { Component } from "react";

interface ConversationWindowProps {
    ConversationStream: ConversationStream;
    Identity: JokerIdentity;
}

interface ConversationWindowState {
    ComposedMessage?: string;
    Messages: JokerMessage[];
}

export default class ConversationWindow extends Component<ConversationWindowProps, ConversationWindowState> {
    private _convStream: ConversationStream;
    private _streamHandle: number;

    constructor(props: ConversationWindowProps) {
        super(props);
        this._convStream = this.props.ConversationStream;

        this.onMessageReceived = this.onMessageReceived.bind(this);
        this.onSendMessageClicked = this.onSendMessageClicked.bind(this);
        this.onComposedMessageChanged = this.onComposedMessageChanged.bind(this);
        this.state = {
            Messages: [],
            ComposedMessage: ""
        };

        this._streamHandle = this._convStream.subscribe(this.onMessageReceived);
    } 

    private onMessageReceived(message: JokerMessage) {
        var messages = this.state.Messages;
        messages.unshift(message);

        this.setState({
            Messages: messages
        });
    }

    public onComposedMessageChanged(event: any) {
        this.setState({
            ComposedMessage: event.target.value
        });
    }

    public onSendMessageClicked() {
        if (!this.state.ComposedMessage) {
            return;
        }

        var message = new JokerMessage(this.state.ComposedMessage, this._convStream.ConversationId, this.props.Identity);
        this._convStream.sendMessage(message);
        this.setState({
            ComposedMessage: ""
        });
    }

    public render() {
        var messagesElement: React.ReactNode;
        if (this.state.Messages && this.state.Messages.length > 0) {
            messagesElement = (
                <ul>
                    {this.state.Messages.map(message => <MessageRow Message={message}></MessageRow>)}
                </ul>
            );
        } else {
            messagesElement = (<div></div>);
        }

        return (
            <div>
                {this._convStream.ConversationId}
                <input type="text" value={this.state.ComposedMessage} onChange={this.onComposedMessageChanged}/>
                <button onClick={this.onSendMessageClicked}>Send message</button>
                <div>
                    {messagesElement}
                </div>
            </div>
        );
    }
}