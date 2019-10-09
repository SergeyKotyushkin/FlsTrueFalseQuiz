define([],
    function() {
        'use strict';

        return function(params) {
            var showTest = params && params.showTest;
            var userAnswers = params && params.userAnswers;

            userAnswers([]);

            return {
                startTestButtonClick: _startTestButtonClick.bind(null, showTest)
            };
        };

        function _startTestButtonClick(showTestCallback) {
            showTestCallback && showTestCallback();
        }
    }
);
