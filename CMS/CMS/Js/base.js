$(function(){
    var oldError = window.onerror;
    window.onerror = function myErrorHandler(errorMsg, url, lineno, colno, error) {
        if (errorMsg.index && errorMsg.index("ignore") > -1 && errorMsg.index("ignore") == errorMsg.length - 6) {

        }else
            alert(errorMsg);
        return true
    }
    $.ajaxSetup({
        async: false,
        global: true,
        beforeSend: function (jqXHR, settings) {
            console.log(settings);
            var arr = settings.url.split('/');
            var arrDian = [];
            var arrurl = [];
            
            arr.forEach(function (item, index) {
                var olditem = item;
                item = item.replaceAll('.', "");
                if (String.IsNullOrEmpty(item))
                    arrDian.push(olditem);
                else
                    arrurl.push(item);
            });
            if (arrDian.length>0)
                settings.url = arrDian.join('/') + getvirtualname() + "/" + arrurl.join('/');
        }
    });
})

function Ajax(type, url, para, issync) {
    var async = typeof (issync) === "function" ? true : false;
    para["isInvoke"] = true;
    para["ajax"] = true;
    var formData = new FormData();
    for (var key in para) {
        formData.append(key, para[key]);
    }
    //para["para"] = JSON.stringify(para);
    var iserror = false;
    var r;
    var ajaxreq = $.ajax({
        url: url,
        async: async,
        type: type,
        data: formData,
        dataType: 'json',
        scriptCharset: 'utf-8',
        processData: false, // 使数据不做处理
        contentType: false,
        timeout: 5000,
        success: function (result, r2, r3) {
            if (result == "l0912") {
                alert("请登录");
                window.location.href = '../../Login/Index';
            }
            if (issync)
                issync(result);
            else {
                if (String.IsNullOrEmpty(result))
                    return result;
                else {
                    try {
                        r = JSON.parse(result);
                    } catch (e) {
                        r = result;
                    }
                    
                }
            }
        },
        fail: function (err) {
            console.log(err);
        },
        error: function (err) {
            //if (!err.responseText.startsWith('l0912'))
            //    alert(err.responseText);
            r = err.responseText
            //记录，当报错的时候不往下执行
            //if (event.stopPropagation) {
            //    event.stopPropagation();
            //} else if (window.event) {
            //    window.event.cancelBubble = true;
            //}
            iserror = true
        }
    })
    if (iserror) {
        var e = new Error(r);
        throw e;
    }
    return r;
}

function AjaxPost(url, para, issync, p) {
    if (!p) {
        var login = Ajax('POST', '/Base/CheckLogin', {}, false);
        if (!login) {
            alert("登录状态已过期");
            window.location.href = '../../Login/Index';
            throw "ignore";
        } else {
            var r = Ajax('POST', url, para, issync);
            return r;
        }
    } else {
        var r = Ajax('POST', url, para, issync);
        return r;
    }
}
//get方法禁用
function AjaxGet(url, para, issync) {
    alert('该方法被禁用了')
    throw '该方法被禁用了'
    return;
    var login = Ajax('POST', '../../../Base/CheckLogin', {}, false);
    if (!login) {
        alert("登录状态已过期");
        window.location.href = '../../Login/Index';
    }
    return Ajax('GET', url, para, issync);
}

function GetFormData() {
    var result = {};
    var nodelist = document.querySelectorAll(".field");
    nodelist.forEach(function (item) {
        var obj = {};
        if (item.type == "text" || item.type == "textarea" || item.type == "password") {
            obj[item.id] = item.value;
            result[item.id] = item.value;
        }
        else if (item.type == "radio") {
            if (item.checked) {
                obj[item.name] = item.value;
                result[item.name] = item.value;
                //result.push(obj);
            }
        }
        else if (item.type == "checkbox") {
            obj[item.id] = item.checked;
            result[item.id] = item.checked;
            //result.push(obj);
        }
        else if (item.type == "select-one") {
            obj[item.id] = item.value;
            result[item.id] = item.value;
            //result.push(obj);
        } else if (item.type == "select-multiple") {
            obj[item.id] = $(item).val()? $(item).val().toString():"";
            result[item.id] = $(item).val()? $(item).val().toString():"";
        }else if (item.type == "file") {
            obj[item.id] = item.files[0];
            result[item.id] = item.files[0];
        } else if (item.localName == "img") {
            obj[item.id] = item.currentSrc
            result[item.id] = item.currentSrc
        }
        else {
            obj[item.id] = item.value;
            result[item.id] = item.value;
            //result.push(obj);

        }
    });
    return result;
}

String.IsNullOrEmpty = function (value) {
    return Object.isUndefinedOrNull(value) || value === "";
}
Object.isUndefinedOrNull = function Object$isUndefined(instance) {
    return typeof (instance) === "undefined" || instance === null;
}
String.prototype.replaceAll = function (oldValue, newValue) {
    return this.replace(new RegExp(oldValue.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'), "gm"), newValue);
}

$numberUtils = (function () {
    var toDig2 = function (value) {
        return roundTo(value, 2);
    }
    var toDig4 = function (value) {
        return roundTo(value, 4);
    }
    var isZero = function (value) {
        return isNaN(value) || Object.isUndefinedOrNull(value) || value == 0;
    }
    var toInt = function (value) {
        return roundTo(value, 0);
    }
    var roundTo = function (value, digit) {
        value = parseFloat(value).toString() == "NaN" ? 0 : parseFloat(value);
        return isZero(value) ? 0 : Math.roundTo(value, digit);
    }
    return {
        toDig2: toDig2,
        toDig4: toDig4,
        isZero: isZero,
        toInt: toInt,
        roundTo: roundTo
    }
})();

Math.roundTo = function (number, digit) {
    var n = Math.abs(number);
    if ((n - Math.floor(n)).toString().length >= 17) { //5.0001*5.5得27.500549999999997
        n += 1e-10; // 存在精度问题，做校正，实际不会用到这么小，所以不会影响到其它结果
    }
    n = n * Math.pow(10, digit);
    if (n.toString().length >= 17) { // 0.00015计算得1.4999999999999997 浮点数进度为15、16位
        n += 1e-10;
    }
    return (number < 0 ? -1 : 1) * Math.round(n) / Math.pow(10, digit);
}