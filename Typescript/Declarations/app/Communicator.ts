// import * as _ from "lodash";
import * as communicatorModularUMD from 'communicatorModularUMD';

class Communicator {
    constructor() {}

    greet(message: string): string {
        return communicatorModularUMD.greet(message);
    }
}

var communicator = new Communicator();
document!.body.innerHTML = communicator.greet("Hello, World");