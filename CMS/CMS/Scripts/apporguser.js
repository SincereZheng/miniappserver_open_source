
var $_selectedOne = true;
function createOrgTree(title, type, selOne) {

    if (selOne != undefined)
    {
        if (selOne != null)
        {
            $_selectedOne = selOne;
           
        }
    }

    var tree = jQuery.jstree.reference("#jstree_org");
    if (tree) {
        tree.destroy();
    }
    $("#sellist").html("");
    $("#myModalLabel").text(title);
    $("#selectText").text("");

    $("#selectId").val("");
    $("#selectName").val("");
    $("#selectType").val("");

    $("#selectIds").val("");
    $("#selectNames").val("");
    $("#selectTypes").val("");

    $('#jstree_org').jstree({
        "plugins": ["themes", "html_data", 'types', "ui"],
        "types": {
            "#": {
                "max_children": 1,
                "max_depth": 4,
                "valid_children": ["root"]
            },
            "root": {
                "icon": "fa fa-home",
                "valid_children": ["default"]
            },
            "default": {
                "valid_children": ["default", "file"]
            },
            "unit": {
                "icon": "fa fa-folder",
                "valid_children": []
            },
            "post": {
                "icon": "fa fa-users",
                "valid_children": []
            },
            "user": {
                "icon": "fa fa-user",
                "valid_children": []
            }
        },
        'core': {
            "animation": 0,
            "check_callback": true,
            'data': {
                'url': function (node) {
                    return node.id === '#' ?
                      '/OrganizationUser/OrgAndUser/root?filter=' + type : '/OrganizationUser/OrgAndUser/child?filter=' + type;
                },
                "data": function (node) {
                    return { "id": node.id, "r": Math.random() };
                }
            }

        }

    }).bind('select_node.jstree', function (event, data) {
        var t = data.node.type;
        if (t == type) {
            if ($_selectedOne) {
                $("#selectId").val(data.node.id);
                $("#selectName").val(data.node.text);
                $("#selectType").val(type);
                $("#selectText").text("已选择：" + data.node.text);
            }
            else {
                var ids=$("#selectIds").val();
                var names=$("#selectNames").val();
                var types = $("#selectTypes").val();
                var have = false;
                $("#selectType").val(type);
                if (ids == "") {
                    ids = data.node.id;
                    names = data.node.text;
                    types = type;
                }
                else {
 
                    if (ids.indexOf(data.node.id) > -1) {
                        have = true;
                    }
                    else {
                        ids = ids + "," + data.node.id;
                        names = names + "," + data.node.text;
                        types = types + "," + type;
                    }
                    
                }

                $("#selectIds").val(ids);
                $("#selectNames").val(names);
                $("#selectTypes").val(types);

                if (!have) {
                    var html = "<div id=\"s_" + data.node.id + "\" class=\"m-t m-l\" > <a class=\"dropdown-toggle btn btn-xs btn-info \"><span class=\"tabtitle2\">" + data.node.text + "</span><i style=\"margin-left:5px\" onclick=\"removeSel('" + data.node.id + "')\" class=\"fa fa-times\"></i></a></div>";
                    $("#sellist").append(html);
                }
                else {
                    showTip("已经选择了该用户！", "error");
                }
                
            }
        }

    });
}


function removeSel(id)
{
    var ids = $("#selectIds").val();
    var names = $("#selectNames").val();
    var types = $("#selectTypes").val();

    var idss = ids.split(",");
    var namess = names.split(",");
    var typess = types.split(",");

    var newids = "";
    var newnames = "";
    var newtypes = "";
    for (var i = 0; i < idss.length; i++)
    {
        if (idss[i] == id) {

        }
        else {
            if (newids == "") {
                newids = idss[i];
                newnames = namess[i];
                newtypes = typess[i];
            } else {
                newids = newids + "," + idss[i];
                newnames = newnames + "," + namess[i];
                newtypes = newtypes + "," + typess[i];
            }
        }
    }
    $("#selectIds").val(newids);
    $("#selectNames").val(newnames);
    $("#selectTypes").val(newtypes);
    $("#s_" + id).remove();
}