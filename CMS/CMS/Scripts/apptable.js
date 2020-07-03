var $table = $('#table'),$selections = [];
var $columns = []
var $tableurl = "";
var $search = true;
var $pagination = true;
var $sidePagination = "server";
var $singleSelect = true;
var $showFooter = false;
var $showRefresh = true;
var $showToggle = true;
var $showColumns = false;
var $pageSize = 25;
var $detailView = false;
var $escape = true;
var $clickToSelect = false;
var $striped = true
var $searchOnEnterKey = true;
function $onRowEditableSave(field, row, oldValue, $el) {
}
function $beforeRefreshTable() {

}
function $afterRefreshTable() {

}
function $rowClick(row)
{

}
function $onLoadSuccess(data) {
}
function $rowDoubleClick(row) {

}
function $onExpandRow(index, row, $detail) {
}
function $rowStyle(row, index) {
    return {};
}
function $onEditableChange(field, colname, row, reason) {

}
function $onLoadError(status, response) {
    if (response.responseText == "<script>window.location.href='../Login/Index';</script>") {
        alert('登录已过期，请重新登录。')
        window.location.href = '../Login/Index';
    }else
        alert(response.responseText)
}

function initTable() {
    $table.bootstrapTable({
        height: getHeight(),
        method: 'post',
        contentType: "application/x-www-form-urlencoded",
        url: $tableurl,
        pageSize: $pageSize,
        sortName: sortName,
        sortOrder: sortOrder,
        search: $search,
        searchOnEnterKey: $searchOnEnterKey,
        queryParams: queryParams,
        columns: columns,
        singleSelect:$singleSelect,
        pagination: $pagination,
        sidePagination:$sidePagination,
        onEditableSave: $onRowEditableSave,
        onLoadSuccess: $onLoadSuccess,
        showFooter: $showFooter,
        showRefresh: $showRefresh,
        showToggle: $showToggle,
        showColumns: $showColumns,
        detailView: $detailView,
        onExpandRow: $onExpandRow,
        clickToSelect: $clickToSelect,
        onLoadError:$onLoadError,
        checkboxHeader:true,
        escape: $escape,
        rowStyle:$rowStyle,
        striped:$striped
    });
    // sometimes footer render error.
    setTimeout(function () {
        $table.bootstrapTable('resetView');
    }, 200);
    $table.on('check.bs.table uncheck.bs.table ' +
            'check-all.bs.table uncheck-all.bs.table', function () {
                if("undefined" != typeof $actionmenu){ 
                    $actionmenu.prop('disabled', !$table.bootstrapTable('getSelections').length);
                }
                // save your data, here just save the current page
                $selections = getIdSelections();
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
    $table.on('dbl-click-row.bs.table', function (e, row, $element, field) {
        $rowDoubleClick(row);
        //alert(row.BusinessCode);
    });
    $table.on('all.bs.table', function (e, name, args) {

        //console.log(name, args);
    });
    $table.on('editable-hidden.bs.table', function (field, colname, row, reason) {
        $onEditableChange(field, colname, row, reason)
    });
    
    $(window).resize(function () {
        $table.bootstrapTable('resetView', {
            height: getHeight()
        });
    });
}

function refreshTable(execBefore, notchecklogin) {
    var login = true;
    if (!notchecklogin)
        login = Ajax('POST', '../../../Base/CheckLogin', {}, false);
    if (!login) {
        alert("登录状态已过期");
        window.location.href = '../../Login/Index';
    } else {

        if (execBefore)
            $beforeRefreshTable()
        $table.bootstrapTable('refresh');
        $afterRefreshTable()
    }
}


function getIdSelections() {
    return $.map($table.bootstrapTable('getSelections'), function (row) {
        return row
    });
}

function responseHandler(res) {
    $.each(res.rows, function (i, row) {
        row.state = $.inArray(row.ObjectId, $selections) !== -1;
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

function getHeight() {
    return $(window).height();
}

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

function $loadTable(initEvt)
{
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

    eachSeries(scripts, getScript, initEvt);
}

    function formatImgFormatter(value, row, index) {
        if (String.IsNullOrEmpty(value))
            return "";
        var img=document.createElement('img');
        img.setAttribute('src', value);
        img.style.width = "50px";
        img.innerHTML = value;
        return img.outerHTML;
    }

    function formatImgStyle(value, row, index) {
        return {
            css: {
                "white-space": 'nowrap',
                "text-overflow": 'ellipsis',
                "overflow": 'hidden'
            }
        }
    }