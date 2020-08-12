// (function (window) {
//     var defaultMessage = "Hello Pluralsight!";
//     if (typeof(window.communicatorGlobal) === 'undefined') {
//         window.communicatorGlobal = function (message) {
//             if (message == 'undefined') {
//                 message = defaultMessage;
//             }
//             return '<h1>' + message + '</h1>';
//         }
//     }
// })(window);

(function (window) {
    function communicationGlobal() {
        var _communicationGlobal = {};
        // this emulates a simple method on a TS class

        _communicationGlobal.greet = function (message) {
            return "<h1>" + message + "</h1>";
        };
        return _communicationGlobal;
    }

    if (typeof (window.communicationGlobal) === 'undefined') {
        window.communicationGlobal = communicationGlobal();
    }
})(window);