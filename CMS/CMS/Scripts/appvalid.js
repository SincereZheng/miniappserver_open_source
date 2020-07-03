var $_css = "";

var hashTable = function () {
    this.items = new Array();
    this.itemsCount = 0;
    this.add = function (key, value) {
        if (!this.containsKey(key)) {
            this.items[key] = value;
            this.itemsCount++;
        }
        else {
            this.items[key] = value;
        }
    }
    this.get = function (key) {
        if (this.containsKey(key))
            return this.items[key];
        else
            return null;
    }

    this.remove = function (key) {
        if (this.containsKey(key)) {
            delete this.items[key];
            this.itemsCount--;
        }
        else
            throw "key '" + key + "' does not exists."

    }

    this.containsKey = function (key) {
        return typeof (this.items[key]) != "undefined";
    }

    this.containsValue = function containsValue(value) {
        for (var item in this.items) {
            if (this.items[item] == value)
                return true;
        }

        return false;
    }

    this.contains = function (keyOrValue) {
        return this.containsKey(keyOrValue) || this.containsValue(keyOrValue);
    }

    this.clear = function () {
        this.items = new Array();
        itemsCount = 0;
    }

    this.size = function () {
        return this.itemsCount;
    }

    this.isEmpty = function () {
        return this.size() == 0;
    }
};

function valid_required(id, error) {
    var content = $getValue(id); 
    if ($.trim(content) == '' || $.trim(content) == '无')
    {
        return error;
    }
    return "";
}

function valid_required_test(id, reg, error1,error2) {
    var content = $getValue(id);
    if ($.trim(content) == '' || $.trim(content) == '无' || $.trim(content) == '空') {
        return error1;
    }
    else {
        if (!reg.test(content))
        {
            return error2;
        }
    }
    return "";
}

function valid_test(id, reg, error) {
    var content = $getValue(id);
    if ($.trim(content) != '' && $.trim(content) != '无' || $.trim(content) == '空') {
        if (!reg.test(content)) {
            return error;
        }
    }
    return "";
}

function valid_format(id, reg, error1,error2) {
    var content = $getValue(id);
    if ($.trim(content) == '' || $.trim(content) == '无' || $.trim(content) == '空') {
        return error1;
    }
    else {
        if (content.indexOf(" / ")<0)
        {
            return error2;
        }
    }
    return "";
}

function valid_suggest(id, reg, error1, error2) {
    var content = $getValue(id);
    var selcontent = $getSelValue(id)
    if ($.trim(content) == '' || $.trim(content) == '无' || $.trim(content) == '空') {
        return error1;
    }
    else {
        if (content != selcontent) {
            return error2;
        }
    }
    return "";
}

function valid_emptysuggest(id, reg, error1, error2) {
    var content = $getValue(id);
    var selcontent = $getSelValue(id)
    if ($.trim(content) == '' || $.trim(content) == '无' || $.trim(content) == '空') {
        return "";
    }
    else {
        if (content != selcontent) {
            return error2;
        }
    }
    return "";
}

var $$$valids = new hashTable();
function $requiredValid(group, id, error) {
    $valid.call(this, group, id, error, valid_required);
}

function $testValid(group, id, reg, error) {
    $valid.call(this, group, id, reg, error, valid_test);
}

function $requiredTestValid(group, id, reg, error1, error2) {
    $valid.call(this, group, id, reg, error1, error2, valid_required_test);
}

function $requiredFuncValid(group, id, func, error1, error2) {
  
    $valid.call(this, group, id, "", error1, error2, func);
}


