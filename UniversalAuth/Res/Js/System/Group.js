var Group = function () {
    return {
        index: null,
        form: null,
        init: function () {
            layui.use(['element', 'form'], function () {
                var element = layui.element;
                Group.form = layui.form;
                Group.fillGroups();
                Group.fillRoles();
                Group.form.on('submit(saveGroup)', function (data) {
                    $.ajax({
                        type: "POST",
                        url: "SaveGroup",
                        data: data.field,
                        beforeSend: function () {
                            Group.index = layer.load(1, { shade: [0.1, '#555'] });
                        },
                        success: function (r) {
                            var data = $.parseJSON(r);
                            if (data.Status == 1) {
                                $("#reset").click();
                                Group.fillGroups();
                            }
                            layer.msg(data.Obj);
                        },
                        complete: function () {
                            layer.close(Group.index);
                        },
                        error: function (textStatus, errorThrown) {
                            layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                        }
                    });
                    return false;
                });
                Group.form.on('submit(deleteGroup)', function (data) {
                    var groupId = $("#formGroup input[name='id']").val();
                    if (groupId == "") {
                        layer.msg("请选择群组");
                        return false;
                    }
                    $.ajax({
                        type: "POST",
                        url: "DeleteGroup",
                        data: { groupId: groupId },
                        beforeSend: function () {
                            Group.index = layer.load(1, { shade: [0.1, '#555'] });
                        },
                        success: function (r) {
                            var data = $.parseJSON(r);
                            if (data.Status == 1) {
                                $("#reset").click();
                                Group.fillGroups();
                            }
                            layer.msg(data.Obj);
                        },
                        complete: function () {
                            layer.close(Group.index);
                        },
                        error: function (textStatus, errorThrown) {
                            layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                        }
                    });
                    return false;
                });
                Group.form.on('submit(saveRoleToGroup)', function (data) {
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
                    var groupId = $("#formGroup input[name='id']").val();
                    if (groupId == "") {
                        layer.msg("请选择群组");
                        return false;
                    }
                    $.ajax({
                        type: "POST",
                        url: "SaveRoleToGroup",
                        data: {
                            groupId: groupId,
                            roleIds: roleIds
                        },
                        beforeSend: function () {
                            Group.index = layer.load(1, { shade: [0.1, '#555'] });
                        },
                        success: function (r) {
                            var data = $.parseJSON(r);
                            layer.msg(data.Obj);
                        },
                        complete: function () {
                            layer.close(Group.index);
                        },
                        error: function (textStatus, errorThrown) {
                            layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                        }
                    });
                    return false;
                });
            });
        },
        fillGroups: function () {
            $.ajax({
                type: "POST",
                url: "GetGroups",
                beforeSend: function () {
                    Group.index = layer.load(1, { shade: [0.1, '#555'] });
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
                            $("#tb_group tbody").html(txtHtml);
                            $("#tb_group tbody tr").click(function () {
                                $.each($("#tb_group tbody tr"), function (i, o) {
                                    $(o).css("background", "");
                                });
                                $(this).css("background", "#FAEBD7");
                                var id = $(this).attr("pk");
                                var name = $(this).attr("name");
                                Group.form.val("formGroup", {
                                    "id": id
                                    , "name": name
                                });
                                Group.fillRolesByGroupId(id);
                                Group.fillUsersByGroupId(id);
                            });
                        } else {
                            $("#tb_group tbody").html("no data");
                        }
                    } else {
                        layer.msg(data.Obj);
                    }
                },
                complete: function () {
                    layer.close(Group.index);
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
                    Group.index = layer.load(1, { shade: [0.1, '#555'] });
                },
                success: function (r) {
                    var data = $.parseJSON(r);
                    if (data.Status == 1) {
                        var txtHtml = "";
                        $.each(data.Obj, function (i, o) {
                            txtHtml += "<input type='checkbox' pk='" + o.Id + "' name='role[" + o.Id + "]' lay-skin='primary' title='" + o.Name + "'>";
                        });
                        $("#cb_roles").html(txtHtml);
                        Group.form.render('checkbox');
                    } else {
                        layer.msg(data.Obj);
                    }
                },
                complete: function () {
                    layer.close(Group.index);
                },
                error: function (textStatus, errorThrown) {
                    layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                }
            });
        },
        fillRolesByGroupId: function (groupId) {
            $.ajax({
                type: "POST",
                url: "GetRolesByGroupId",
                data: { groupId: groupId },
                beforeSend: function () {
                    Group.index = layer.load(1, { shade: [0.1, '#555'] });
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
                        Group.form.render('checkbox');
                    } else {
                        layer.msg(data.Obj);
                    }
                },
                complete: function () {
                    layer.close(Group.index);
                },
                error: function (textStatus, errorThrown) {
                    layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                }
            });
        },
        fillUsersByGroupId: function (groupId) {
            $.ajax({
                type: "POST",
                url: "GetUsersByGroupId",
                data: { groupId: groupId },
                beforeSend: function () {
                    Group.index = layer.load(1, { shade: [0.1, '#555'] });
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
                                    url: "RemoveUserToGroup",
                                    data: {
                                        groupId: groupId,
                                        userId: userId
                                    },
                                    beforeSend: function () {
                                        Group.index = layer.load(1, { shade: [0.1, '#555'] });
                                    },
                                    success: function (r) {
                                        var data = $.parseJSON(r);
                                        if (data.Status == 1) {
                                            $(_this).parent().parent().remove();
                                        }
                                        layer.msg(data.Obj);
                                    },
                                    complete: function () {
                                        layer.close(Group.index);
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
                    layer.close(Group.index);
                },
                error: function (textStatus, errorThrown) {
                    layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                }
            });
        },
    }
}()
Group.init();