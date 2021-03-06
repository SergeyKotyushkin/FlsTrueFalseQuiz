define('text!apps/quiz/components/finish/finish.html', [], function () { return '<div class="content-v-centered indent-b">\r\n    <div class="bg-colored indent-all">\r\n        <div class="cosmos"></div>\r\n        <h2 class="centered-h"><br>Результаты были отправлены на указанный email адрес</h2>\r\n    </div>\r\n</div>';});
define('text!apps/quiz/components/intro/intro.html', [], function () { return '<div class="intro-background"></div>\r\n<div class="intro-header-background"></div>\r\n<div class="intro-text-background">\r\n    <svg viewBox="0 0 75 8" style="width: 100%">\r\n        <text x="0" y="7">First Line Software</text>\r\n    </svg>\r\n</div>\r\n<div class="intro-content indent-b">\r\n    <h2>Правда или вымысел</h2>\r\n    <p>Выведи нас на чистую воду<br/>Самые занимательные IT-факты</p>\r\n    <div>\r\n        <input readonly class="form-button button-transparent" data-bind="click: startTestButtonClick" onfocus="this.blur()" value="Туть" />\r\n    </div>\r\n</div>';});
define('text!apps/quiz/components/question/question.html', [], function () { return '<div class="polls-form">\r\n    <!-- ko if: text.length -->\r\n        <div class="polls-form-header">\r\n            <h2 data-bind="text: text"></h2>\r\n        </div>\r\n    <!-- /ko -->\r\n\r\n    <div class="polls-form-content indent-all">\r\n        <div class="polls-counter"><span class="current" data-bind="text: currentQuestionNumber"></span> из <span data-bind="text: countOfQuestions"></span></div>\r\n        <ul class="polls-ul">\r\n            <li class="polls-li">\r\n                <input type="radio" id="poll_1" value="1" data-bind="checked: selectedAnswer"/>\r\n                <label for="poll_1">Правда</label>\r\n            </li>\r\n            <li class="polls-li">\r\n                <input type="radio" id="poll_0" value="0" data-bind="checked: selectedAnswer"/>\r\n                <label for="poll_0">Вымысел</label>\r\n            </li>\r\n        </ul>\r\n        <div class="polls-buttons">\r\n            <input readonly class="form-button" onfocus="this.blur()" value="Продолжить" data-bind="enable: +selectedAnswer() !== -1, click: answerButtonClick" />\r\n        </div>\r\n    </div>\r\n</div>';});
define('text!apps/quiz/components/submit/submit.html', [], function () { return '<div class="content-v-centered indent-b w50">\r\n    <div class="bg-colored">\r\n        <div class="sign-up-form">\r\n            <h2>Спасибо! Надеемся, тебе понравилось.<br>Заполни поля ниже и получи мейл с правильными ответами.</h2>\r\n            <div class="fields">\r\n                <div class="field">\r\n                    <label class="field-label" for="form-name">Имя *</label>\r\n                    <input class="field-input" type="text" data-bind="value: name" onkeyup="this.setAttribute(\'value\', this.value);" value="" required placeholder="Имя…*" id="form-name">\r\n                </div>\r\n                <div class="form-tip field-error" data-bind="validationMessage: name"></div>\r\n                <div class="field">\r\n                    <label class="field-label" for="form-email">E-mail *</label>\r\n                    <input class="field-input" data-bind="value: email" onkeydown="return event.key != \' \'" onkeyup="this.setAttribute(\'value\', this.value);" value="" placeholder="me@example.com…*" id="form-email">\r\n                </div>\r\n                <div class="form-tip field-error" data-bind="validationMessage: email"></div>\r\n                <div class="form-tip">Результаты будут отправлены на указанный почтовый ящик</div>\r\n                <div class="field">\r\n                    <select class="field-select" id="form-stack" onchange="this.setAttribute(\'value\', this.value);" value="" data-bind="value: stack, options: [\'.NET\',\'Java\',\'Frontend\',\'Mobile\',\'Testing\',\'Другое\'], optionsCaption: \'Стек технологий…*\'"></select>\r\n                </div>\r\n                <div class="form-tip field-error" data-bind="validationMessage: stack"></div>\r\n                <div class="field">\r\n                    <label class="field-label" for="form-phone">Телефон</label>\r\n                    <input class="field-input" type="tel" data-bind="value: phone" onkeyup="this.setAttribute(\'value\', this.value);" value="" placeholder="Телефон…" id="form-phone">\r\n                </div>\r\n                <div class="field margin-t">\r\n                    <label class="field-label" for="form-message">Сообщение</label>\r\n                    <textarea class="field-textarea" rows="10" cols="45" data-bind="value: comment" onkeyup="this.setAttribute(\'value\', this.value);" value="" placeholder="Что знаю/умею/предпочитаю" id="form-message"></textarea>\r\n                </div>\r\n            </div>\r\n            <div class="sign-up-buttons">\r\n                <input readonly class="form-button" data-bind="click: submitButtonClick" onfocus="this.blur()" value="Отправить" />\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>';});
define('text!apps/quiz/components/test/test.html', [], function () { return '<div class="content-v-centered indent-b">\r\n    <div class="bg-colored">\r\n\r\n        <!-- ko if: currentQuestion -->\r\n            <!-- ko component: { \r\n                name: "question", \r\n                params: { \r\n                    question: currentQuestion(), \r\n                    nextQuestionHandler: nextQuestionClick, \r\n                    addUserAnswer: addUserAnswer, \r\n                    currentQuestionNumber: currentQuestionNumber, \r\n                    countOfQuestions: countOfQuestions \r\n                } \r\n            } -->\r\n            <!-- /ko -->\r\n        <!-- /ko -->\r\n    </div>\r\n</div>';});

