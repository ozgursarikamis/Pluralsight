// import * as _ from "lodash";

class Communicator {
    constructor() {}

    greet(message: string): string {
        return communicatorModularUMD.greet(message);
    }
}

var communicator = new Communicator();
document!.body.innerHTML = communicator.greet("Hello, World");