(function (root, factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD
        define([], factory);
    } else if(typeof exports === 'object' && module.exports) {
        // Common JS
        module.exports = factory();
    } else {
        // Browser globals (Note: root is window)
        root.communicatorModularUMD = factory();
    }
}(this, function () {
    // methods:
    function greet(message) {
        return '<h1>' + message + '</h1>';
    }
    // exposed public methods:
    return { greet: greet }
}));