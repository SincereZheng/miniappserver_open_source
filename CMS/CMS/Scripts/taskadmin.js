var $table = $('#table'),
    $actionmenu = $('#actionmenu'),
    selections = [];
var sortName= "DisplayCreateTime";
var sortOrder = "desc";
var tableUrl = "/Task/Table/Task";
function $rowClick(row) {

}
var columns=[
                {
                    field: 'state',
                    checkbox: true,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '优先程度',
                    field: 'TopLevel',
                    align: 'center',
                    valign: 'middle',
                    sortable: true,
                    formatter: topLevelFormatter
                }, {
                    title: '异常重做任务',
                    field: 'CancelReason',
                    align: 'left',
                    valign: 'middle',
                    sortable: true,
                    formatter: cancelReasonFormatter
                }, {
                    title: '任务编码',
                    field: 'Code',
                    align: 'center',
                    valign: 'middle',
                }, {
                    title: '任务事项',
                    field: 'TaskModule',
                    align: 'center',
                    valign: 'middle',
                    sortable: true
                }, {
                    title: '公司',
                    field: 'CompanyName',
                    align: 'center',
                    valign: 'middle',
                    sortable: true
                }
                , {
                    title: '任务名称',
                    field: 'Title',
                    align: 'left',
                    valign: 'middle'
                }, {
                    title: '事项类别',
                    field: 'BusinessType',
                    align: 'left',
                    valign: 'middle',
                    sortable: true
                }
                , {
                    title: '业务分类',
                    field: 'BusinessCategory',
                    align: 'left',
                    valign: 'middle',
                    sortable: true
                }, {
                    title: '紧急程度',
                    field: 'Urgent',
                    align: 'center',
                    valign: 'middle',
                    sortable: true
                }
                , {
                    title: '任务时间设定',
                    field: 'NeedTime',
                    align: 'center',
                    valign: 'middle',
                    sortable: true
                }, {
                    title: '任务申请人',
                    field: 'OriginatorUser',
                    align: 'left',
                    valign: 'middle'
                }, {
                    title: '业务申请单',
                    field: 'BusinessCode',
                    align: 'left',
                    valign: 'middle'
                }
                , {
                    title: '任务生成时间',
                    field: 'DisplayCreateTime',
                    align: 'center',
                    valign: 'middle',
                    sortable: true
                }
                , {
                    title: '接单人',
                    field: 'AcceptUser',
                    align: 'center',
                    valign: 'middle',
                    sortable: true
                }, {
                    title: '接单时间',
                    field: 'DisplayAcceptTime',
                    align: 'center',
                    valign: 'middle',
                    sortable: true
                }, {
                    title: '完成时间',
                    field: 'DisplayFinishTime',
                    align: 'center',
                    valign: 'middle',
                    sortable: true
                }
]

function initTable() {
    $table.bootstrapTable({
        height: getHeight(),
        method: 'post',
        contentType: "application/x-www-form-urlencoded",
        url: tableUrl,
        pageSize: 25,
        sortName: sortName,
        sortOrder: sortOrder,
        searchOnEnterKey: true,
        queryParams: queryParams,
        columns: columns
    });
    // sometimes footer render error.
    setTimeout(function () {
        $table.bootstrapTable('resetView');
    }, 200);
    $table.on('check.bs.table uncheck.bs.table ' +
            'check-all.bs.table uncheck-all.bs.table', function () {
                $actionmenu.prop('disabled', !$table.bootstrapTable('getSelections').length);
                // save your data, here just save the current page
                selections = getIdSelections();
                // push or splice the selections if you want to save all data selections
            });
    $table.on('expand-row.bs.table', function (e, index, row, $detail) {
        if (index % 2 == 1) {
            $detail.html('Loading from ajax request...');
            $.get('LICENSE', function (res) {
                $detail.html(res.replace(/\n/g, '<br>'));
            });
        }
    });
    $table.on('click-row.bs.table', function (e, row, $element, field) {

        $rowClick(row);
        //alert(row.BusinessCode);
    });
    $table.on('all.bs.table', function (e, name, args) {
        console.log(name, args);
    });

    $(window).resize(function () {
        $table.bootstrapTable('resetView', {
            height: getHeight()
        });
    });
}

