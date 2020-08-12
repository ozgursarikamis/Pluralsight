// Type definitions for communicatorGlobal.js
// Project: Hello Pluralsight
// Definitions by: Matt Kruczek
/*~ If this library is callable (e.g. can be invoked as myLib(3)),
 *~ include those call signatures here.
 *~ Otherwise, delete this section.
 */

// declare function communicatorGlobal(message:string): string;

declare namespace communicatorGlobal {
    function greet(): string;
    let settings: Settings;
    class Settings {
        constructor(message: string);
        message: string;
    }
}