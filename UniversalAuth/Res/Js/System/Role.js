var Role = function () {
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
                Role.form = layui.form;
                Role.mytree = layui.mytree;
                Role.fillRoles();
                Role.fillPrems();
                Role.form.on('submit(saveRole)', function (data) {
                    $.ajax({
                        type: "POST",
                        url: "SaveRole",
                        data: data.field,
                        beforeSend: function () {
                            Role.index = layer.load(1, { shade: [0.1, '#555'] });
                        },
                        success: function (r) {
                            var data = $.parseJSON(r);
                            if (data.Status == 1) {
                                $("#reset").click();
                                Role.fillRoles();
                            }
                            layer.msg(data.Obj);
                        },
                        complete: function () {
                            layer.close(Role.index);
                        },
                        error: function (textStatus, errorThrown) {
                            layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                        }
                    });
                    return false;
                });
                Role.form.on('submit(deleteRole)', function (data) {
                    var roleId = $("#formRole input[name='id']").val();
                    if (roleId == "") {
                        layer.msg("请选择角色");
                        return false;
                    }
                    $.ajax({
                        type: "POST",
                        url: "DeleteRole",
                        data: { roleId: roleId },
                        beforeSend: function () {
                            Role.index = layer.load(1, { shade: [0.1, '#555'] });
                        },
                        success: function (r) {
                            var data = $.parseJSON(r);
                            if (data.Status == 1) {
                                $("#reset").click();
                                Role.fillRoles();
                            }
                            layer.msg(data.Obj);
                        },
                        complete: function () {
                            layer.close(Role.index);
                        },
                        error: function (textStatus, errorThrown) {
                            layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                        }
                    });
                    return false;
                });
                Role.form.on('submit(savePremToRole)', function (data) {
                    var tree = document.getElementById("gw-tree");
                    var inputs = tree.getElementsByTagName("input");
                    var premIds = "";
                    for (var i = 0; i < inputs.length; i++) {
                        if (inputs[i].checked) {
                            var premId = inputs[i].getAttribute("pk");
                            if (premIds=="") {
                                premIds += premId;
                            } else {
                                premIds += "," + premId;
                            }                            
                        }
                    }
                    var roleId = $("#formRole input[name='id']").val();
                    if (roleId == "") {
                        layer.msg("请选择角色");
                        return false;
                    }
                    $.ajax({
                        type: "POST",
                        url: "SavePremToRole",
                        data: {
                            roleId: roleId,
                            premIds: premIds
                        },
                        beforeSend: function () {
                            Role.index = layer.load(1, { shade: [0.1, '#555'] });
                        },
                        success: function (r) {
                            var data = $.parseJSON(r);                            
                            layer.msg(data.Obj);
                        },
                        complete: function () {
                            layer.close(Role.index);
                        },
                        error: function (textStatus, errorThrown) {
                            layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                        }
                    });
                    return false;
                });
            });
        },
        fillRoles: function () {
            $.ajax({
                type: "POST",
                url: "GetRoles",
                beforeSend: function () {
                    Role.index = layer.load(1, { shade: [0.1, '#555'] });
                },
                success: function (r) {
                    var data = $.parseJSON(r);
                    if (data.Status == 1) {
                        if (data.Obj.length > 0) {
                            var txtHtml = "";
                            $.each(data.Obj, function (i, o) {
                                txtHtml += "<tr pk='" + o.Id + "' name='" + o.Name + "'>";
                                txtHtml += "<td>" + (i + 1) + "</td>";
                                txtHtml += "<td>" + o.Name + "</td>";
                                txtHtml += "</tr>";
                            });                            
                            $("#tb_role tbody").html(txtHtml);
                            $("#tb_role tbody tr").click(function () {
                                $.each($("#tb_role tbody tr"), function (i, o) {
                                    $(o).css("background", "");
                                });
                                $(this).css("background", "#FAEBD7");
                                var id = $(this).attr("pk");
                                var name = $(this).attr("name");
                                Role.form.val("formRole", {
                                    "id": id
                                    , "name": name
                                });
                                Role.fillPremsByRoleId(id);
                                Role.fillUsersByRoleId(id);
                                Role.fillGroupsByRoleId(id);
                            });
                        } else {
                            $("#tb_role tbody").html("no data");
                        }
                    } else {
                        layer.msg(data.Obj);
                    }
                },
                complete: function () {
                    layer.close(Role.index);
                },
                error: function (textStatus, errorThrown) {
                    layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                }
            });
        },
        fillPrems: function () {
            $.ajax({
                type: "POST",
                url: "GetPrems",
                beforeSend: function () {
                    Role.index = layer.load(1, { shade: [0.1, '#555'] });
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
                        Role.mytree.render('gw-tree', nodes);
                        $("#gw-tree ul").css("display", "block");
                        $("#gw-tree i").attr("spread", "open");
                        $("#gw-tree i").html("&#xe625;");
                    }
                },
                complete: function () {
                    layer.close(Role.index);
                },
                error: function (textStatus, errorThrown) {
                    layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                }
            });
        },
        fillPremsByRoleId: function (roleId) {
            $.ajax({
                type: "POST",
                url: "GetPremsByRoleId",
                data: { roleId: roleId },
                beforeSend: function () {
                    Role.index = layer.load(1, { shade: [0.1, '#555'] });
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
                        form.render('checkbox');                                                
                    }
                },
                complete: function () {
                    layer.close(Role.index);
                },
                error: function (textStatus, errorThrown) {
                    layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                }
            });
        },
        fillUsersByRoleId: function (roleId) {
            $.ajax({
                type: "POST",
                url: "GetUsersByRoleId",
                data: { roleId: roleId },
                beforeSend: function () {
                    Role.index = layer.load(1, { shade: [0.1, '#555'] });
                },
                success: function (r) {
                    var data = $.parseJSON(r);
                    if (data.Status == 1) {
                        if (data.Obj.length > 0) {
                            var txtHtml = "";
                            $.each(data.Obj, function (i, o) {
                                txtHtml += "<tr>";
                                txtHtml += "<td>" + o.Username + "</td>";
                                txtHtml += "<td>" + o.Name + "</td>";
                                txtHtml += "<td><a pk='" + o.Id + "'>移除</a></td>";
                                txtHtml += "</tr>";
                            });
                            $("#tb_user tbody").html(txtHtml);
                            $("#tb_user tbody a").click(function () {
                                var _this = this;
                                var userId = $(_this).attr("pk");
                                $.ajax({
                                    type: "POST",
                                    url: "RemoveUserToRole",
                                    data: {
                                        roleId: roleId,
                                        userId: userId
                                    },
                                    beforeSend: function () {
                                        Role.index = layer.load(1, { shade: [0.1, '#555'] });
                                    },
                                    success: function (r) {
                                        var data = $.parseJSON(r);
                                        if (data.Status == 1) {
                                            $(_this).parent().parent().remove();
                                        }
                                        layer.msg(data.Obj);
                                    },
                                    complete: function () {
                                        layer.close(Role.index);
                                    },
                                    error: function (textStatus, errorThrown) {
                                        layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                                    }
                                });
                            });
                        } else {
                            $("#tb_user tbody").html("no data");
                        }
                    } else {
                        layer.msg(data.Obj);
                    }
                },
                complete: function () {
                    layer.close(Role.index);
                },
                error: function (textStatus, errorThrown) {
                    layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                }
            });
        },
        fillGroupsByRoleId: function (roleId) {
            $.ajax({
                type: "POST",
                url: "GetGroupsByRoleId",
                data: { roleId: roleId },
                beforeSend: function () {
                    Role.index = layer.load(1, { shade: [0.1, '#555'] });
                },
                success: function (r) {
                    var data = $.parseJSON(r);
                    if (data.Status == 1) {
                        if (data.Obj.length > 0) {
                            var txtHtml = "";
                            $.each(data.Obj, function (i, o) {
                                txtHtml += "<tr>";
                                txtHtml += "<td>" + o.Name + "</td>";
                                txtHtml += "<td><a pk='" + o.Id + "'>移除</a></td>";
                                txtHtml += "</tr>";
                            });
                            $("#tb_group tbody").html(txtHtml);
                            $("#tb_group tbody a").click(function () {
                                var _this = this;
                                var groupId = $(_this).attr("pk");
                                $.ajax({
                                    type: "POST",
                                    url: "RemoveGroupToRole",
                                    data: {
                                        roleId: roleId,
                                        groupId: groupId
                                    },
                                    beforeSend: function () {
                                        Role.index = layer.load(1, { shade: [0.1, '#555'] });
                                    },
                                    success: function (r) {
                                        var data = $.parseJSON(r);
                                        if (data.Status == 1) {
                                            $(_this).parent().parent().remove();
                                        }
                                        layer.msg(data.Obj);
                                    },
                                    complete: function () {
                                        layer.close(Role.index);
                                    },
                                    error: function (textStatus, errorThrown) {
                                        layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                                    }
                                });
                            });
                        } else {
                            $("#tb_group tbody").html("no data");
                        }
                    } else {
                        layer.msg(data.Obj);
                    }
                },
                complete: function () {
                    layer.close(Role.index);
                },
                error: function (textStatus, errorThrown) {
                    layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                }
            });
        },
    }
}()
Role.init();