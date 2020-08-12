// import * as _ from "lodash";

class Communicator {
    constructor() {}

    greet(message: string): string {
        // return communicationGlobal.greet(message);
        let _settings = new communicatorGlobal.Settings(message);
        communicatorGlobal.settings = _settings;
        return communicatorGlobal.greet();
    }
}

var communicator = new Communicator();
document!.body.innerHTML = communicator.greet("Hello, World");