function $valid() {
    var len = arguments.length;
    var item = {};
    //group, id, reg, error1, error2, fuc
    if (len == 4)
    {
        item.group = arguments[0];
        item.id = arguments[1];
        item.reg = "";
        item.error1 = arguments[2];
        item.error2 = "";
        item.fuc = arguments[3];
        item.len = len;
    }
    //group, id, reg, error1, fuc
    else if (len == 5)
    {
        item.group = arguments[0];
        item.id = arguments[1];
        item.reg = arguments[2];
        item.error1 = arguments[3];
        item.error2 = "";
        item.fuc = arguments[4];
        item.len = len;
    }
    //group, id, reg, error1, error2, fuc
    else if (len == 6) {
        item.group = arguments[0];
        item.id = arguments[1];
        item.reg = arguments[2];
        item.error1 = arguments[3];
        item.error2 = arguments[4];
        item.fuc = arguments[5];
        item.len = len;
    }

    var _ele = $("#" + item.id);

    var _b = true;
    if ($_css != "")
    {
        _b = _ele.hasClass($_css);
    }

    if (_b)
    {
        var t = _ele.get(0).tagName
        if (t == "INPUT" || t == "TEXTAREA") {
            _ele.focus(function () {
                showInputError(item.id, false);
            }).blur(function () {
                var verror = "";
                if (len == 4) {
                    verror = item.fuc(item.id, item.error1);
                }
                else if (len == 5) {
                    verror = item.fuc(item.id, item.reg, item.error1);
                }
                else if (len == 6) {
                    verror = item.fuc(item.id, item.reg, item.error1, item.error2);
                }
                if (verror == "") {
                    showInputError(item.id, false);
                }
                else {
                    showInputError(item.id, true, verror);
                }
            });
        }

        var groups = item.group.split("|");
        for (var i = 0; i < groups.length; i++) {
            var array = $$$valids.get(groups[i]);

            if (array == null) {
                $$$valids.add(groups[i], [item]);
            }
            else {
                array.push(item);
            }
        }
    }
    
}

function $getValue(id)
{
    var t = $("#" + id).get(0).tagName;
    if (t == "INPUT" || t == "TEXTAREA") {
        return $("#" + id).val();
    }
    else {
        return $("#" + id).text();
    }
}

function $getSelValue(id) {
    var temp = $("#" + id).attr("selv");
    if (temp)
    {
        temp = temp.replace(/\n/g, "");
    }
    return temp;
}

function groupValid(group)
{
    var error="";
    var array = $$$valids.get(group);
    if (array != null)
    {
        for (var i = 0; i < array.length; i++)
        {
            var obj = array[i];
            var verror = "";
            if (obj.len == 4) {
                verror = obj.fuc(obj.id, obj.error1);
            }
            else if (obj.len == 5) {
                verror = obj.fuc(obj.id, obj.reg, obj.error1);
            }
            else if (obj.len == 6) {
                verror = obj.fuc(obj.id, obj.reg, obj.error1, obj.error2);
            }
            if (verror != "") {
                showInputError(obj.id, true, obj.error);
            }
            else {
                showInputError(obj.id, false)
            }
            if (error == "" && verror != "")
            {
                error = verror;
            }
        }
    }
    return error;
}

function showInputError(id, show, message) {
    if (show) {
        $("#" + id).css("background", "rgba(255, 0, 0, 0.1)").attr("title", message);
    } else {
        $("#" + id).css("background", "rgba(255, 255, 255, 0.1)");
        //$("#" + id).removeAttr("style");
    }
}

var $reg = {};
//整数或者小数
$reg.number0 = /^[0-9]+\.{0,1}[0-9]{0,2}$/
//验证Email地址
$reg.email = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/
//验证身份证号（15位或18位数字）
$reg.peopleid = /^\d{15}|\d{18}$/
//验证用户密码正确格式为：以字母开头，长度在6~18之间，只能包含字符、数字和下划线
$reg.pwd = /^[a-zA-Z]\w{5,17}$/
//只能输入汉字
$reg.zh = /^[\u4e00-\u9fa5]{0,}$/
//26个英文字母组成的字符串
$reg.en = /^[A-Za-z]+$/
//数字
$reg.number = /^[0-9]*$/
//电话号码(手机)
$reg.phonenum = /^1(3|4|5|7|8)\d{9}$/;
//电话号码(座机)
$reg.telnum = /^([\d-+]*)$/;
//邮箱
$reg.email = /^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/;



$reg.int0 = /^\d+$/　　//非负整数（正整数 + 0） 
$reg.int = /^[0-9]*[1-9][0-9]*$/　　//正整数 
$reg.minusint0 = /^((-\d+)|(0+))$/　　//非正整数（负整数 + 0） 
$reg.minusint = /^-[0-9]*[1-9][0-9]*$/　　//负整数 
$reg.float0 = /^\d+(\.\d+)?$/　　//非负浮点数（正浮点数 + 0） 
$reg.plusfloat = /^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$/　　//正浮点数 
$reg.minusfloat0 = /^((-\d+(\.\d+)?)|(0+(\.0+)?))$/　　//非正浮点数（负浮点数 + 0） 
$reg.minusfloat = /^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$/　　//负浮点数 
$reg.float = /^(-?\d+)(\.\d+)?$/　　//浮点数 