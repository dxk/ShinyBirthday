/*
* you-may 
* version: 1.0 (08-APR-2009)
* @requires jQuery v1.3.2 or later
*/

var ajaxDefaultOptions = {
    dataType: "json",
    type: "POST",
    dataFilter: jsonFilter,
    error: function(XMLHttpRequest, textStatus, errorThrown) {
        showMessage(XMLHttpRequest + "\n" + textStatus + "\n" + errorThrown);
    }
}

var validatorDefaultOptions = {
    focusCleanup: true,
    onkeyup: false,
    onfocusout: false,
    onclick: false,
    showErrors: function(errorMap, errorList) {
        var errors = [];
        $.each(errorMap, function(key, value) {
            errors.push(value);
        });
        if (errors.length > 0)
            alert(errors.join("\n"));
    }
}

var isRedirecting = false;
function jsonFilter(data, type) {
    //        if (isRedirecting)
    //            return data;

    if (type == "json" && data) {
        var result = (typeof data === "string") ?
                 window["eval"]("(" + data + ")") :
                 data;

        data = null;

        switch (result.ResultType) {
            case 1:
                data = result.Data;
                break;
            case 2:
                showMessage(result.Message);
                break;
            //            case 3:                      
            //                data = result.Data;                      
            //                showMessage(result.Message);                      
            //                break;                      
            case 4:
                window.location.href = result.Url;
                isRedirecting = true;
                break;
            case 6:
                window.location.href = result.Url;
                isRedirecting = true;
                break;
            case 8:
            case 10:
                if (result.TargetId) {
                    var searchForm = $("#" + result.TargetId);
                    searchForm.submit();
                } else {
                    window.location.reload();
                }
                break;
        }
    }
    return data; // 返回处理后的数据
}

function readyErrorMessage() {

    var errorSummary = $("#errorSummary");

    if (errorSummary.size() != 0) {
        var errors = [];

        errorSummary.children("li").each(function() {
            errors.push($(this).text());

        });

        if (errors.length > 0)
            alert(errors.join("\n"));
    }
}

function invokeFunc(func) {
    var args = [];
    for (var i = 1; i < arguments.length; i++) {
        args.push(arguments[i]);
    }
    alert(args);

    func.call(this, args);
}


function confirmPostAction(url, text, success) {
    if (confirm(text))
        postAction.call(this, url, success);
    return false;
}

//ajax post
function postAction(url, success) {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: url,
        success: success
    });
}

//show message
function showMessage(message) {
    alert(message);
}

//ajax post and refresh
function postRefresh(url, selector) {
    postAction(url, function(data) {
        refresh(selector);
    })
}

//refresh
function refresh(selector) {
    selector = selector || "form:first";

    if ($(selector).size())
        $(selector).submit();
    else
        window.location.reload();
}


function getPageSize() {
    var scrW, scrH;
    if (window.innerHeight && window.scrollMaxY) {
        // Mozilla
        scrW = window.innerWidth + window.scrollMaxX;
        scrH = window.innerHeight + window.scrollMaxY;
    } else if (document.body.scrollHeight > document.body.offsetHeight) {
        // all but IE Mac
        scrW = document.body.scrollWidth;
        scrH = document.body.scrollHeight;
    } else if (document.body) { // IE Mac
        scrW = document.body.offsetWidth;
        scrH = document.body.offsetHeight;
    }

    var winW, winH;
    if (window.innerHeight) { // all except IE
        winW = window.innerWidth;
        winH = window.innerHeight;
    } else if (document.documentElement
    && document.documentElement.clientHeight) {
        // IE 6 Strict Mode
        winW = document.documentElement.clientWidth;
        winH = document.documentElement.clientHeight;
    } else if (document.body) { // other
        winW = document.body.clientWidth;
        winH = document.body.clientHeight;
    }

    // for small pages with total size less then the viewport
    var pageW = (scrW < winW) ? winW : scrW;
    var pageH = (scrH < winH) ? winH : scrH;

    return { pageW: pageW, pageH: pageH, winW: winW, winH: winH };
}

function fetchTempForm(hasFile) {
    var tempFormId = "TempPostForm";

    var oldForm = $("#" + tempFormId);
    if (oldForm.size()) {
        oldForm.remove();
    }

    var newForm = $("<form></form>").attr("id", tempFormId).hide();
    if (hasFile) {
        newForm.attr("enctype", "multipart/form-data");
    }

    $("body").append(newForm);

    return newForm;
}

//jquery 扩展
; (function($) {

    $.fn.copyChildsToForm = function(options) {
        options = $.extend({}, options || {});

        var self = this;
        var prefix = options.prefix;

        var hasFile = options.hasFile;

        var clonedInputs = null;

        if (hasFile) {
            clonedInputs = self.find("select,input,textarea").map(function() {
                var old = $(this);
                return old;
            });
        } else {
            clonedInputs = self.find("select,input,textarea").map(function() {
                var old = $(this);
                var clone = old.clone();
                if (old.is(":file")) {
                    hasFile = true;
                }
                clone.val(old.val());
                return clone;
            });
        }

        var form = fetchTempForm(hasFile);

        if (prefix) {
            clonedInputs.each(function() {
                var $this = $(this);
                var name = $this.attr("name");
                if (name.indexOf(prefix) == 0) {
                    var newName = name.substring(prefix.length);

                    $this.attr("name", newName);
                }
            });
        }
        clonedInputs.each(function(){
            $(this).appendTo(form);
        });
       // clonedInputs.appendTo(form);
        return form;
    }

})(jQuery);