function refreshTable() {
    $table.bootstrapTable('refresh');
}


function getIdSelections() {
    return $.map($table.bootstrapTable('getSelections'), function (row) {
        return row.Code
    });
}

function responseHandler(res) {
    $.each(res.rows, function (i, row) {
        row.state = $.inArray(row.id, selections) !== -1;
    });
    return res;
}

function detailFormatter(index, row) {
    var html = [];
    $.each(row, function (key, value) {
        html.push('<p><b>' + key + ':</b> ' + value + '</p>');
    });
    return html.join('');
}

function cancelReasonFormatter(value, row, index) {
    if (value == null) {
        value = "";
    }
    return [
        '<span style="background-color:#FFFF00">' + value + '</span>'
    ].join('');
}

function displayCostTimeFormatter(value, row, index) {
    var str = "";
    if (value.indexOf("超时") > -1) {
        str = '<span style="background-color:#ffc4ba">' + value + '</span>';
    }
    else if (value.indexOf("待接单") > -1) {
        str = '<span style="background-color:#FFFF00">' + value + '</span>';
    }
   
    else {
        str = value;
    }
    return [
        str
    ].join('');
}

function isAcceptFormatter(value, row, index) {
    if (value == 1) {
        value = "已接单";
    }
    else {
        value = "未接单";
    }
    return [
        '<span >' + value + '</span>'
    ].join('');
}


function topLevelFormatter(value, row, index) {
    var str = "";
    if (Number(value) == 100)
    {
        str = '<span style="background-color:#FFFF00">置底</span>'
    }
    else if (Number(value) == 99)
    {
        str = "普通";
    }
    else if (Number(value) == 98)
    {
        str = '<span style="background-color:#FFFF00">8级</span>'
    }
    else if (Number(value) == 97) {
        str = '<span style="background-color:#FFFF00">7级</span>'
    }
    else if (Number(value) == 96) {
        str = '<span style="background-color:#FFFF00">6级</span>'
    }
    else if (Number(value) == 95) {
        str = '<span style="background-color:#FFFF00">5级</span>'
    } else if (Number(value) == 94) {
        str = '<span style="background-color:#FFFF00">4级</span>'
    } else if (Number(value) == 93) {
        str = '<span style="background-color:#FFFF00">3级</span>'
    } else if (Number(value) == 92) {
        str = '<span style="background-color:#FFFF00">2级</span>'
    } else if (Number(value) == 91) {
        str = '<span style="background-color:#FFFF00">1级</span>'
    }
    return [
        str
    ].join('');
}


window.operateEvents = {
    'click .like': function (e, value, row, index) {
        alert('You click like action, row: ' + JSON.stringify(row));
    },
    'click .remove': function (e, value, row, index) {
        delRows(row);

    }
};

function totalTextFormatter(data) {
    return 'Total';
}

function totalNameFormatter(data) {
    return data.length;
}

function totalPriceFormatter(data) {
    var total = 0;
    $.each(data, function (i, row) {
        total += +(row.price.substring(1));
    });
    return '$' + total;
}

function getHeight() {
    return $(window).height();
}

