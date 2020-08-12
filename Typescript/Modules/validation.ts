export namespace Validation {
    export interface SingleValidator {
        isAcceptable(s: string): boolean;
    }

    const lettersRegexp = /^[A-Za-z]+$/;
    const numberRegexp = /^[0-9]+$/;
    
    export class LettersOnlyValidator implements SingleValidator {
        isAcceptable(s: string): boolean {
            return lettersRegexp.test(s);
        }
    }

    export class ZipCodeValidator implements SingleValidator {
        isAcceptable(s: string): boolean {
            return s.length === 5 && 
                numberRegexp.test(s);
        }
    }
}