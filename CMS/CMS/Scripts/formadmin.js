var $table = $('#table'),
    $actionmenu = $('#actionmenu'),
    selections = [];
var sortName = "DisplayCreateTime";
var sortOrder = "desc";
var columns = [
                {
                    field: 'state',
                    checkbox: true,
                    align: 'center',
                    valign: 'middle'
                }
                , {
                    title: '状态',
                    field: 'StateInfo',
                    align: 'center',
                    valign: 'middle'
                }
                , {
                    title: '编码',
                    field: 'ContentCode',
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '事项',
                    field: 'Title',
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
                    title: '事项类别',
                    field: 'BusinessType',
                    align: 'left',
                    valign: 'middle',
                    sortable: true
                }
               , {
                    title: '紧急程度',
                    field: 'Urgent',
                    align: 'center',
                    valign: 'middle',
                    sortable: true
                }
                , {
                    title: '申请人',
                    field: 'OriginatorUser',
                    align: 'left',
                    valign: 'middle'
                }
                , {
                    title: '生成时间',
                    field: 'DisplayCreateTime',
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
        url: "/FormAdmin/Table/Admin",
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
        //alert(row.BusinessCode);

        openFrom(row.Title, row.Code, row.ContentCode);
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
    var ids;
    $.map($table.bootstrapTable('getSelections'), function (row) {
        ids = row;
    });
    return ids;
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


function stateFormatter(value, row, index) {

    if (value == "0")
    {
        value = "草稿";
    }
    else if (value == "1") {
        value = "已发起";
    }
    else if (value == "2") {
        value = "等待影像扫描";
    }
    else if (value == "3") {
        value = "影像已扫描";
    }
    else if (value == "4") {
        value = "等待流程审批";
    }
    else if (value == "5") {
        value = "流程审批中";
    }
    else if (value == "5") {
        value = "流程审批中";
    }
    return value;
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

    if (Number(value) == 100) {
        str = '<span style="background-color:#FFFF00">置底</span>'
    }
    else if (Number(value) == 99) {
        str = "普通";
    }
    else if (Number(value) == 98) {
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
        url: "/FromCommon/Suggest/Company?{{AllCompany}}&{{haveAll}}&key=",
        idField: "ObjectId",
        keyField: "Name",
        effectiveFields: ["Name"],
        effectiveFieldsAlias: { "Name": "名称" },
        searchFields: ["Code", "Name"],
        getDataMethod: "url"
    }).on("onDataRequestSuccess", function (e, result) {

    }).on("onSetSelectValue", function (e, keyword) {
        selCompany = keyword.id;


        refreshTable();
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

    var sls = $table.bootstrapTable('getSelections')
    if (sls.length > 0) {
        if (sls[0].MergeSign != 2 ) {
            $('#myModal').modal({ backdrop: 'static', keyboard: false });

            $("#dia_icon").addClass("fa-question-circle").removeClass("fa-exclamation");
            $("#dia_title").text("确定删除申请单");
            $("#dia_info").text("申请单被删除后不能恢复，请谨慎操作！");
            $("#dia_close").show();
            $("#dia_close2").show().text("关闭");
            $("#dia_ok").show().text("确定");

            confirmType = "del";
        }
        else {
            showTip('已合并的申请单不能被删除，请先解除合并！', "error");
        }
    }
    else {
        showTip('请勾选要删除的申请单！', "error");
    }
}

function doRemove() {
    showLoading(true, "正在删除申请单，请稍后", "dia");
    var ids = getIdSelections();
    var str = ids.ObjectId;

    $("#dia_ok").hide();
    $.ajax({
        url: '/FormAdmin/Delete/Form',
        data: "codes=" + str,
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
        field: 'ContentCode',
        values: ids
    });

    $actionmenu.prop('disabled', true);

    $('#myModal').modal('hide');

    showTip('任务删除成功！', "success");
    refreshTable();
}

function openFrom(title, typecode, code) {

    var url = "/Common/ToView/" + typecode + "Form/" + code;

    parent.showChildPage(title, url);
}

function viewRows() {
    var sls = $table.bootstrapTable('getSelections')

    if (sls.length > 0) {
        openFrom(sls[0].Title, sls[0].Code, sls[0].ContentCode);
    }
    else {
        showTip('请勾选要查看的申请单！', "error");
    }
}


