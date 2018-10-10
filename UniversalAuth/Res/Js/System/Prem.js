var Prem = function () {
    return {
        index: null,
        mytree: null,
        form: null,
        init: function () {
            layui.config({
                base: '/Res/Libs/extends/',
            }).extend({
                authtree: 'mytree',
            });
            layui.use(['element', 'form', 'mytree'], function () {
                var element = layui.element;
                Prem.form = layui.form;
                Prem.mytree = layui.mytree;
                Prem.fillPrems();
                form.on('submit(savePrem)', function (data) {
                    var pid = data.field.pid;
                    if (pid == "") {
                        layer.msg("菜单不能为空");
                        return false;
                    }
                    $.ajax({
                        type: "POST",
                        url: "SavePrem",
                        data: data.field,
                        beforeSend: function () {
                            Prem.index = layer.load(1, { shade: [0.1, '#555'] });
                        },
                        success: function (r) {
                            var data = $.parseJSON(r);
                            if (data.Status == 1) {
                                $("#reset").click();
                                Prem.fillPrems();
                            }
                            layer.msg(data.Obj);
                        },
                        complete: function () {
                            layer.close(Prem.index);
                        },
                        error: function (textStatus, errorThrown) {
                            layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                        }
                    });
                    return false;
                });
                form.on('submit(deletePrem)', function (data) {
                    var menuId = $("#formPrem input[name='id']").val();
                    if (menuId == "") {
                        layer.msg("请选择菜单");
                        return false;
                    }
                    $.ajax({
                        type: "POST",
                        url: "DeletePrem",
                        data: { menuId: menuId },
                        beforeSend: function () {
                            Prem.index = layer.load(1, { shade: [0.1, '#555'] });
                        },
                        success: function (r) {
                            var data = $.parseJSON(r);
                            if (data.Status == 1) {
                                $("#reset").click();
                                Prem.fillPrems();
                            }
                            layer.msg(data.Obj);
                        },
                        complete: function () {
                            layer.close(Prem.index);
                        },
                        error: function (textStatus, errorThrown) {
                            layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                        }
                    });
                    return false;
                });
                form.on('submit(savePremItem)', function (data) {
                    var menuId = $("#formPrem input[name='id']").val();
                    if (menuId == "") {
                        layer.msg("请选择菜单下的权限");
                        return false;
                    }
                    data.field.menuId = menuId;
                    $.ajax({
                        type: "POST",
                        url: "SavePremItem",
                        data: data.field,
                        beforeSend: function () {
                            Prem.index = layer.load(1, { shade: [0.1, '#555'] });
                        },
                        success: function (r) {
                            var data = $.parseJSON(r);
                            if (data.Status == 1) {
                                Prem.fillPremItems(menuId);
                            }
                            layer.msg(data.Obj);
                        },
                        complete: function () {
                            layer.close(Prem.index);
                        },
                        error: function (textStatus, errorThrown) {
                            layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                        }
                    });
                    return false;
                });
            });
        },
        fillPrems: function () {
            $.ajax({
                type: "POST",
                url: "GetPrems",
                beforeSend: function () {
                    Prem.index = layer.load(1, { shade: [0.1, '#555'] });
                },
                success: function (r) {
                    var data = $.parseJSON(r);
                    if (data.Status == 1) {
                        var nodes = [];
                        var menuNodes = [];
                        $.each(data.Obj, function (i, o) {
                            var tmp = {};
                            tmp.id = o.Id;
                            tmp.pid = o.Pid;
                            tmp.name = o.Name;
                            tmp.controllerName = o.ControllerName;
                            tmp.actionName = o.ActionName;
                            tmp.status = o.Status;
                            nodes.push(tmp);
                            if (o.Status == "0") {
                                menuNodes.push(tmp);
                            }
                        });
                        var tree = Prem.toTreeData(menuNodes);                                                
                        $("#formPrem select[name='pid']").html("");
                        var options = Prem.toTreeOptions(tree, 0, $("#formPrem select[name='pid']"));
                        form.render('select');
                        Prem.mytree.render('gw-tree', nodes, function (res) {
                            if (res.status == "1") {
                                form.val("formPrem", res);
                                $("#formPremItem input[name='menuId']").val(res.id);
                                Prem.fillPremItems(res.id);
                            }                            
                        });
                    }
                },
                complete: function () {
                    layer.close(Prem.index);
                },
                error: function (textStatus, errorThrown) {
                    layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                }
            });
        },
        fillPremItems: function (menuId) {
            $.ajax({
                type: "POST",
                url: "GetPremItems",
                data: { menuId: menuId },
                beforeSend: function () {
                    Prem.index = layer.load(1, { shade: [0.1, '#555'] });
                },
                success: function (r) {
                    var data = $.parseJSON(r);
                    if (data.Status == 1) {
                        if (data.Obj.length > 0) {
                            var txtHtml = "";
                            $.each(data.Obj, function (i, o) {
                                txtHtml += "<tr pk='" + o.Id + "'>";
                                txtHtml += "<td>" + (i + 1) + "</td>";
                                txtHtml += "<td>" + o.Url + "</td>";
                                txtHtml += "<td>" + o.CreateDate + "</td>";
                                txtHtml += "<td><a href='javascript:;' pk='" + o.Id + "'>删除</a></td>";
                                txtHtml += "</tr>";
                            });
                            $("#tb_prem tbody").html(txtHtml);
                            $("#tb_prem tbody a").click(function () {                                
                                var _this = this;
                                var premId = $(_this).attr("pk");
                                $.ajax({
                                    type: "POST",
                                    url: "DeletePremItem",
                                    data: { premId: premId },
                                    beforeSend: function () {
                                        Prem.index = layer.load(1, { shade: [0.1, '#555'] });
                                    },
                                    success: function (r) {
                                        var data = $.parseJSON(r);
                                        if (data.Status == 1) {
                                            $(_this).parent().parent().remove();
                                        }
                                        layer.msg(data.Obj);
                                    },
                                    complete: function () {
                                        layer.close(Prem.index);
                                    },
                                    error: function (textStatus, errorThrown) {
                                        layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                                    }
                                });
                            });
                        } else {
                            $("#tb_prem tbody").html("no data");
                        }                                               
                    } else {                        
                        layer.msg(data.Obj);
                    }
                },
                complete: function () {
                    layer.close(Prem.index);
                },
                error: function (textStatus, errorThrown) {
                    layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                }
            });
        },
        toTreeData: function (data) {
            var pos = {};
            var tree = [];
            var i = 0;
            while (data.length != 0) {
                if (data[i].pid == '#') {
                    var o = data[i];
                    o.children = [];
                    tree.push(o);
                    pos[data[i].id] = [tree.length - 1];
                    data.splice(i, 1);
                    i--;
                } else {
                    var posArr = pos[data[i].pid];
                    if (posArr != undefined) {
                        var obj = tree[posArr[0]];
                        for (var j = 1; j < posArr.length; j++) {
                            obj = obj.children[posArr[j]];
                        }
                        var o = data[i];
                        o.children = [];
                        obj.children.push(o);
                        pos[data[i].id] = posArr.concat([obj.children.length - 1]);
                        data.splice(i, 1);
                        i--;
                    }
                }
                i++;
                if (i > data.length - 1) {
                    i = 0;
                }
            }
            return tree;
        },
        toTreeOptions: function (tree,level,el) {
            for (var i = 0; i < tree.length; i++) {
                if (level == 0) {
                    var option = "<option value='" + tree[i].id + "'>" + tree[i].name + "</option>";
                    $(el).append(option);
                } else {
                    var blank = "";
                    for (var j = 0; j < level; j++) {
                        blank += "&emsp;";
                    }
                    blank += "|--";
                    var option = "<option value='" + tree[i].id + "'>" + blank + tree[i].name + "</option>";
                    $(el).append(option);
                }
                if (tree[i].children.length > 0) {
                    Prem.toTreeOptions(tree[i].children, (level + 1), el);
                }
            }
            Prem.form.render('select');
        },
    }
}()
Prem.init();