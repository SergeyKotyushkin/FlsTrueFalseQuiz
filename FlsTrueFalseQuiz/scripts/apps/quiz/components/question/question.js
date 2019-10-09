define([
        'knockout'
    ],
    function(ko) {
        'use strict';

        return function(params) {
            var self = this;

            var model = params && params.question || {};

            self.id = model.id;
            self.text = model.text;
            self.answers = model.answers || [];
            self.selectedAnswer = ko.observable(-1);

            self.nextQuestionHandler = params && params.nextQuestionHandler;
            self.addUserAnswer = params && params.addUserAnswer;
            self.currentQuestionNumber = params && params.currentQuestionNumber;
            self.countOfQuestions = params && params.countOfQuestions;

            var answerButtonClick = _answerButtonClick.bind(self);

            window.scrollTo(0, 0);

            return {
                text: self.text,
                imageUrl: self.imageUrl,
                answers: self.answers,
                selectedAnswer: self.selectedAnswer,
                answerButtonClick: answerButtonClick,
                countOfQuestions: self.countOfQuestions,
                currentQuestionNumber: self.currentQuestionNumber
            };
        };

        function _answerButtonClick() {
            var self = this;

            var selectedAnswer = +self.selectedAnswer();
            if (selectedAnswer === -1) {
                return;
            }

            self.addUserAnswer(self.id, !!selectedAnswer);
            self.nextQuestionHandler();
        }
    }
);
