var captcha = sliderCaptcha({
    id: 'captcha',
    loadingText: 'Loading...',
    failedText: 'Try again',
    barText: 'Slide right to fill',
    repeatIcon: 'fa fa-redo',
    onSuccess: function () {
        setTimeout(function () {
            alert('Your captcha is successfully verified.');
            captcha.reset();
        }, 1000);
    },
    setSrc: function () {
        //return 'https://picsum.photos/' + Math.round(Math.random() * 136) + '.jpg';
    },
});