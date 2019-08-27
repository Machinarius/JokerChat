import JokerMessage from "../../models/JokerMessage";

import React, { Component } from "react";

interface MessageRowProps {
    Message: JokerMessage;
}

export default class MessageRow extends Component<MessageRowProps, {}> {
    constructor(props: MessageRowProps) {
        super(props);
    }

    public render() {
        var message = this.props.Message;
        var dateNode: React.ReactNode;
        if (message.DateSent) {
            dateNode = (<small>{message.DateSent.toLocaleString()}</small>);
        } else {
            dateNode = (<span></span>);
        }

        return (
            <div>
                <p><strong>{message.SenderUsername}</strong> {dateNode}</p>
                <p>{message.Content}</p>
            </div>
        );
    }
}