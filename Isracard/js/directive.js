﻿function enterKey() {
    return {
        link: function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.enterKey);
                    });

                    event.preventDefault();
                }
            });

        }
    };
}
isracard.directive('enterKey', [enterKey]);