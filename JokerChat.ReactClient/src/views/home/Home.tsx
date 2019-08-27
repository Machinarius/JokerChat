import IdentityStore from "../../storage/IdentityStore";
import JokerIdentity from "../../models/JokerIdentity";

import React, { Component } from "react";
import ConversationShell from "../chat/ConversationShell";

interface HomeState {
    Identity?: JokerIdentity;
    DesiredUsername?: string;
    UsernameErrors?: string[];
}

export default class HomeComponent extends Component<{}, HomeState> {
    private _identityStore = new IdentityStore();

    constructor(props: {}) {
        super(props);

        var identity: JokerIdentity | undefined = undefined;
        if (this._identityStore.identityExists()) {
            identity = this._identityStore.getStoredIdentity();
        }

        this.state = {
            Identity: identity
        };

        this.onUserClickedContinue = this.onUserClickedContinue.bind(this);
        this.onUsernameChanged = this.onUsernameChanged.bind(this);
    }

    public onUserClickedContinue() {
        var errors = [];
        if (!this.state.DesiredUsername || this.state.DesiredUsername.length < 3) {
            errors.push("Username values must contain at least 3 characters");
        }

        if (this.state.DesiredUsername && /\s/.test(this.state.DesiredUsername)) {
            errors.push("Usernames may not contain spaces");
        }

        if (!this.state.DesiredUsername) {
            throw "Can not create a user with an undefined Username";
        }

        var username: string = this.state.DesiredUsername;
        var createdIdentity = new JokerIdentity(username);
        this._identityStore.storeIdentity(createdIdentity);
        this.setState({
            Identity: createdIdentity
        });
    }

    public onUsernameChanged(event: any) {
        this.setState({
            DesiredUsername: event.target.value
        });
    }

    public render() {
        if (!this.state.Identity) {
            var errorNode: React.ReactNode;
            if (this.state.UsernameErrors && this.state.UsernameErrors.length > 0) {
                errorNode = (
                    <ul>
                        {this.state.UsernameErrors.map(error => <li>{error}</li>)}
                    </ul>
                );
            } else {
                errorNode = (<div></div>);
            }

            return (
                <div>
                    <h1>Welcome!</h1>
                    <p>Choose a Username to continue</p>
                    <input type="text" value={this.state.DesiredUsername} onChange={this.onUsernameChanged} />
                    {errorNode}
                    <button onClick={this.onUserClickedContinue}>Continue</button>
                </div>
            );
        }

        return (
            <ConversationShell Identity={this.state.Identity}>
            </ConversationShell>
        );
    }
}