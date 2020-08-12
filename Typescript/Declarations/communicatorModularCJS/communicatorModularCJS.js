var communicatorModularCJS = {};
communicatorModularCJS.greet = function (message) {
    return '<h1>' + message + '</h1>';
}

module.exports = communicatorModularCJS;

// CJS Patterns loads modules synchoronously
// AMD loads modules asynchronously