define('apps/quiz/components/finish/finish', [],
    function() {
        'use strict';

        return function() {
            return {};
        };
    }
);

define('apps/quiz/components/intro/intro', [],
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

define('apps/quiz/components/question/question', [
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

define('apps/quiz/components/submit/submit', [
        'knockout',
        'jquery',
        'knockout.validation'
    ],
    function(ko, $) {
        'use strict';

        return function(params) {
            var self = this;

            self.email = ko.observable();
            self.name = ko.observable();
            self.stack = ko.observable();
            self.phone = ko.observable();
            self.comment = ko.observable();

            _initValidation.call(self);
            
            self.loading = params && params.loading;
            self.userAnswers = params && params.userAnswers || [];
            self.showFinish = params && params.showFinish;
            self.showModalErrorMessage = params && params.showModalErrorMessage;

            self.isReadyForSubmit = ko.pureComputed(_isReadyForSubmit.bind(self));

            var submitButtonClick = _submit.bind(self);

            window.scrollTo(0, 0);

            return {
                email: self.email,
                name: self.name,
                stack: self.stack,
                phone: self.phone,
                comment: self.comment,
                submitButtonClick: submitButtonClick,
                isReadyForSubmit: self.isReadyForSubmit
            };
        };

        function _initValidation() {
            var self = this;
            
            ko.validation.init({ insertMessages: false }, true);
            
            self.name.extend({
                required: { message: 'Имя должно быть заполнено!' }
            });
            
            self.email.extend({
                required: { message: 'Email должен быть заполнен!' },
                email: { message: 'Некорректный Email!' }
            });
            
            self.stack.extend({
                required: { message: 'Стек технологий должен быть заполнен!' }
            });

            self.errors = ko.validation.group(self);
        }

        function _isReadyForSubmit() {
            var self = this;

            return self.userAnswers &&
                ko.unwrap(self.userAnswers()).length &&
                self.isValid();
        }

        function _submit() {
            var self = this;

            if (self.errors().length) {
                self.errors.showAllMessages(true);
                return;
            }

            _saveResults.call(self);
        }

        function _saveResults() {
            var self = this;
            var genericErrorMsg = 'Упс, что-то пошло не так. Приносим наши извинения за причиненные неудобства. :(';

            self.loading(true);
            $.post('/result/saveResults',
                    {
                        email: self.email(),
                        name: self.name(),
                        stack: self.stack(),
                        phone: self.phone(),
                        comment: self.comment(),
                        userAnswers: self.userAnswers()
                    },
                    function _onSuccess(response) {
                        if (response.hasErrors) {
                            if (response.usedEmail) {
                                self.showModalErrorMessage('Этот email уже участвовал. Чтобы отправить результаты повторно, нужно указать другой email.');
                                return;
                            }

                            var emailWasNotSent = 'Упс, что-то пошло не так. Произошла ошибка при отправке письма с результатами по указанному адресу. :(';
                            if (response.mailSendError) {
                                self.showModalErrorMessage(emailWasNotSent);
                                return;
                            }

                            if (!response.mailSent) {
                                self.showModalErrorMessage(response.mailSendError
                                    ? emailWasNotSent
                                    : genericErrorMsg);
                                return;
                            }

                            self.showModalErrorMessage(emailWasNotSent);
                            return;
                        }

                        self.showFinish();
                    },
                    'json'
                )
                .fail(function _onError() {
                    self.showModalErrorMessage(genericErrorMsg);
                })
                .always(function _always() {
                    self.loading(false);
                });
        }
    }
);

define('apps/quiz/components/test/test', [
        'knockout',
        'jquery',
        'json!/settings/quizOptions'
    ],
    function(ko, $, options) {
        'use strict';

        return function(params) {
            var self = this;

            var settings = options && options.settings || {};

            self.loading = params && params.loading;
            self.userAnswers = params && params.userAnswers;
            self.showSubmit = params && params.showSubmit;
            self.showIntro = params && params.showIntro;
            self.addUserAnswer = params && params.addUserAnswer;
            self.showModalErrorMessage = params && params.showModalErrorMessage;

            self.currentQuestion = ko.observable();
            self.currentQuestionNumber = ko.observable(0);

            self.countOfQuestions = settings.countOfQuestions || 1;
            
            _loadQuestion.call(self);

            return {
                nextQuestionClick: _nextQuestionClick.bind(self),
                currentQuestion: self.currentQuestion,
                addUserAnswer: self.addUserAnswer,
                currentQuestionNumber: self.currentQuestionNumber,
                countOfQuestions: self.countOfQuestions
            };
        };

        function _nextQuestionClick() {
            var self = this;

            if (self.currentQuestionNumber() === self.countOfQuestions) {
                self.showSubmit();
                return;
            }

            _loadQuestion.call(self);
        }

        function _loadQuestion() {
            var self = this;
            var genericErrorMsg = 'Упс, что-то пошло не так. Приносим наши извинения за причиненные неудобства. :(';

            self.loading(true);
            $.post('/question/getRandom',
                    {
                        excludedQuestionsIds: self.userAnswers().map(_mapQuestionId)
                    },
                    function _onSuccess(result) {
                        if (!result || !result.question) {
                            self.showModalErrorMessage(genericErrorMsg, function() {
                                if (self.currentQuestionNumber() === 0) {
                                    self.showIntro();
                                }
                            });

                        }

                        self.currentQuestion(result.question);
                        _incrementCurrentQuestionNumber.call(self);
                    },
                    'json'
                )
                .fail(function _onError() {
                    self.showModalErrorMessage(genericErrorMsg, function() {
                        if (self.currentQuestionNumber() === 0) {
                            self.showIntro();
                        }
                    });
                })
                .always(function _always() {
                    self.loading(false);
                });
        }

        function _mapQuestionId(userAnswer) {
            return userAnswer.questionId;
        }

        function _incrementCurrentQuestionNumber() {
            var self = this;

            self.currentQuestionNumber(self.currentQuestionNumber() + 1);
        }
    }
);

define('components', ['finish', 'intro', 'question', 'submit', 'test']);

define('finish', ['text!apps/quiz/components/finish/finish.html', 'apps/quiz/components/finish/finish']);
define('intro', ['text!apps/quiz/components/intro/intro.html', 'apps/quiz/components/intro/intro']);
define('question', ['text!apps/quiz/components/question/question.html', 'apps/quiz/components/question/question']);
define('submit', ['text!apps/quiz/components/submit/submit.html', 'apps/quiz/components/submit/submit']);
define('test', ['text!apps/quiz/components/test/test.html', 'apps/quiz/components/test/test']);
