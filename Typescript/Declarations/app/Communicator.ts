import * as communicatorModularAMD from 'communicatorModularAMD';

class Communicator {
    constructor() {}

    greet(message: string): string {
        return communicatorModularAMD.greet(message);
        // return communicatorModularUMD.greet(message);
    }
}

var communicator = new Communicator();
document!.body.innerHTML = communicator.greet("HelloWorld");