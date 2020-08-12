// Type definitions for communicatorModularUMD.js
// Project: Hello Pluralsight
// Definitions by: Matt Kruczek

/*~ This is the module template file. You should rename it to index.d.ts
 *~ and place it in a folder with the same name as the module.
 *~ For example, if you were writing a file for "super-greeter", this
 *~ file should be 'super-greeter/index.d.ts'
 */

/*~ If this module is a UMD module that exposes a global variable 'myLib' when
 *~ loaded outside a module loader environment, declare that global here.
 *~ Otherwise, delete this declaration.
 */

// export as namespace communicatorModularUMD;

export function greet(message:string): string;

export namespace otherFunctions {
    export function goodbye(): string;
}