var selCompany = "all";
$(function () {
    $("#company").bsSuggest({
        url: "/Task/Suggest/Company/",
        idField: "ParentId",
        keyField: "Name",
        effectiveFields: ["Name"],
        effectiveFieldsAlias: { "Name": "名称" },
        searchFields: ["ParentId", "Name"],
        getDataMethod: "url"
    }).on("onDataRequestSuccess", function (e, result) {

    }).on("onSetSelectValue", function (e, keyword) {
        selCompany = keyword.id;


        refreshTable();
    }).on("onUnsetSelectValue", function (e) {

    });

    selPool = "";
    $("#pool").bsSuggest({
        url: "/Task/Suggest/Pool/",
        idField: "ObjectId",
        keyField: "Name",
        effectiveFields: ["Name"],
        effectiveFieldsAlias: { "Name": "名称" },
        searchFields: ["ObjectId", "Name"],
        getDataMethod: "url"
    }).on("onDataRequestSuccess", function (e, result) {

    }).on("onSetSelectValue", function (e, keyword) {
        if (selPool != keyword.id) {
            $("#group").val("");
            selGroup = "";
            $("#user").val("");
            selUser = "";

            $("#userinfo").html("");
        }
        selPool = keyword.id;

    }).on("onUnsetSelectValue", function (e) {
        $("#group").val("");

    });

    selGroup = "";
    $("#group").bsSuggest({
        url: "/Task/Suggest/Group?{{selPool_val}}&key=",
        idField: "ObjectId",
        keyField: "Name",
        effectiveFields: ["Name"],
        effectiveFieldsAlias: { "Name": "名称" },
        searchFields: ["ObjectId", "Name"],
        getDataMethod: "url"
    }).on("onDataRequestSuccess", function (e, result) {
       

    }).on("onSetSelectValue", function (e, keyword) {
        if (selGroup != keyword.id) {
            $("#user").val("");
            selUser = "";
            $("#userinfo").html("");
        }
        selGroup = keyword.id;
    }).on("onUnsetSelectValue", function (e) {

    });

    selUser = "";
    $("#user").bsSuggest({
        url: "/Task/Suggest/User?{{selGroup_val}}&key=",
        idField: "ObjectId",
        keyField: "Name",
        effectiveFields: ["Name"],
        effectiveFieldsAlias: { "Name": "名称" },
        searchFields: ["ObjectId", "Name"],
        getDataMethod: "url"
    }).on("onDataRequestSuccess", function (e, result) {

    }).on("onSetSelectValue", function (e, keyword) {
        selUser = keyword.id;
        showUserinfo(keyword.id);

    }).on("onUnsetSelectValue", function (e) {

    });

    var scripts = [

    ],
        eachSeries = function (arr, iterator, callback) {
            callback = callback || function () { };
            if (!arr.length) {
                return callback();
            }
            var completed = 0;
            var iterate = function () {
                iterator(arr[completed], function (err) {
                    if (err) {
                        callback(err);
                        callback = function () { };
                    }
                    else {
                        completed += 1;
                        if (completed >= arr.length) {
                            callback(null);
                        }
                        else {
                            iterate();
                        }
                    }
                });
            };
            iterate();
        };

    eachSeries(scripts, getScript, initTable);


});

function getScript(url, callback) {
    var head = document.getElementsByTagName('head')[0];
    var script = document.createElement('script');
    script.src = url;

    var done = false;
    // Attach handlers for all browsers
    script.onload = script.onreadystatechange = function () {
        if (!done && (!this.readyState ||
                this.readyState == 'loaded' || this.readyState == 'complete')) {
            done = true;
            if (callback)
                callback();

            // Handle memory leak in IE
            script.onload = script.onreadystatechange = null;
        }
    };

    head.appendChild(script);

    // We handle everything using the script element injection
    return undefined;
}

var confirmType = "";
function doConfirm() {
    if (confirmType == "del") {
        doRemove();
    }
    else if (confirmType == "cancel") {
        doCancel();
    }
}

function delRows() {
    var ids = getIdSelections();
    if (ids.length > 0) {
        $('#myModal').modal({ backdrop: 'static', keyboard: false });

        $("#dia_icon").addClass("fa-question-circle").removeClass("fa-exclamation");
        $("#dia_title").text("确定删除任务");
        $("#dia_info").text("任务被删除后不能恢复，请谨慎操作！");
        $("#dia_close").show();
        $("#dia_close2").show().text("关闭");
        $("#dia_ok").show().text("确定");

        confirmType = "del";
    }
    else {
        showTip('请勾选要删除的任务！', "error");
    }
}

