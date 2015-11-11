$(function () {
    var config = liquidFillGaugeDefaultSettings();
    config.circleThickness = 0.15;
    config.circleColor = "#6DA398";
    config.textColor = "#0E5144";
    config.waveTextColor = "#6DA398";
    config.waveColor = "#246D5F";
    config.textVertPosition = 0.8;
    config.waveAnimateTime = 5000;
    config.waveHeight = 0.1;
    config.waveAnimate = true;
    config.waveRise = true;
    config.waveHeightScaling = true;
    config.waveOffset = 0.25;
    config.textSize = 0.75;
    config.waveCount = 3;
    config.displayPercent = false;

    var fillgaugeAdHominem = loadLiquidFillGauge("fillgaugeAdHominem", 0, config);
    var fillgaugeSexist = loadLiquidFillGauge("fillgaugeSexist", 0, config);
    var fillgaugeRacist = loadLiquidFillGauge("fillgaugeRacist", 0, config);
    var fillgaugeOther = loadLiquidFillGauge("fillgaugeOther", 0, config);

    
    var update = function () {
        $.getJSON("api/hate/stats", function (data) {
            if (data.HateCount > 0) {
                fillgaugeAdHominem.update(data.AdHominemCount, data.HateCount);
                fillgaugeSexist.update(data.SexistCount, data.HateCount);
                fillgaugeRacist.update(data.RacistCount, data.HateCount);
                fillgaugeOther.update(data.OtherCount,  data.HateCount);
                $("#count").text(data.HateCount);
            }
            setTimeout(update, 3000);
        });
    }

    update();
});

