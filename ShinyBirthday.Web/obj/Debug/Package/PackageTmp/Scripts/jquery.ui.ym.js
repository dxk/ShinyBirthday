(function ($) {
    $.widget("ym.JCheckBox", {
        options: {
            title: "", //标题
            action: "",
            touch: "", //触发窗体的按钮 样式名: .class  ID: #id 
            ul: "", 	//列表 样式名: .class  ID: #id 
            windowClass: "", 	//控制弹窗样式 class名
            ulClass: "",  //显示列表样式 class名
            css: "",
            data: "", //参数 {id:12,name:"exmple"}
            selectAll: false, //是否全部选中
            identity: ""
        },

        _create: function () {

        },

        _init: function () {
            var self = this;
            var passShift = false;
            var identityCard = self._identityCard();
            self.options.identity = identityCard;
            $(self.element).hide();

            var $ul = self._loadUl();
            var $window = self._loadWindow($ul);
            var $touch = $(this.options.touch);

            $window.data("ui-JCheckBox-index", identityCard);
            $ul.data("ui-JCheckBox-index", identityCard);
            $touch.data("ui-JCheckBox-index", identityCard);
            $window.find("*").data("ui-JCheckBox-index", identityCard);

            self._checkBoxToggle($ul, $window);
            self._windowToggle($touch, $window);
            self._deleteItem($ul, $window);

            if (self.options.selectAll) {
                self._selectAll($ul, $window, $(self.element));
            }

            self._passShift($window, $ul);
        },

        _loadWindow: function (u) {
            var self = this;
            var $window = $("<div><div class='ui-corner-all ui-widget-header ui-JCheckBox-header'>" + this.options.title + "</div><ul style='margin:5px; overflow:hidden;'></ul></div>").addClass("ui-JCheckBox-window ui-widget-content ui-corner-all");
            $window.appendTo($("body")).css({ display: "none", position: "absolute", minWidth: 200 }).addClass(self.options.windowClass).css(self.options.css);
            $window.css(self.options.css);

            if (this.options.action == "") {
                var opts = $(this.element).children();
                u.empty();

                opts.each(function (i) {
                    var val = $(this).val();
                    var text = $(this).text();
                    var identityCard = self._identityCard();

                    $("<li><input name='ui-JCheckBox-checkbox' type='checkbox' id='ui-JCheckBox-checkbox" +
						identityCard + "' value='" +
						val + "'  /><label for='ui-JCheckBox-checkbox" + identityCard + "'>" +
						text + "</label></li>").appendTo($window.find("ul"));

                    if ($(this).prop("selected")) {
                        var li = $("<li>" + text + "<div> </div></li>");
                        u.append(li.data("index", val));
                    }
                });

                $(this.element).children().each(function () {
                    var val = $(this).val();

                    if ($(this).prop("selected")) {
                        $window.find("input[value=" + val + "]").prop("checked", true);
                    }
                });
            } else {
                $.ajax({
                    url: self.options.action,
                    data: self.options.data,
                    type: 'get',
                    dataType: 'json',
                    async: false,
                    success: function (d) {
                        $.each(d, function (index, view) {
                            var identityCard = self._identityCard();

                            $("<li><input name='ui-JCheckBox-checkbox' type=checkbox id='ui-JCheckBox-checkbox" +
								identityCard + "' value='" +
								view["Id"] + "' /><label for='ui-JCheckBox-checkbox" + identityCard + "'>" +
								view["Name"] + "</label></li>").appendTo($window.find("ul"));

                            $("<option value='" + view["Id"] + "'>" + view["Name"] + "</option>").appendTo($(self.element));
                        });
                    }
                });
            }

            return $window;
        },

        _loadUl: function () {
            var ul;
            if (this.options.ul == "") {
                ul = $("<ul class='ui-JCheckBox-ul'></ul>")
                $(this.element).after(ul);

            } else {
                ul = $(this.options.ul);
            }

            return ul.addClass(this.options.ulClass);
        },

        _identityCard: function () {
            var $windows = $(".ui-JCheckBox-window");
            var randomNum = Math.random() * 10000000000000000;

            while (true) {
                var b = false;
                $windows.each(function () {
                    var currentNum = $(this).data("ui-JCheckBox-index");

                    if (currentNum == randomNum) {
                        b = true;
                    }
                });

                if (b) {
                    randomNum = Math.random() * 10000000000000000;
                } else {
                    break;
                }
            }

            return randomNum;
        },

        _windowToggle: function (t, w) {
            var self = this;
            t.click(function () {
                var pos = self._pos(t, w);
                // move input on screen for focus, but hidden behind dialog
                w.css('left', (pos[0] + 20) + 'px').css('top', pos[1] + 'px');

                if (w.css("display") == "none") {
                    w.animate({ opacity: 'show' }, 200);
                } else {
                    w.animate({ opacity: 'hide' }, 200);
                }
            });

            document.onclick = function (e) {
                e = e || window.event;
                var target = e.srcElement || e.target;

                var index = $(target).data("ui-JCheckBox-index");

                if (index) {
                    $(".ui-JCheckBox-window").each(function () {
                        if ($(this).data("ui-JCheckBox-index") != index) {
                            $(this).animate({ opacity: 'hide' }, 200);
                        }
                    });

                } else {
                    $(".ui-JCheckBox-window").animate({ opacity: 'hide' }, 200);
                }
            }
        },

        _checkBoxToggle: function (u, w) {
            var self = this;

            w.find("input").change(function () {
                self._selectFromWindow(w, $(self.element), u);
            });
        },

        _deleteItem: function (u, w) {
            var self = this;
            u.find("div").die().live("click", function () {
                var li = $(this).closest("li");
                var val = li.data("index");

                $(self.element).find("option[value=" + val + "]").prop("selected", false);
                li.remove();

                self._selectFromElement(w, $(self.element), u);
            });
        },

        _selectAll: function (u, w, s) {
            w.find("input").prop("checked", true);

            this._selectFromWindow(w, s, u);
        },

        _selectFromWindow: function (w, s, u) {
            var si = s.children();
            si.prop("selected", false);
            u.empty();

            w.find("input").each(function () {
                if ($(this).prop("checked")) {
                    var val = $(this).val();
                    var text = $(this).next().text();

                    s.find("option[value=" + val + "]").prop("selected", true);

                    var li = $("<li>" + text + "<div> </div></li>");
                    u.append(li.data("index", val));
                }
            });
        },

        _selectFromElement: function (w, s, u) {
            var si = s.children();
            w.find("input").prop("checked", false);
            u.empty();

            si.each(function () {
                if ($(this).prop("selected")) {
                    var val = $(this).val();
                    var text = $(this).text();

                    w.find("input[value=" + val + "]").prop("checked", true);

                    var li = $("<li>" + text + "<div> </div></li>");
                    u.append(li.data("index", val));
                }
            });
        },

        _passShift: function (w, u) {
            var self = this;
            var passShift = false;

            w.find("li").mousemove(function () {
                var c = $(this).find("input");

                if (passShift && c.prop("checked") == false) {
                    c.prop("checked", true);
                    self._selectFromWindow(w, $(self.element), u);
                }
            });

            $(document).keydown(function (event) {
                if (event.keyCode == 16) {
                    passShift = true;
                }
            }).keyup(function () {
                passShift = false;
            });
        },

        _pos: function (t, w) {
            var win = [$(window).width(), $(window).height()];
            var tCss = [t.width(), t.height()];
            var wCss = [w.width(), w.height()];
            var tPos = [t.offset().left, t.offset().top];
            var pos = [tPos[0] + tCss[0], tPos[1]];

            if (wCss[0] + tPos[0] > win[0]) {
                pos[0] = win[0] - tCss[0];
            }

            return pos;
        },

        destroy: function () {
            var self = this;

            $(".ui-JCheckBox-window").each(function () {
                if ($(this).data("ui-JCheckBox-index") == self.options.identity) {
                    $(this).remove();
                }
            });

            if (self.options.ul == "") {
                $(".ui-JCheckBox-ul").each(function () {
                    if ($(this).data("ui-JCheckBox-index") == self.options.identity) {
                        $(this).remove();
                    }
                });
            } else {
                $(self.options.ul).empty();
            }

            self.options = {
                title: "",
                action: "",
                touch: "",
                ul: "",
                windowClass: "",
                ulClass: "",
                css: "",
                data: "",
                selectAll: false,
                identity: ""
            }
        }

    })

    $.widget("ym.defaultWord", {
        options: {
            word: "", //默认值
            color: "", //变量
            defaultColor: "#bbb" //默认值颜色
        },

        _create: function () {
            var self = this;
            var text = self.element.val(); 

            if (jQuery.trim(text) == "") {
                self.options.color = self.element.css("color");
                self.element.val(self.options.word).css({ color: self.options.defaultColor });
            }
        },

        _init: function () {
            var self = this;

            self.element.focus(function () {
                var text = $(this).val();

                if (text == self.options.word) {
                    $(this).val("").css({ color: self.options.color });
                }

            }).blur(function () {
                var text = $(this).val();

                if (jQuery.trim(text) == "") {
                    self.element.val(self.options.word).css({ color: self.options.defaultColor });
                }
            });

            self.element.closest("form").submit(function () {
                if (self.element.val() == self.options.word) {
                    self.element.val("");
                }
            });

        },

        destroy: function () {
            var self = this;
        }

    })
})(jQuery)