function doRemove() {
    showLoading(true, "正在删除任务，请稍后", "dia");
    var tid = "";
    var rows = $table.bootstrapTable('getSelections');
    if (rows.length > 0) {
        tid = rows[0].ObjectId;
    }
    
    $("#dia_ok").hide();
    $.ajax({
        url: '/Task/Delete/Task/'+tid,
        data: "ids=" + tid,
        type: 'post',
        cache: false,
        success: function (data) {
            if (data.ErrorCode == "") {
                $('#myModal').modal('hide');
                doRemoveResult();
            }
            else {
                showLoading(false, "删除失败，请检查数据", "dia");
                $("#dia_close2").show().text("知道了");
            }
        },
        error: function () {
            showLoading(false, "删除失败，请检查数据", "dia");
            $("#dia_close2").show().text("知道了");
        }
    });

}

function doRemoveResult() {
    showLoading(false, "", "dia");
    var ids = getIdSelections();
    $table.bootstrapTable('remove', {
        field: 'Code',
        values: ids
    });

    $actionmenu.prop('disabled', true);

    $('#myModal').modal('hide');

    showTip('任务删除成功！', "success");
}


function cancelAccept() {
    var rows = $table.bootstrapTable('getSelections');
    if (rows.length > 0) {
        if (rows[0].IsAccept==1) {
            if (rows[0].IsFinish==1) {
                showTip('本任务已经完成，无法取消接单！', "error");
            }
            else {
                $('#myModal').modal({ backdrop: 'static', keyboard: false });

                $("#dia_icon").addClass("fa-question-circle").removeClass("fa-exclamation");
                $("#dia_title").text("确定取消接单");
                $("#dia_info").text("任务被取消接单后，将从接单人“" + rows[0].AcceptUser + "”待办任务表中消失重新回到任务池等待重新被接单！");
                $("#dia_close").show();
                $("#dia_close2").show().text("关闭");
                $("#dia_ok").show().text("确定");

                confirmType = "cancel";
            }
        }
        else {
            showTip('本任务还未有人接单，无法取消接单！', "error");
        }
    }
    else {
        showTip('请勾选要取消接单的任务！', "error");
    }
}

function doCancel() {
    var rows = $table.bootstrapTable('getSelections');
    showLoading(true, "正在取消“" + rows[0].AcceptUser + "”任务，请稍后", "dia");

    $("#dia_ok").hide();
    $.ajax({
        url: '/Task/Cancel/Accept',
        data: "objectid=" + rows[0].ObjectId,
        type: 'post',
        cache: false,
        success: function (data) {
            if (data.ErrorCode == "") {
                $('#myModal').modal('hide');
                doCancelResult();
            }
            else {
                showLoading(false, "取消失败，请检查数据", "dia");
                $("#dia_close2").show().text("知道了");
            }
        },
        error: function () {
            showLoading(false, "取消失败，请检查数据", "dia");
            $("#dia_close2").show().text("知道了");
        }
    });
}

function doCancelResult() {
    showLoading(false, "", "dia");
    refreshTable();
    $actionmenu.prop('disabled', true);

    $('#myModal').modal('hide');

    showTip('任务取消成功！', "success");
}

function viewForm() {
    var rows = $table.bootstrapTable('getSelections');
    if (rows.length > 0) {
  
        var codes = rows[0].BusinessCode
        if (codes.indexOf("|") > -1) {
            showTip('属于多单合并！', "error");
        }
        else {
            var typecode = "";
            typecode = codes.split("_")[1];

            var url = "/Common/ToView/" + typecode + "Form/" + codes;

            parent.showChildPage("申请单", url);
        }

        
    }
    else {
        showTip('请勾选要查看的任务！', "error");
    }
}

