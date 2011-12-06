//滚动广告
$.fn.rolling = function(options) {

    var rolling = this;

    options = $.extend({
        delay: 3000,
        animateDuration: 1000,
        rollingCount: 1,
        rollingItem: ".rolling-item",
        rollingBox: ".rolling-box",
        direction: "left"
    }, options || {});

    var animateDuration = options.animateDuration;
    var delay = options.delay;
    var rollingCount = options.rollingCount;
    var displayCount = options.displayCount;
    var direction = options.direction;

    var rollingBox = rolling.find(options.rollingBox);
    var rollingItems = rolling.find(options.rollingItem);

    var getIndexItems = function(index) {

        var items = null;
        if (index == null) {
            items = rollingItems;
        } else {
            items = rollingItems.slice(0, index);
        }

        return items;
    };

    var getMaxAndSum = function(index) {
        var items = getIndexItems(index);

        var widthMax = 0;
        var widthSum = 0;

        var heightMax = 0;
        var heightSum = 0;

        items.each(function() {
            var $item = $(this);
            var width = $(this).innerWidth();
            var height = $(this).innerHeight();

            widthMax = Math.max(widthMax, width);
            widthSum += width;

            heightMax = Math.max(heightMax, height);
            heightSum += height;
        });

        return { width: { max: widthMax, sum: widthSum }, height: { max: heightMax, sum: heightSum} };
    };

    var maxAndSum = getMaxAndSum();

    var blockWidth = maxAndSum.width.sum;

    var blockHeight = maxAndSum.height.sum;

    var rollingInterval = null;
    var currentIndex = 0;

    switch (direction) {
        case "top":

            //如果滚动盒子的高度小于内容项最大高度，则设置和其一样
            //(这样是为了可以完全显示所有的内容项。)
            if (rolling.innerHeight() < maxAndSum.height.max) {
                rolling.height(maxAndSum.height.max);
            }

            //如果滚动盒子的高度大于内容总和的高度，则不滚动。
            if (rolling.innerHeight() > maxAndSum.height.sum) {
                return;
            }

            //克隆一份 实现循环滚动效果
            rollingBox.append(rollingItems.clone());

            rollingInterval = function() {
                if (currentIndex == rollingItems.size()) {
                    currentIndex = 0;
                    rolling.scrollTop(0);
                }

                var nextIndex = currentIndex + 1;

                var currMaxAndSum = getMaxAndSum(nextIndex * rollingCount);

                rolling.animate({
                    scrollTop: currMaxAndSum.height.sum
                }, animateDuration);

                currentIndex++;
            }
            break;
        case "right":
            // TODO:实现向右滚动
            break;
        case "bottom":
            // TODO:实现向下滚动
            break;
        case "left":

            //克隆一份 实现循环滚动效果
            rollingBox.append(rollingItems.clone());

            rollingInterval = function() {
                if (currentIndex == rollingItems.size()) {
                    currentIndex = 0;
                    rolling.scrollLeft(0);
                }

                var nextIndex = currentIndex + 1;

                var currMaxAndSum = getMaxAndSum(nextIndex * rollingCount);

                rolling.animate({
                    scrollLeft: currMaxAndSum.width.sum
                }, animateDuration);

                currentIndex++;
            }
            break;
    }

    // TODO:提供事件给外部订阅

    var intervalId = setInterval(rollingInterval, delay);

    rolling.mouseover(function() {
        clearInterval(intervalId);
    });

    rolling.mouseout(function() {
        intervalId = setInterval(rollingInterval, delay);
    });
};


$.fn.autoSearch = function(options) {

    options = $.extend({
        url: '<%=Url.Action("Keyword") %>',
        hidde: "#KeywordHidde",
        description: "#KeywordDescription"
    }, options || {});

    var self = $(this);

    self.autocomplete({
        minLength: 0,
        source: options.url,
        focus: function(event, ui) {
            self.val(ui.item.lable);
            return false;
        },
        select: function(event, ui) {
            self.val(ui.item.label);
            $(options.hidde).val(ui.item.Id);
            $(options.description).html("客户姓名：" + ui.item.Name + "&nbsp;&nbsp;" +
            //"学号：" + ui.item.studentNumber + "&nbsp;&nbsp;" +
                                               "电话号码：" + ui.item.PhoneNo);
            return false;
        }
    }).data("autocomplete")._renderItem = function(ul, item) {
        return $("<li></li>").data("item.autocomplete", item)
                             .append("<a>" + "客户姓名：" + item.Name + "&nbsp;&nbsp;" +
        //"学号：" + item.studentNumber + "&nbsp;&nbsp;" +
                                     "电话号码：" + item.PhoneNo + "</a>")
                             .appendTo(ul);
    };

    self.keyup(function() {
        var self = $(this);
        if (!self.val()) {
            $(options.hidde).val("");
        }
    })
};

