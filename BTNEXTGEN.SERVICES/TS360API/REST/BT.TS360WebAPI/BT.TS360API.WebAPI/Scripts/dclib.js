var dcObject = new Object();
dcObject.uri = 'cart/DistributedCache';
dcObject.TS360SystemNotifCacheKey = "___TS360_SystemNotification";

dcObject.PageLoad = function () {
    $("#hrefHomePage").click(function () {
        dcObject.ShowHomePage();
    });

    $("#hrefManageCachePage").click(function () {
        dcObject.ShowManageCachePage();
    });

    $("#hrefCleanSpecificCache").click(function () {
        dcObject.ShowSpecificCacheRegion();
    });

    $("#hrefCleanSystemNotification").click(function () {
        dcObject.ShowSystemNotificationCacheRegion();
    });

    $("#hrefCleanAllCache").click(function () {
        dcObject.ShowAllCacheRegion();
    });
};

dcObject.CleanSpecificCacheKey = function () {
    dcObject.SetStateForButton("btnCleanSpecificCache", true);
    var postParam = new Object();
    postParam.CacheName = $("#selectCache").val();;
    postParam.CacheKey = $.trim($("#txtCacheKey").val());

    if (postParam.CacheKey == "" && postParam.CacheName != "") {
        if (confirm("This action will remove all cache data of \"" + $("#selectCache option:selected").text() + "\". Is this what you want to do?")) {
            dcObject.InvokeFunctionPost("CleanSpecificCache", postParam, function(result) {
                alert("Clean Successful.");
                dcObject.SetStateForButton("btnCleanSpecificCache", false);
            }, function(errorCode) {
                alert(errorCode);
                dcObject.SetStateForButton("btnCleanSpecificCache", false);
            });
        } else {
            dcObject.SetStateForButton("btnCleanSpecificCache", false);
        }
    }else {
        if (postParam.CacheName == "") {
            alert("Please select cache name to clean");
            dcObject.SetStateForButton("btnCleanSpecificCache", false);
        } else {
            dcObject.InvokeFunctionPost("CleanSpecificCache", postParam, function (result) {
                alert("Clean Successful.");
                dcObject.SetStateForButton("btnCleanSpecificCache", false);
            }, function (errorCode) {
                alert(errorCode);
                dcObject.SetStateForButton("btnCleanSpecificCache", false);
            });
        }
    }
};

dcObject.CleanSystemNotification = function () {
    dcObject.SetStateForButton("btnCleanSystemNotificationCache", true);
    var postParam = new Object();
    postParam.CacheName = "";
    postParam.CacheKey = dcObject.TS360SystemNotifCacheKey;

    if (confirm("This action will remove cache data of System Notification. Is this what you want to do?")) {
        dcObject.InvokeFunctionPost("CleanSpecificCache", postParam, function (result) {
            alert("Clean Successful.");
            dcObject.SetStateForButton("btnCleanSystemNotificationCache", false);
        }, function (errorCode) {
            alert(errorCode);
            dcObject.SetStateForButton("btnCleanSystemNotificationCache", false);
        });
    } else {
        dcObject.SetStateForButton("btnCleanSystemNotificationCache", false);
    }
};

dcObject.CleanAllCache = function () {
    dcObject.SetStateForButton("btnCleanSystemCache", true);
    var postParam = new Object();
    postParam.CacheName = "";
    postParam.CacheKey = "";

    if (confirm("This action will remove all cache data of System. Is this what you want to do?")) {
        dcObject.InvokeFunctionPost("CleanSpecificCache", postParam, function (result) {
            alert("Clean Successful.");
            dcObject.SetStateForButton("btnCleanSystemCache", false);
        }, function(errorCode) {
            alert(errorCode);
            dcObject.SetStateForButton("btnCleanSystemCache", false);
        });
    } else {
        dcObject.SetStateForButton("btnCleanSystemCache", false);
    }
};

dcObject.InvokeFunctionPost = function (functionName, postData, successCallback, failCallback) {
    var webServiceUrl = dcObject.uri +  "/" + functionName;
    $.ajax({
        type: "POST",
        dataType: 'json',
        contentType: "application/json; charset=UTF-8; charset=utf-8",
        traditional: true,
        url: webServiceUrl,
        data: JSON.stringify(postData),
        success: function (result) {
            if (typeof successCallback != "undefined") {
                successCallback(result.d);
            }
        },
        statusCode: {
            404: function () {
                failCallback(errorCode);
            }
        }
    });
};

dcObject.SetStateForButton = function (buttonId, state) {
    if (!state) {
        $('#' + buttonId).attr("disabled", state);
    } else {
        $('#' + buttonId).attr("disabled", "");
    }
};

dcObject.ShowSpecificCacheRegion = function () {
    $("#divCleanSpecificCache").show();
    $("#divCleanSystemNotification").hide();
    $("#divCleanAllSystemCache").hide();
};

dcObject.ShowSystemNotificationCacheRegion = function () {
    $("#divCleanSpecificCache").hide();
    $("#divCleanSystemNotification").show();
    $("#divCleanAllSystemCache").hide();
};

dcObject.ShowAllCacheRegion = function () {
    $("#divCleanSpecificCache").hide();
    $("#divCleanSystemNotification").hide();
    $("#divCleanAllSystemCache").show();
};

dcObject.ShowManageCachePage = function () {
    $("#spanHomeMenu").hide();
    $("#hrefHomePage").show();

    $("#spanManageCacheMenu").show();
    $("#hrefManageCachePage").hide();

    $("#divMainContent").hide();
    $("#divNavigation").show();
    $("#divCleanSpecificCache").show();
    $("#divCleanSystemNotification").hide();
    $("#divCleanAllSystemCache").hide();
};

dcObject.ShowHomePage = function () {
    $("#spanHomeMenu").show();
    $("#hrefHomePage").hide();

    $("#spanManageCacheMenu").hide();
    $("#hrefManageCachePage").show();

    $("#divMainContent").show();
    $("#divNavigation").hide();
    $("#divCleanSpecificCache").hide();
    $("#divCleanSystemNotification").hide();
    $("#divCleanAllSystemCache").hide();
};