var Menu = function () {
    return {
        index: null,
        mytree: null,
        init: function () {
            layui.config({
                base: '/Res/Libs/extends/',
            }).extend({
                authtree: 'mytree',
            });
            layui.use(['element', 'form', 'mytree'], function () {
                var element = layui.element;
                var form = layui.form;
                Menu.mytree = layui.mytree;
                Menu.fillMenus();
                form.on('submit(saveMenu)', function (data) {
                    $.ajax({
                        type: "POST",
                        url: "SaveMenu",
                        data: data.field,
                        beforeSend: function () {
                            Menu.index = layer.load(1, { shade: [0.1, '#555'] });
                        },
                        success: function (r) {
                            var data = $.parseJSON(r);
                            if (data.Status == 1) {
                                $("#reset").click();
                                Menu.fillMenus();
                            }
                            layer.msg(data.Obj);
                        },
                        complete: function () {
                            layer.close(Menu.index);
                        },
                        error: function (textStatus, errorThrown) {
                            layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                        }
                    });
                    return false;
                });
                form.on('submit(deleteMenu)', function (data) {
                    var menuId = data.field.id;
                    if (menuId == "") {
                        layer.msg("请选择菜单");
                        return false;
                    }
                    $.ajax({
                        type: "POST",
                        url: "DeleteMenu",
                        data: { menuId: menuId },
                        beforeSend: function () {
                            Menu.index = layer.load(1, { shade: [0.1, '#555'] });
                        },
                        success: function (r) {
                            var data = $.parseJSON(r);
                            if (data.Status == 1) {
                                $("#reset").click();
                                Menu.fillMenus();
                            }
                            layer.msg(data.Obj);
                        },
                        complete: function () {
                            layer.close(Menu.index);
                        },
                        error: function (textStatus, errorThrown) {
                            layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                        }
                    });
                    return false;
                });
            });
        },
        fillMenus: function () {
            $.ajax({
                type: "POST",
                url: "GetMenus",
                data: {status:0},
                beforeSend: function () {
                    Menu.index = layer.load(1, { shade: [0.1, '#555'] });
                },
                success: function (r) {
                    var data = $.parseJSON(r);
                    if (data.Status == 1) {
                        var nodes =[];
                        var txtHtml = "<option value='#' selected>root</option>";
                        $.each(data.Obj, function (i, o) {
                            var tmp={};
                            tmp.id=o.Id;
                            tmp.pid=o.Pid;
                            tmp.name = o.Name;
                            tmp.controllerName = o.ControllerName;
                            tmp.actionName = o.ActionName;
                            nodes.push(tmp);
                            if(o.Pid=="#"){
                                txtHtml += "<option value='" + o.Id + "'>" + o.Name + "</option>";
                            }                            
                        });
                        $("#formMenu select[name='pid']").html(txtHtml);
                        form.render('select');
                        Menu.mytree.render('gw-tree', nodes,function(res){                            
                            form.val("formMenu", res);                            
                        });
                    }                    
                },
                complete: function () {
                    layer.close(Menu.index);
                },
                error: function (textStatus, errorThrown) {
                    layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                }
            });
        },
    }
}()
Menu.init();