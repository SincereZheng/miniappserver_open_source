$.extend($.validator.messages, {
    required: "必选字段",
    remote: "请修正该字段",
    email: "请输入正确格式的电子邮件",
    url: "请输入合法的网址",
    date: "请输入合法的日期",
    dateISO: "请输入合法的日期 (ISO).",
    number: "请输入合法的数字",
    digits: "只能输入整数",
    creditcard: "请输入合法的信用卡号",
    equalTo: "请再次输入相同的值",
    accept: "请输入拥有合法后缀名的字符串",
    maxlength: $.validator.format("请输入一个长度最多是 {0} 的字符串"),
    minlength: $.validator.format("请输入一个长度最少是 {0} 的字符串"),
    rangelength: $.validator.format("请输入一个长度介于 {0} 和 {1} 之间的字符串"),
    range: $.validator.format("请输入一个介于 {0} 和 {1} 之间的值"),
    max: $.validator.format("请输入一个最大为 {0} 的值"),
    min: $.validator.format("请输入一个最小为 {0} 的值")
});


// 手机号码验证
jQuery.validator.addMethod("isMobile", function (value, element) {
    var length = value.length;
    var mobile = /^(13[0-9]{9})|(18[0-9]{9})|(14[0-9]{9})|(17[0-9]{9})|(15[0-9]{9})$/;
    return this.optional(element) || (length == 11 && mobile.test(value));
}, "请正确填写您的手机号码");


function showSuggestError(id,show, message)
{
    if (show) {
        $("#" + id).css("background", "rgba(255, 0, 0, 0.1)").attr("title", message);
    } else {
        $("#" + id).css("background", "rgba(255, 255, 255, 1)").removeAttr("title", message);
    }
}

function $getFormField() {
    var cla = "";
    var len = arguments.length;
    if (len > 0) {
        cla = arguments[0];
        cla = cla.replace(/\s+/g, "");
    }
    else {
        cla = "field";
    }
    var fields = [];

    $("." + cla).each(function (index, obj) {

        var t = obj.tagName;
        if (t == "INPUT" || t == "SELECT") {
            fields.push(obj.id);
        }
        //else if (t == "BUTTON") {
        //}
        else {

            fields.push(obj.id);
        }

    });

    return fields;
}

function $getFormDataString()
{
    var datas= $getFormData.apply(this, arguments);
    var data = JSON.stringify(datas);
    return data;
}

function $getFormData()
{
    var cla = "";
    var len = arguments.length;
    if (len > 0) {
        cla = arguments[0];
        cla = cla.replace(/\s+/g, "");
    }
    else {
        cla = "field";
    }
    var datas = {};

    $("."+cla).each(function (index, obj) {

        $setJson(datas, obj);

    });
    return datas;
}


function $getFieldsData(fields)
{
    var datas = {};
    for (var i = 0; i < fields.length; i++)
    {
        var obj = document.getElementById(fields[i]);
        if (obj != undefined)
        {
            if (obj != null)
            {
                $setJson(datas, obj);
            }
        }
        
    }
    return datas;
}

function $setJson(json, obj)
{
    var val = $getValue2(obj);
    if (val != null)
    {
        json[obj.id] =escape(val);
    }
    
}

function $setValue(id,val) {
    var t = $("#" + id).get(0).tagName;
    if (t == "INPUT") {
        return $("#" + id).val(val);
    }
    else if (t == "TEXTAREA") {
        return $("#" + id).val(val);
    }
    else {
        return $("#" + id).text(val);
    }

}

function $getValue(id) {
    var t = $("#" + id).get(0).tagName;
    if (t == "INPUT") {
        return $("#" + id).val();
    }
    else if (t == "TEXTAREA") {
        return $("#" + id).val();
    }
    else {
        return $("#" + id).text();
    }

}

function $getValue2(obj) {
    var t = obj.tagName;
    if (t == "INPUT") {
        if (obj.type == "checkbox") {

            if ($(obj).is(':checked')) {
                return "1";
            }
            else {
                return "0";
            }
        }
        else if (obj.type == "radio") {

            if ($(obj).is(':checked')) {
                return "1";
            }
            else {
                return "0";
            }
        }
        else {
            return $(obj).val();
        }
    }
    else if (t == "SELECT") {
        return $(obj).find('option:selected').val();
    }
    else if (t == "TEXTAREA") {
        return $(obj).val();
    }
    else if (t == "BUTTON") {
        return null;
    }
    else {

        return $(obj).text();
    }

}


function $getFloat() {
    var v = 0;

    var len = arguments.length;
    var bg = true;
    if (len == 3)
    {
        var id = arguments[0];
        var nid = arguments[1];
        var nval = arguments[2];
        if (nid && nval)
        {
            if (nid == id)
            {
                v =Number( nval);
                bg = false;
            }
        }
    }
    if (bg)
    {
        var id = arguments[0];
        var gv = $getValue(id);
        if (isNumber(gv)) {
            v = Number(gv);
        }
    }
    return toDecimal(v);
}


function isNumber(val)
{
    var b = false;
    if (isNaN(val)) {
        b = false;
    }
    else {
        b = true;
    }

    return b;
}

function showTip(msg, type) {
    toastr.options = {
        closeButton: true,
        debug: true,
        progressBar: true,
        preventDuplicates: true,
        positionClass: 'toast-top-center',
        onclick: null,
        timeOut: 1000
    };

    if (type == "success") {
        toastr.success(msg, '信息提醒')
    }
    else if (type == "error") {
        toastr.error(msg, '信息提醒')
    }
}

