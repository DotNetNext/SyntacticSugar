//基于validate验证
//作者:sunkaixuan
//时间:2015-6-5
var validateFactory = function (form) {
    this.init = function () {
        addMethod();
        bind();
    }
    this.submit = function () {
        if (form.data("validateFactory")) {
            form.data("validateFactory").form();
        }
    }
    this.ajaxSubmit = function (rollback) {
        if (form.data("validateFactory")) {
            if (form.valid()) {
                if (rollback != null) {
                    rollback();
                }
            }
        }
    }
    function addMethod() {
        form.find("[pattern]").each(function () {
            var th = $(this);
            var pattern = GetPattern(th);
            var methodName = GetMdthodName(th);
            $.validator.addMethod(methodName, function (value, element, params) {
                return this.optional(element) || new RegExp(pattern).test(value);
            }, GetTip(th));
        });
    }
    function bind() {
        var rules = {};
        var messages = {};
        form.find("[pattern]").each(function () {
            var th = $(this);
            var methodName = GetMdthodName(th);
            var name = GetName(th);
            rules[name] = {};
            rules[name][methodName] = true;
            if (GetIsRequired(th)) {
                rules[name].required = true;

                messages[name] = {};
                messages[name].required = "不能为空！";
            }


        });
        var v = form.validate({
            onfocusout: function (element) {
                $(element).valid();
            },
            onkeyup: function (element) {
                $(element).valid();
            },
            rules: rules,
            messages: messages,
            debug: false,
            errorPlacement: function (error, element) {
                if (element.is(":radio,:checkbox")) {
                    element.parent().append(error);
                } else {
                    element.after(error);
                }
            }
        });
        form.data("validateFactory", v);
    }


    function GetMdthodName(ele) {
        return ele.attr("name") + "ValidateMethod";
    }
    function GetName(ele) {
        return ele.attr("name");
    }
    function GetPattern(ele) {
        return ele.attr("pattern");
    }
    function GetTip(ele) {
        return ele.next().text();
    }
    function GetIsRequired(ele) {
        if (ele.attr("required")) {
            return true;
        } else {
            return false;
        }
    }
};

