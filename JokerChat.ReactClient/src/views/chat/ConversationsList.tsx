import React, { Component } from "react";

interface ConversationsListProps {
    CurrentConversationId: string,
    KnownConversations: string[]
}

export default class ConversationsList extends Component<ConversationsListProps, {}> {
    public render() {
        return (<div>List</div>);
    }
}