function showAlert(msg, evt) {
    $("#alertmsg").html(msg);
    $("#alert").modal('show');

    $("#alertokbtn").unbind("click").click(evt);
}


function showLoading(show, msg, type) {
    if (show) {
        $("#" + type + "_icon").addClass("fa-clock-o").removeClass("fa-question-circle");
        $("#" + type + "_title").html(msg);
        $("#" + type + "_loading").append('<div id="' + type + '_loading_ele" class="sk-spinner sk-spinner-three-bounce"><div class="sk-bounce1"></div><div class="sk-bounce2"></div><div class="sk-bounce3"></div></div>');
    }
    else {
        $("#" + type + "_icon").addClass("fa-exclamation").removeClass("fa-clock-o");
        $("#" + type + "_title").html(msg);
        $("#" + type + "_loading_ele").remove();
    }
}


function  pickUpFields(str)
{
    var val = [];
    pattern = new RegExp("\\{{(.| )+?\\}}", "igm");
    var v = str.match(pattern);
    if (v)
    {
        for (var i = 0; i < v.length; i++)
        {
            val.push(v[i].replace("{{", "").replace("}}", ""));
        }
    }



    return val;
}

function urlFixFields(url)
{
    var fields = pickUpFields(url);

    if (fields.length > 0)
    {
        for (var i = 0; i < fields.length; i++)
        {
            var field = fields[i];
     
            var verfield = field;
            if (field.indexOf("_val") > -1) {
                field = field.replace("_val", "");
                var val = eval(field);
                url = url.replace("{{" + field + "_val}}", field + "=" + escape(val));
            }
            else if (field.indexOf("_ick") > -1) {
                field = field.replace("_ick", "");
                var val = $('#' + field).is(':checked');
                url = url.replace("{{" + field + "_ick}}", field + "=" + escape(val));
            }
            else {
                url = url.replace("{{" + field + "}}", field + "=" + escape($("#" + field).val()));
            }
            
        }
    }

    var temps = url.split("key=");
    if (temps.length == 2)
    {
        url = temps[0] + "key=" + escape(temps[1]);
    }

    return url;
}

var pageSize = 10;
var pageIndex = 1;
function paging(total, index) {
    //alert(index);
    pageIndex = index;
    buildPager(total, pageSize, index)
    if (pagedList) {
        pagedList();
    }
}

function pagingPre(total) {
    pageIndex = pageIndex - 1;
    buildPager(total, pageSize, pageIndex);

    if (pagedList) {
        pagedList();
    }
}

function pagingNext(total) {
    pageIndex = pageIndex + 1;
    buildPager(total, pageSize, pageIndex);

    if (pagedList) {
        pagedList();
    }
}

function buildPager(totalCount, pageSize, pageIndex) {
    pageIndex = pageIndex - 1;
    var pagerHtml = "";
    var y = pageIndex % 10;

    var sindex = pageIndex - y;
    for (var i = 1; i <= 10; i++) {
        var cindex = sindex + i;
        if (cindex <= totalCount) {
            if (cindex == pageIndex + 1) {
                pagerHtml = pagerHtml + "<li class=\"active\"><a href=\"#\" >" + cindex + "</a></li>";
            }
            else {
                pagerHtml = pagerHtml + "<li><a href=\"#\" onclick=\"paging(" + totalCount + "," + cindex + ")\">" + cindex + "</a></li>";
            }
        }
    }
    if (pageIndex + 1 == 1) {
        if (pageIndex + 1 == totalCount) {
            pagerHtml = "<li class=\"disabled\" ><a href=\"#\">&larr; 上页</a></li>" + pagerHtml + "<li class=\"disabled\" ><a href=\"#\">下页 &rarr;</a></li>";
        }
        else {
            pagerHtml = "<li class=\"disabled\" ><a href=\"#\">&larr; 上页</a></li>" + pagerHtml + "<li class=\"next\" onclick=\"pagingNext(" + totalCount + ")\"><a href=\"#\">下页 &rarr;</a></li>";
        }
    }
    else if (pageIndex + 1 == totalCount) {
        pagerHtml = "<li class=\"previous\"  onclick=\"pagingPre(" + totalCount + ")\"><a href=\"#\">&larr; 上页</a></li>" + pagerHtml + "<li class=\"disabled\"><a href=\"#\">下页 &rarr;</a></li>";
    }
    else {
        pagerHtml = "<li class=\"previous\"  onclick=\"pagingPre(" + totalCount + ")\"><a href=\"#\">&larr; 上页</a></li>" + pagerHtml + "<li class=\"next\"  onclick=\"pagingNext(" + totalCount + ")\"><a href=\"#\">下页 &rarr;</a></li>";
    }
    $(".pagination").html(pagerHtml);
}

function toDecimal(x) {
    var f = parseFloat(x);
    if (isNaN(f)) {
        return;
    }
    f = Math.round(x * 100) / 100;
    return f;
}

function formatCurrency(num) {
    num = num.toString().replace(/\$|\,/g, '');
    if (isNaN(num))
        num = "0";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    cents = num % 100;
    num = Math.floor(num / 100).toString();
    if (cents < 10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++)
        num = num.substring(0, num.length - (4 * i + 3)) + ',' +
        num.substring(num.length - (4 * i + 3));
    return (((sign) ? '' : '-') + num + '.' + cents);
}