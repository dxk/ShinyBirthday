$.fn.cascade = function (options) {
    options = $.extend({
        emptyRequest: false
    }, options || {});

    var trigger = (!options.trigger) ? this : $(options.trigger);
    var target = $(options.target);

    if (!trigger || trigger.size() == 0) {
        //alert("trigger can not null or empty");
        return;
    }

    if (!target || target.size() == 0) {
        //alert("target can not null or empty");
        return;
    }

    if (!options.eventName)
        options.eventName = "change";

    if (!options.myevent) {
        options.myevent = function (e) {

            if (options.before)
                options.before(trigger, target);

            target.children().filter(function () {
                return $(this).val();
            }).remove();

            if (!options.emptyRequest && !$(this).val()) {
                return;
            }

            trigger.closest("form").attr("disabled", "disabled");

            options.url = options.baseUrl.concat($(this).val());

            if (!options.success)
                options.success = function (result) {

                    $.each(result, function (index, view) {
                        $("<option></option>").val(view[options.valueKey])
                            .text(view[options.textKey]).appendTo(target);
                    });

                    trigger.closest("form").removeAttr("disabled");

                    if (options.after)
                        options.after(trigger, target);
                };
            options.async = false;
            $.ajax(options);
        };
    }
    trigger.bind(options.eventName, options.myevent);
}

$.format = function (source, params) {
    if (arguments.length == 1)
        return function () {
            var args = $.makeArray(arguments);
            args.unshift(source);
            return $.validator.format.apply(this, args);
        };
    if (arguments.length > 2 && params.constructor != Array) {
        params = $.makeArray(arguments).slice(1);
    }
    if (params.constructor != Array) {
        params = [params];
    }
    $.each(params, function (i, n) {
        source = source.replace(new RegExp("\\{" + i + "\\}", "g"), n);
    });
    return source;
};


function formatFloat(src, pos) {
    return Math.round(src * Math.pow(10, pos)) / Math.pow(10, pos);
}


function openWindow(href) {

    window.open(href);

    return false;
}

function noSide() {
    $(".fold-line").hide();
    $("#side").hide();
}

function submitForm(options) {

    var form = $(options.self).closest('form');

    options.url = options.url || form.attr("action");

    options = $.extend({
        target: form,
        method: form.attr("method")
    }, options || {});

    submitChilds(options);
}

function submitClosestForm(self, action, extValues, confirmText, confirmAction) {
    var $self = $(self);

    if (confirmText) {
        confirmAction = confirmAction || function (message) {
            return confirm(message);
        };
        if (!confirmAction(confirmText)) {
            return false;
        }
    }

    var form = $self.closest("form");

    if (!form.size()) {
        alert("miss form");
        return false;
    }

    var oldAction = form.attr("action");

    if (action) {
        form.attr({ action: action });
    }

    var extInputs = [];

    if (extValues) {
        $.each(extValues, function (name, value) {

            var extInput = document.createElement("input");
            extInput.setAttribute("type", "hidden");
            extInput.setAttribute("name", name);
            extInput.setAttribute("value", value);
            form.append(extInput);
            extInputs.push(extInput);
        });
    }

    form.submit();

    form.attr({ action: oldAction });

    //清除临时在form中修改的数据
    $(extInputs).remove();
}

function batchSumbit(self, action, confirmText, confirmAction) {

    if (confirmText) {
        confirmAction = confirmAction || function (message) {
            return confirm(message);
        };
        if (!confirmAction(confirmText)) {
            return false;
        }
    }

    var $self = $(self);

    var form = $self.closest("form");

    if (!form.size()) {
        alert("miss form");
        return false;
    }

    form.attr({ method: "post", action: action });
    form.submit();
}

function fetchTempForm() {
    var tempFormId = "TempPostForm";

    var oldForm = $("#" + tempFormId);
    if (oldForm.size()) {
        oldForm.remove();
    }

    var newForm = $("<form></form>").attr("id", tempFormId).hide();
    $("body").append(newForm);

    return newForm;
}

function postConfirm(options) {

    options = options || {};

    var action = options.action;
    var confirmText = options.confirmText;
    var extValues = options.extValues;
    var openNewWindow = options.openNewWindow;

    //有值才确认
    if (confirmText) {
        if (!confirm(confirmText))
            return false;
    }

    var id = action.substring(action.lastIndexOf("/") + 1);

    extValues = $.extend({
        "items[0].Id": id
    }, extValues || {});

    var form = fetchTempForm();

    form.attr({ method: "post", action: action });

    if (extValues) {
        $.each(extValues, function (name, value) {

            var extInput = document.createElement("input");
            extInput.setAttribute("type", "hidden");
            extInput.setAttribute("name", name);
            extInput.setAttribute("value", value);
            form.append(extInput);
        });
    }

    if (openNewWindow) {
        form.attr("target", "_blank");
    }

    form.submit();

    return false;
}

function dependSelect(triggerSelector, targetSelector) {
    var $trigger = $(triggerSelector);
    var $target = $(targetSelector);

    var dependAction = function () {
        var checked = $trigger.is(":checked");

        var options = $target.children();

        if (checked) {
            $target.hide();
            options.attr("selected", "selected");
            $target.show();
        } else {
            options.removeAttr("selected");
        }
    }

    $trigger.click(dependAction);
}