function selectAccept() {
    var rows = $table.bootstrapTable('getSelections');
    if (rows.length > 0) {
        if (rows[0].IsAccept==0) {
            $('#selectModal').modal({ backdrop: 'static', keyboard: false });

            $("#pool").val("");
            selPool = "";
            $("#group").val("");
            selGroup = "";
            $("#user").val("");
            selUser = "";
            $("#userinfo").html("");
        }
        else {
            showTip('本任务已经有人接单，无法派单！', "error");
        }
    }
    else {
        showTip('请勾选要派单的任务！', "error");
    }

}

function showUserinfo(id) {
    $.ajax({
        url: '/Task/Data/UserRecord/' + id,
        type: 'get',
        cache: false,
        success: function (data) {

            if (data.ErrorCode == "") {
                $("#userinfo").html("今天接单数：" + data.Today.Accept + " 完成数：" + data.Today.Finish + " 剩余任务数：" + data.Today.UnFinish);
            }

        },
        error: function () {

        }
    });

}

function finishUserSel() {
    var task = "";
    var rows = $table.bootstrapTable('getSelections');
    if (rows.length > 0) {
        task = rows[0].ObjectId;
    }

    var b = true;
    var error = "";
    if (selPool == "") {
        b = false;
        error = "请选择任务池！";
    }
    else if (selGroup == "") {
        b = false;
        error = "请选择任务组！";
    }
    else if (selUser == "") {
        b = false;
        error = "请选择任务接收人！";
    }
    if (!b) {
        showTip(error, "error");
    }
    else {
        $.ajax({
            url: '/Task/Accept/User/' + task + "?pool=" + selPool + "&group=" + selGroup + "&user=" + selUser + "&username=" + escape($("#user").val()) + "&r=" + Math.random(),
            type: 'get',
            cache: false,
            success: function (data) {
                if (data.ErrorCode == "") {
                    showTip('任务派单成功！', "success");

                    $('#selectModal').modal('hide');

                    refreshTable();
                }

            },
            error: function () {

            }
        });
    }

}

function changeTopLevel()
{
    var rows = $table.bootstrapTable('getSelections');
    if (rows.length > 0) {

        if (rows[0].IsAccept==0) {
            $('#levelModal').modal({ backdrop: 'static', keyboard: false });
        }
        else {
            showTip('本任务已经有人接单，无法调整！', "error");
        }
    }
    else {
        showTip('请勾选要调整的任务！', "error");
    }
}

function finishLevelSel()
{
    var task = "";
    var rows = $table.bootstrapTable('getSelections');
    if (rows.length > 0) {
        task = rows[0].ObjectId;
    }

    $.ajax({
        url: '/Task/Update/TopLevel/' + task + "?level=" + $("#toplevel").val() + "&r=" + Math.random(),
        type: 'get',
        cache: false,
        success: function (data) {
            if (data.ErrorCode == "") {
                showTip('调整任务优先程度成功！', "success");

                $('#levelModal').modal('hide');

                refreshTable();
            }

        },
        error: function () {

        }
    });
}

function openTask(module, title, typecode, code, mcode, task) {

    var url = "";
    if (module == "影像扫描" || module == "影像重扫") {
        url = "/Common/ToView/" + typecode + "Form/" + code + "?merge=" + mcode + "&task=" + task;
    }
    else if (module == "现金支付") {
        title = module;
        url = "/Pay/Pay4Cash/" + typecode + "/" + code + "?merge=" + mcode + "&task=" + task;
    }
    else if (module == "汇票支付") {
        title = module;
        url = "/Pay/Pay4Bill/" + typecode + "/" + code + "?merge=" + mcode + "&task=" + task;
    }
    else if (module == "收款发起") {
        title = module;
        url = "/BankRunning/ViewDetail/BankRunning?merge=" + mcode + "&code=" + code + "&task=" + task;
    }
    else if (module == "汇票凭证" || module == "银付凭证" || module == "转账凭证" || module == "银收凭证") {
        title = module;
        url = "/Voucher/Index/" + typecode + "/" + code + "?merge=" + mcode + "&task=" + task;
    }
    parent.showChildPage(title, url);

}