import { Validation } from "./validation";

let strings = ["Hello", "98052", "101"];

let validators: {
    [s: string]: Validation.SingleValidator
} = {};

validators["Zip Code"] = new Validation.ZipCodeValidator();
validators["Letters only"] = new Validation.LettersOnlyValidator();

for (const s of strings) {
    for (let name in validators) {
        console.log(
            `"${s}" - ${
                validators[name].isAcceptable(s) ? "matches" : "does not match"
            } ${name}`
        );
    }
}