function dependShow(triggerSelector, targetSelector, isShow, showFunc) {

    var trigger = $(triggerSelector);

    var target = $(targetSelector);

    if (trigger.is(":checkbox") || trigger.is(":radio")) {
        if (!isShow) {
            isShow = function () {
                return trigger.is(":checked");
            }
        }
        trigger.click(function () {
            var show = isShow(trigger, target);
            if (show && showFunc) {
                showFunc(trigger, target);
            }

            target.toggle(show);
        });


    } else {
        trigger.change(function () {
            var show = isShow(trigger, target);
            if (show && showFunc) {
                showFunc(trigger, target);
            }
            target.toggle(show);
        });
    }

    var show = isShow(trigger, target);
    if (show && showFunc) {
        showFunc(trigger, target);
    }

    //触发
    target.toggle(show);

}

function toggleDisableChilds(selector, disable) {
    var selector = $(selector);
    selector.find("input,select").each(function () {
        var $this = $(this);
        $this.attr("disabled", disable ? "disabled" : "");
    });
    var datepickerDisableStr = disable ? "disable" : "enable";
    selector.find(".date-picker").datepicker(datepickerDisableStr);
}

function disableChilds(selector) {
    toggleDisableChilds(selector, true);
}
function enableChilds(selector) {
    toggleDisableChilds(selector, false);
}



//将某元素所在的表格的行下面的所有输入控件提交
function submitRow(options) {

    options = $.extend({
        target: $(options.self).closest('tr')
    }, options || {});

    submitChilds(options);
}

//提交某元素下面所有的输入控件(input、select、textarea)
function submitChilds(options) {

    options = $.extend({
        method: "post"
    }, options || {});

    var confirmText = options.confirmText;
    if (confirmText) {
        if (!confirm(confirmText))
            return false;
    }

    var self = $(options.seft);
    var target = $(options.target);

    var prefix = options.prefix;

    var form = fetchTempForm();

    form.attr({ method: options.method, action: options.url });

    var clonedInputs = target.find("select,input,textarea");

    clonedInputs = jQuery.map(clonedInputs, function (old) {
        var clones = $(old).clone();

        if ($(old).attr("type") == "file") {
            alert("'submitButton' cannot submit 'file'");
        } else {
            clones.val($(old).val());

            if (prefix) {
                var name = clones.attr("name");
                if (name.indexOf(prefix) == 0) {
                    var newName = name.substring(prefix.length);

                    clones.attr("name", newName);
                }
            }
        }

        form.append(clones);

        return clones;
    });

    form.submit();
}

//系统全局ajax设置的函数
function setupAjax(options) {

    options = $.extend({
        showDelay: 300, //ajax消息提示延迟毫秒数
        hideDelay: 1000, //隐藏延迟
        loadingSelector: "#ajaxLoading"
    }, options || {});

    //设置jquery ajax 默认参数
    $.ajaxSetup({
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $(document).append("<input type='hide' value='" + XMLHttpRequest + "\n" + textStatus + "\n" + errorThrown + "'/>");
        },
        jsonp: options.jsonp
    });

    var ajaxMsgDelay = options.showDelay;

    var hideDelay = options.hideDelay;

    var loading = $(options.loadingSelector).css({
        position: "absolute",
        top: $(document).scrollTop(),
        right: 0
    });

    $(window).scroll(function () {
        loading.css({
            top: $(window).scrollTop(),
            right: 0
        });
    });

    //当前Timeout的id
    var currentTimeoutId = null;
    $.AjaxRequstCount = 0;
    loading.ajaxSend(function (evt, request, settings) {

        $.AjaxRequstCount++;

        var showLoading = function () {
            if ($.AjaxRequstCount > 0) {
                loading.stop(true, true);
                loading.show();
            }

            //执行完Timeout后将id清除
            currentTimeoutId = null;
        };

        //如果currentTimeoutId已经存在，清除这个timeout
        if (currentTimeoutId) {
            clearTimeout(currentTimeoutId);
        }

        currentTimeoutId = setTimeout(showLoading, ajaxMsgDelay);

    }).ajaxComplete(function (event, request, settings) {

        $.AjaxRequstCount--;
        if ($.AjaxRequstCount == 0) {

            loading.animate({
                opacity: 'hide'
            }, hideDelay, "swing", function () { loading.hide(); });
        }
    });
};


//$.fn.imgFace = function(options) {

//    alert('dsa');
//    var opts = $.extend({}, $.fn.subPage.defaults, options);

//    var $imgCtn = $(this);
//    var $img = $(this).find("img");
//    var $imgHover = $("<span></span>").addClass("imgHover").css({ top: opts.top, left: opts.left });
//    var $imgOut = $("<span></span>").addClass("imgOut").css({ top: opts.top, left: opts.left });
//    $imgCtn.append($imgHover, $imgOut).css({ position: "relative" });

//    $imgCtn.hover(function() {
//        $imgHover.show();
//        $imgOut.hide();
//    }, function() {
//        $imgHover.hide();
//        $imgOut.show();
//    });

//};
//$.fn.imgFace.defaults = {
//    top: 0,
//    left: 0
//};
//$.fn.imgFace.setDefaults = function(settings) {
//    $.extend($.fn.imgFace.defaults, settings);
//};