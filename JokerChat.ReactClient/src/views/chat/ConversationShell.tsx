import JokerIdentity from "../../models/JokerIdentity";
import IdentityStore from "../../storage/IdentityStore";

import React, { Component } from "react";
import ConversationsList from "./ConversationsList";
import ConversationWindow from "./ConversationWindow";
import StreamsManager from "../../communications/StreamsManager";

interface ConversationShellProps {
    Identity: JokerIdentity;
    StreamsManager: StreamsManager;
}

interface ConversationShellState {
    CurrentConversationId: string;
}

export default class ConversationShell extends Component<ConversationShellProps, ConversationShellState> {
    private _identityStore = new IdentityStore();

    constructor(props: ConversationShellProps) {
        super(props);
        this._identityStore = new IdentityStore();
        var conversationId = this._identityStore.getLastActiveConversationId();
        if (!conversationId) {
            conversationId = props.Identity.Conversations[0];
            if (!conversationId) {
                conversationId = "#general";
            }
        }

        this.state = {
            CurrentConversationId: conversationId
        }
    }

    public render() {
        var convStream = this.props.StreamsManager.getConversationStream(this.state.CurrentConversationId);

        return (
            <div>
                <ConversationsList 
                    CurrentConversationId={this.state.CurrentConversationId}
                    KnownConversations={this.props.Identity.Conversations}>
                </ConversationsList>
                <ConversationWindow
                    Identity={this.props.Identity}
                    ConversationStream={convStream}>
                </ConversationWindow>
            </div>
        )
    }
}