
$(function () {
    $.fn.tab = function (options) { //tab
        var opts = $.extend({}, $.fn.tab.defaults, options);

        var $this = $(this);
        $this.hide();
        var $tabs = $this.children("ul").addClass("tabUl").find("li").addClass(opts.outClass);
        var $sheets = $this.children("dl").css({ clear: "both", backgroundColor: "#fcfcfc" });
        var $tabRight = "<li class='" + opts.tabRight + "'></li>";
        var $tabLast = "<li class='" + opts.tabLast + "'></li>";

        $tabs.each(function (i) {
            $(this).data("index", i);

            if (i + 1 == $tabs.size()) {
                $(this).after($tabLast);
                $this.children("ul").append("<li class='" + opts.hoverClass + "'></li>");
            } else {
                $(this).after($tabRight);
            }

            $this.show();

        }).click(function () {
            var i = index($(this));
            $sheets.hide().eq(i).show();
            $this.find("." + opts.hoverClass).css({ left: $(this).next().position().left });

            $tabs.each(function (j) {
                if (j == i) {
                    $(this).removeClass().addClass(opts.tabOutHover);
                } else {
                    $(this).removeClass().addClass(opts.outClass);
                }
            });

        }).eq(0).click();

        function index(item) {
            return $(item).data("index");
        }

    };
    $.fn.tab.defaults = {
        outClass: "tabOut",
        hoverClass: "tabHover",
        lastClass: "tabLast",
        tabRight: "tabRight",
        tabLast: "tabLast",
        tabOutHover: "tabOutHover"
    };
    $.fn.tab.setDefaults = function (settings) {
        $.extend($.fn.tab.defaults, settings);
    };
});
