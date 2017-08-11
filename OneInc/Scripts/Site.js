window.AudioContext = window.AudioContext || window.webkitAudioContext;

var audioContext = new AudioContext();
var audioInput = null,
    realAudioInput = null,
    inputPoint = null,
    audioRecorder = null;

function gotBuffers(buffers) {
    audioRecorder.exportWAV(doneEncoding);
}

function doneEncoding(blob) {
    var formData = new FormData();

    formData.append('questionId', $('#questionId').data('val'));
    formData.append('blob', blob);

    $.ajax('/Result/Upload', {
        method: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.success) {
                if (data.isLastQuestion) {
                    $('#finishModal').modal();
                }
                loadPage();
            }
            else {
                alert("К сожалению, Ваш выбор не удалось распознать. Попробуйте ещё!")
            }
        }
    });
}

function gotStream(stream) {
    inputPoint = audioContext.createGain();

    // Create an AudioNode from the stream.
    realAudioInput = audioContext.createMediaStreamSource(stream);
    audioInput = realAudioInput;
    audioInput.connect(inputPoint);

    audioRecorder = new Recorder(inputPoint);

    zeroGain = audioContext.createGain();
    zeroGain.gain.value = 0.0;
    inputPoint.connect(zeroGain);
    zeroGain.connect(audioContext.destination);
}

function initAudio() {
    if (!navigator.getUserMedia)
        navigator.getUserMedia = navigator.webkitGetUserMedia || navigator.mozGetUserMedia;
    if (!navigator.cancelAnimationFrame)
        navigator.cancelAnimationFrame = navigator.webkitCancelAnimationFrame || navigator.mozCancelAnimationFrame;
    if (!navigator.requestAnimationFrame)
        navigator.requestAnimationFrame = navigator.webkitRequestAnimationFrame || navigator.mozRequestAnimationFrame;

    navigator.getUserMedia(
        {
            "audio": {
                "mandatory": {
                    "googEchoCancellation": "false",
                    "googAutoGainControl": "false",
                    "googNoiseSuppression": "false",
                    "googHighpassFilter": "false"
                },
                "optional": []
            },
        }, gotStream, function (e) {
            alert('Error getting audio');
            console.log(e);
        });
}

window.addEventListener('load', initAudio);


$('#rec-action').click(function () {
    if ($(this).attr('fn-action') === 'rec') {
        $('span', this).removeClass('glyphicon-play').addClass('glyphicon-stop');
        $(this).attr('fn-action', 'stop');

        audioRecorder.clear();
        audioRecorder.record();
    }
    else {
        $('span', this).removeClass('glyphicon-stop').addClass('glyphicon-play');
        $(this).attr('fn-action', 'rec');

        audioRecorder.stop();
        audioRecorder.getBuffers(gotBuffers);
    }

});

var sendResult = function (res) {
    if (res == 0)
        return;

    var formData = new FormData();

    formData.append('questionId', $('#questionId').data('val'));
    formData.append('optionId', res);

    $.ajax('/Result/AddResult', {
        method: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.success) {
                if (data.isLastQuestion) {
                    $('#finishModal').modal();
                }
                loadPage();
            }
            else {
                alert("К сожалению, Ваш выбор не удалось распозгать. Попробуйте ещё!")
            }
        }
    });
}

var loadPage = function (page) {
    if (page == undefined)
        page = $('.pagination .active').data('page') + 1;
    if (page == undefined)
        return;

    $.ajax('/home/GetPage', {
        data: { page: page },
        success: function (data) {
            $("#questions").html(data);
            $('.pagination .active').removeClass('active');
            $('[data-page=' + page + ']').addClass('active');

            $('.pagination .prev').off().click(function () {
                loadPage(page - 1);
            });
            $('.pagination .next').off().click(function () {
                loadPage(page + 1);
            });
        }
    });
}
