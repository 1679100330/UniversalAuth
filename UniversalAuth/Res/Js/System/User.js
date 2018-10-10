var User = function () {
    return {
        index: null,
        form: null,
        mytree: null,
        init: function () {
            layui.config({
                base: '/Res/Libs/extends/',
            }).extend({
                authtree: 'mytree',
            });
            layui.use(['element', 'form', 'mytree'], function () {
                var element = layui.element;
                User.form = layui.form;
                User.mytree = layui.mytree;
                User.fillPrems();
                User.fillRoles();
                User.fillGroups();
                //查詢用戶
                User.form.on('submit(searchUser)', function (data) {
                    $.ajax({
                        type: "POST",
                        url: "GetUser",
                        data: data.field,
                        beforeSend: function () {
                            User.index = layer.load(1, { shade: [0.1, '#555'] });
                        },
                        success: function (r) {
                            var data = $.parseJSON(r);
                            if (data.Status == 1) {                                
                                if (data.Obj.length > 0) {
                                    var txtHtml = "";
                                    $.each(data.Obj, function (i, o) {
                                        txtHtml += "<tr id='" + o.Id + "' username='" + o.Username + "' name='" + o.Name + "'>";
                                        txtHtml += "<td>" + o.Username + "</td>";
                                        txtHtml += "<td>" + o.Name + "</td>";
                                        txtHtml += "<td>" + o.CreateDate + "</td>";
                                        txtHtml += "</tr>";
                                    });
                                    $("#tb_user tbody").html(txtHtml);
                                    $("#tb_user tbody tr").click(function () {
                                        $.each($("#tb_user tbody tr"), function (i, o) {
                                            $(o).css("background", "");
                                        });
                                        $(this).css("background", "#FAEBD7");
                                        var id = $(this).attr("id");
                                        var username = $(this).attr("username");
                                        var name = $(this).attr("name");
                                        form.val("formUser", {
                                            "id": id
                                            , "username": username
                                            , "name": name
                                        });
                                        User.fillPremsByUserId(id);
                                        User.fillRolesByUserId(id);
                                        User.fillGroupsByUserId(id);
                                    });
                                } else {
                                    $("#tb_user tbody").html("no data");
                                }
                            } else {
                                layer.msg(data.Obj);
                            }
                        },
                        complete: function () {
                            layer.close(User.index);
                        },
                        error: function (textStatus, errorThrown) {
                            layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                        }
                    });
                    return false;
                });
                User.form.on('submit(saveUser)', function (data) {
                    $.ajax({
                        type: "POST",
                        url: "SaveUser",
                        data: data.field,
                        beforeSend: function () {
                            User.index = layer.load(1, { shade: [0.1, '#555'] });
                        },
                        success: function (r) {
                            var data = $.parseJSON(r);
                            if (data.Status == 1) {
                                $("#reset").click();
                                $("#searchUser").click();
                            }
                            layer.msg(data.Obj);
                        },
                        complete: function () {
                            layer.close(User.index);
                        },
                        error: function (textStatus, errorThrown) {
                            layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                        }
                    });
                    return false;
                });
                User.form.on('submit(deleteUser)', function (data) {
                    var userId = $("#formUser input[name='id']").val();
                    if (userId == "") {
                        layer.msg("请选择用户");
                        return false;
                    }
                    $.ajax({
                        type: "POST",
                        url: "DeleteUser",
                        data: { userId: userId },
                        beforeSend: function () {
                            User.index = layer.load(1, { shade: [0.1, '#555'] });
                        },
                        success: function (r) {
                            var data = $.parseJSON(r);
                            if (data.Status == 1) {
                                $("#reset").click();
                                $("#searchUser").click();
                            }
                            layer.msg(data.Obj);
                        },
                        complete: function () {
                            layer.close(User.index);
                        },
                        error: function (textStatus, errorThrown) {
                            layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                        }
                    });
                    return false;
                });
                User.form.on('submit(savePremToUser)', function (data) {
                    var tree = document.getElementById("gw-tree");
                    var inputs = tree.getElementsByTagName("input");
                    var premIds = "";
                    for (var i = 0; i < inputs.length; i++) {
                        if (inputs[i].checked) {
                            var premId = inputs[i].getAttribute("pk");
                            if (premIds == "") {
                                premIds += premId;
                            } else {
                                premIds += "," + premId;
                            }
                        }
                    }
                    var userId = $("#formUser input[name='id']").val();
                    if (userId == "") {
                        layer.msg("请选择用户");
                        return false;
                    }
                    $.ajax({
                        type: "POST",
                        url: "SavePremToUser",
                        data: {
                            userId: userId,
                            premIds: premIds
                        },
                        beforeSend: function () {
                            User.index = layer.load(1, { shade: [0.1, '#555'] });
                        },
                        success: function (r) {
                            var data = $.parseJSON(r);
                            layer.msg(data.Obj);
                        },
                        complete: function () {
                            layer.close(User.index);
                        },
                        error: function (textStatus, errorThrown) {
                            layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                        }
                    });
                    return false;
                });
                User.form.on('submit(saveRoleToUser)', function (data) {                    
                    var cb_roles = document.getElementById("cb_roles");
                    var inputs = cb_roles.getElementsByTagName("input");
                    var roleIds = "";
                    for (var i = 0; i < inputs.length; i++) {
                        if (inputs[i].checked) {
                            var roleId = inputs[i].getAttribute("pk");
                            if (roleIds == "") {
                                roleIds += roleId;
                            } else {
                                roleIds += "," + roleId;
                            }
                        }
                    }
                    var userId = $("#formUser input[name='id']").val();
                    if (userId == "") {
                        layer.msg("请选择用户");
                        return false;
                    }
                    $.ajax({
                        type: "POST",
                        url: "SaveRoleToUser",
                        data: {
                            userId: userId,
                            roleIds: roleIds
                        },
                        beforeSend: function () {
                            User.index = layer.load(1, { shade: [0.1, '#555'] });
                        },
                        success: function (r) {
                            var data = $.parseJSON(r);
                            layer.msg(data.Obj);
                        },
                        complete: function () {
                            layer.close(User.index);
                        },
                        error: function (textStatus, errorThrown) {
                            layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                        }
                    });
                    return false;
                });
                User.form.on('submit(saveGroupToUser)', function (data) {
                    var cb_groups = document.getElementById("cb_groups");
                    var inputs = cb_groups.getElementsByTagName("input");
                    var groupIds = "";
                    for (var i = 0; i < inputs.length; i++) {
                        if (inputs[i].checked) {
                            var groupId = inputs[i].getAttribute("pk");
                            if (groupIds == "") {
                                groupIds += groupId;
                            } else {
                                groupIds += "," + groupId;
                            }
                        }
                    }
                    var userId = $("#formUser input[name='id']").val();
                    if (userId == "") {
                        layer.msg("请选择用户");
                        return false;
                    }
                    $.ajax({
                        type: "POST",
                        url: "SaveGroupToUser",
                        data: {
                            userId: userId,
                            groupIds: groupIds
                        },
                        beforeSend: function () {
                            User.index = layer.load(1, { shade: [0.1, '#555'] });
                        },
                        success: function (r) {
                            var data = $.parseJSON(r);
                            layer.msg(data.Obj);
                        },
                        complete: function () {
                            layer.close(User.index);
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
                    User.index = layer.load(1, { shade: [0.1, '#555'] });
                },
                success: function (r) {
                    var data = $.parseJSON(r);
                    if (data.Status == 1) {
                        var nodes = [];
                        $.each(data.Obj, function (i, o) {
                            var tmp = {};
                            tmp.id = o.Id;
                            tmp.pid = o.Pid;
                            tmp.name = o.Name;
                            tmp.controllerName = o.ControllerName;
                            tmp.actionName = o.ActionName;
                            tmp.status = o.Status;
                            nodes.push(tmp);
                        });
                        User.mytree.render('gw-tree', nodes);
                        $("#gw-tree ul").css("display", "block");
                        $("#gw-tree i").attr("spread", "open");
                        $("#gw-tree i").html("&#xe625;");
                    }
                },
                complete: function () {
                    layer.close(User.index);
                },
                error: function (textStatus, errorThrown) {
                    layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                }
            });
        },
        fillPremsByUserId: function (userId) {
            $.ajax({
                type: "POST",
                url: "GetPremsByUserId",
                data: { userId: userId },
                beforeSend: function () {
                    User.index = layer.load(1, { shade: [0.1, '#555'] });
                },
                success: function (r) {
                    var data = $.parseJSON(r);
                    if (data.Status == 1) {
                        var tree = document.getElementById("gw-tree");
                        var inputs = tree.getElementsByTagName("input");
                        for (var i = 0; i < inputs.length; i++) {
                            inputs[i].checked = false;
                        }
                        for (var j = 0; j < inputs.length; j++) {
                            var premId = inputs[j].getAttribute("pk");
                            $.each(data.Obj, function (i, o) {
                                if (o.Id == premId) {
                                    inputs[j].checked = true;
                                }
                            });
                        }
                        User.form.render('checkbox');
                    }
                },
                complete: function () {
                    layer.close(User.index);
                },
                error: function (textStatus, errorThrown) {
                    layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                }
            });
        },
        fillRoles: function () {
            $.ajax({
                type: "POST",
                url: "GetRoles",
                beforeSend: function () {
                    User.index = layer.load(1, { shade: [0.1, '#555'] });
                },
                success: function (r) {
                    var data = $.parseJSON(r);
                    if (data.Status == 1) {
                        var txtHtml = "";
                        $.each(data.Obj, function (i,o) {
                            txtHtml += "<input type='checkbox' pk='" + o.Id + "' name='role[" + o.Id + "]' lay-skin='primary' title='" + o.Name + "'>";
                        });
                        $("#cb_roles").html(txtHtml);
                        User.form.render('checkbox');
                    } else {
                        layer.msg(data.Obj);
                    }
                },
                complete: function () {
                    layer.close(User.index);
                },
                error: function (textStatus, errorThrown) {
                    layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                }
            });
        },
        fillRolesByUserId: function (userId) {
            $.ajax({
                type: "POST",
                url: "GetRolesByUserId",
                data: { userId: userId },
                beforeSend: function () {
                    User.index = layer.load(1, { shade: [0.1, '#555'] });
                },
                success: function (r) {
                    var data = $.parseJSON(r);
                    if (data.Status == 1) {
                        var cb_roles = document.getElementById("cb_roles");
                        var inputs = cb_roles.getElementsByTagName("input");
                        for (var i = 0; i < inputs.length; i++) {
                            inputs[i].checked = false;
                        }
                        for (var j = 0; j < inputs.length; j++) {
                            var roleId = inputs[j].getAttribute("pk");
                            $.each(data.Obj, function (i, o) {
                                if (o.Id == roleId) {
                                    inputs[j].checked = true;
                                }
                            });
                        }                                                
                        User.form.render('checkbox');
                    } else {
                        layer.msg(data.Obj);
                    }
                },
                complete: function () {
                    layer.close(User.index);
                },
                error: function (textStatus, errorThrown) {
                    layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                }
            });
        },
        fillGroups: function () {
            $.ajax({
                type: "POST",
                url: "GetGroups",
                beforeSend: function () {
                    User.index = layer.load(1, { shade: [0.1, '#555'] });
                },
                success: function (r) {
                    var data = $.parseJSON(r);
                    if (data.Status == 1) {
                        var txtHtml = "";
                        $.each(data.Obj, function (i, o) {
                            txtHtml += "<input type='checkbox' pk='" + o.Id + "' name='group[" + o.Id + "]' lay-skin='primary' title='" + o.Name + "'>";
                        });
                        $("#cb_groups").html(txtHtml);
                        User.form.render('checkbox');
                    } else {
                        layer.msg(data.Obj);
                    }
                },
                complete: function () {
                    layer.close(User.index);
                },
                error: function (textStatus, errorThrown) {
                    layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                }
            });
        },
        fillGroupsByUserId: function (userId) {
            $.ajax({
                type: "POST",
                url: "GetGroupsByUserId",
                data: { userId: userId },
                beforeSend: function () {
                    User.index = layer.load(1, { shade: [0.1, '#555'] });
                },
                success: function (r) {
                    var data = $.parseJSON(r);
                    if (data.Status == 1) {
                        var cb_groups = document.getElementById("cb_groups");
                        var inputs = cb_groups.getElementsByTagName("input");
                        for (var i = 0; i < inputs.length; i++) {
                            inputs[i].checked = false;
                        }
                        for (var j = 0; j < inputs.length; j++) {
                            var groupId = inputs[j].getAttribute("pk");
                            $.each(data.Obj, function (i, o) {
                                if (o.Id == groupId) {
                                    inputs[j].checked = true;
                                }
                            });
                        }
                        User.form.render('checkbox');
                    } else {
                        layer.msg(data.Obj);
                    }
                },
                complete: function () {
                    layer.close(User.index);
                },
                error: function (textStatus, errorThrown) {
                    layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                }
            });
        },
    }
}()
User.init();