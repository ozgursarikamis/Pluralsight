import * as communicatorModularCJS from 'communicatorModularCJS';

class Communicator {
    constructor() {}

    greet(message: string): string {
        return communicatorModularCJS.greet(message);
    }
}

var communicator = new Communicator();
document!.body.innerHTML = communicator.greet("HelloWorld");