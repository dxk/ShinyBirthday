/*
define command
$(document).command({name:"delete", params:["url"]});

bingding command
$(document).command("delete").bind(function(params){
params.url;
do xxxxxx
});

execute command
$("#link").command("delete").execute({url:url}); */
$.fn.command = function(options) {

    options = options || {};

    var keyPrefix = "command:";

    var document$ = $(document);

    //获取命令
    if (typeof (options) == "string") {
        var name = options;

        var command = document$.data(keyPrefix + name);

        return command;
    }

    options = $.extend({
        context: document$,
        bind: function(handle) {

            var command = this.context.data(keyPrefix + this.name);

            command = $.extend({
                _handle: handle,
                execute: function(params) {
                    this._handle(params);
                }
            }, command);

            this.context.data(keyPrefix + this.name, command);

            return this;
        }
    }, options);

    document$.data(keyPrefix + options.name, options);

    return this;
}
