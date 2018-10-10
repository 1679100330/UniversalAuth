var Login = function () {
    return {
        index: null,
        init: function () {
            layui.use(['element', 'form'], function () {
                var form = layui.form;
                $("#main form.layui-form input").click(function () {
                    var name = $(this).attr("name");
                    if (name == "username") {
                        $(this).attr("placeholder", "请输入用户");
                    } else {
                        $(this).attr("placeholder", "请输入密码");
                    }                    
                    $(this).parent().children("label").css("top", "-10px");
                    $(this).parent().children("label").css("font-size", "12px");
                    $(this).parent().children("label").css("color", "#ff6700");
                });
                $("#main form.layui-form input").blur(function () {
                    $(this).attr("placeholder", "");
                    var val = $.trim($(this).val());
                    if (val == "") {
                        $(this).parent().children("label").css("top", "9px");
                        $(this).parent().children("label").css("font-size", "14px");
                        $(this).parent().children("label").css("color", "#757575");
                    }                    
                });
                form.on('submit(login)', function (data) {
                    $.ajax({
                        type: "POST",
                        url: "DoLogin",
                        data: data.field,
                        beforeSend: function () {
                            Login.index = layer.load(1, { shade: [0.1, '#555'] });
                        },
                        success: function (r) {
                            var data = $.parseJSON(r);
                            if (data.Status == 1) {
                                window.location.href = data.Obj;
                            } else {
                                layer.msg(data.Obj);
                            }                            
                        },
                        complete: function () {
                            layer.close(Login.index);
                        },
                        error: function (textStatus, errorThrown) {
                            layer.msg("異常:" + errorThrown + ",頁面失效,請刷新頁面!");
                        }
                    });
                    return false;
                });
            });
        }
    }
}()
Login.init();