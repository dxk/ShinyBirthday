/*
* you-may jQuery multiupload Plugin
* version: 1.0 (08-APR-2009)
* @requires jQuery v1.3.2 or later
* 
*/
; (function ($) {

    $.fn.multiupload = function (options) {

        var multiupload = this;

        options = $.extend({
            fileSelector: ".file-upload",
            deleteSelector: ".delete-file"
        }, options || {});

        var fileNameDataKey = "targetFileName";

        var addOperate = function (target) {
            var warp = $("<div></div>");
            multiupload.append(warp);

            warp.append(target);
            target = $(target);

            var deleteButton = $("<a></a>");
            warp.prev().append(deleteButton);
            deleteButton.text("剔除");
            deleteButton.attr("href", "javascript:void(0);");
            deleteButton.data(fileNameDataKey, target.attr("name"));
            deleteButton.addClass(options.deleteSelector.substring(1));
        };

        var fileCount = function () {
            return multiupload.find(options.fileSelector).size();
        };

        var emptyFileCount = function () {
            return multiupload.find(options.fileSelector).filter(function () {
                return !$(this).val();
            }).size();
        };

        var templete = multiupload.find(options.fileSelector + ":first");
        // addOperate(templete);
        var div = $("<div></div>");
        div.append(templete);
        multiupload.append(div);
        //alert(multiupload.html());

        var baseName = templete.attr("name");

        var count = 1;

        this.find(options.fileSelector).change(function () {
            var self = $(this);

            if (self.val() && emptyFileCount() === 0) {
                var newFile = self.clone(true);

                self.parent().append(newFile);
                newFile.val(null);
                newFile.attr("name", baseName + ++count);

                addOperate(newFile);
            }

        });

        this.find(options.deleteSelector).live("click", function () {
            var self = $(this);
            self.parent().remove();
            //            var fileName = self.data(fileNameDataKey);
            //            var file = multiupload.find("input[name=" + fileName + "]");

            //            if (fileCount() === 1) {
            //                file.val(null);
            //            } else {
            //                file.parent().remove();
            //            }

        });

    };



})(jQuery);

$(function () {
    $.fn.uploadList = function (options) { //navAnimate
        var opts = $.extend({}, $.fn.uploadList.defaults, options);

        var $button = $(this);
        var $files = $(opts.list);
        var btnName = $button.find("input").attr("name");



        $button.find("input").live("blur", function () {
            var fileName = $(this).val();
			
			if($button.find("input").index($(this)) != $button.find("input").size() -1){return;}
			
			if(fileName == "" || fileName == null){return;}

            if (fileName.lastIndexOf("\\") > 0) {
                fileName = fileName.substring(fileName.lastIndexOf("\\") + 1);
            }

            if (fileName.lastIndexOf("/") > 0) {
                fileName = fileName.substring(fileName.lastIndexOf("/") + 1);
            }

            var $newBtn = '<input type="file" name=""/>';
            var $li = '<li><span>' + fileName + '</span><div></div></li>';

            $button.append($newBtn);
            $files.append($li);

            $button.find("input").each(function (i) {
                $(this).attr("name", btnName + (i + 1));
            });

            $button.find("input").hide().last().show();
        });

        $("table").mousemove(function () {
            $button.find("input").blur();
        });

        $files.find("div").live("click", function () {
            var index = $files.find("li").index($(this).closest("li"));

            $button.find("input").eq(index).remove();

            $(this).closest("li").remove();

            $button.find("input").each(function (i) {
                $(this).attr("name", "file" + (i + 1));
            });

        });

    };
    $.fn.uploadList.defaults = {
        list: ""
    };
    $.fn.uploadList.setDefaults = function (settings) {
        $.extend($.fn.uploadList.defaults, settings);
    };
});