import * as _ from "lodash";

class Communicator {
    constructor() {}

    greet(message: string): string {
        return `<h1>${_.toUpper(message)}</h1>`
    }
}

var communicator = new Communicator();
document!.body.innerHTML = communicator.greet("Hello World");