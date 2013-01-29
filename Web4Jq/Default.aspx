<%@ Page Title="主页" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        显示表格信息
    </h2>
    <div>
        <select id="spagesize" style=" width:120px; text-decoration:underline; cursor:pointer;" onchange="InitPageByData()">
        <option value="2" selected="selected">每页显示2行</option>
        <option value="4">每页显示4行</option>
        <option value="10">每页显示10行</option>
        </select>
        <table id="datas" style="margin-top:4px; border:1px solid black;" cellpadding="0" cellspacing="0" >
        <tr id="tr_head" style="height:20px; border:1px;"><td id="no" style="width:50px;" >序号</td><td id="name" style="width:50px;">姓名</td><td id="age" style="width:50px;">年龄</td><td id="datetime" style="width:120px;">注册时间</td></tr>
        </table>
        <div id="Pagination" class="pagination" style=" margin-top:5px;"><!-- 这里显示分页 --></div>
    </div>
    <input type="button" id="btnShow" value="按钮" />
    <div id="divCss" style=" width:200px; height:200px; background-color:red; font-weight:normal">
    <asp:FileUpload ID="FileUpload1" runat="server" Width="300px" Height="25px" style="background: #fafafa; border: 1px solid #3366cc;" />
    </div>
    <div>
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br />
<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
<asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
    </div>

<!-- 页面加载脚本 -->
<script type="text/javascript">
    $(function () {
        InitPageByData();
        $("#btnShow").toggle( function () {
            $("#divCss").slideDown(700);
        }, function () {
            $("#divCss").slideUp(700);
        });
    });

    var initPagination = function (maxPage, curPage) {        // 创建分页
        var num_entries = maxPage; // 总页数
        $("#Pagination").pagination(num_entries, {
            num_edge_entries: 1,    //边缘页数
            num_display_entries: 4, //主体页数
            callback: pageselectCallback,
            items_per_page: 1,  //每页显示1项
            current_page: curPage,
            prev_text: "Prev",
            next_text: "Next"
        });
    };

    function pageselectCallback(page_index, jq) {
        $.ajax({
            type: "get",
            dataType: "json",
            url: "Default.aspx",
            data: "method=gettbinfo&pageindex=" + page_index + "&pagesize=" + $("#spagesize").val() + "&t=" + Math.random(),
            complete: function () { $("#load").hide(); },
            success: BindData
        });
    }

    function InitPageByData() { //取数据，初始化分页控件
        var pageSize = $("#spagesize").val();
        $.ajax({
            type: "get",
            dataType: "json",
            url: "Default.aspx",
            data: "method=gettbinfo&pageindex=0&pagesize=" + pageSize + "&t=" + Math.random(),
            complete: function () { $("#load").hide(); },
            success: function (data) {
                var pageCount = data[1][0]["totalcount"] / pageSize;
                if (pageCount > parseInt(pageCount)) { pageCount = parseInt(pageCount) + 1; }
                initPagination(pageCount, 0);
            }
        });
    }

    function BindData(data) {
        $("#datas tr:not(:first)").remove();
        $("#datas").children()
        $.each(data[0], function (i, n) {
            var row = $("#tr_head").clone();
            row.find("#no").text(++i);
            row[s].text(n.B);
            row.find("#age").text(n.C);
            var curDate = new Date(n.D);
            var strDate = "";
            if (curDate != null) {
                strDate = curDate.getYear() + "-" + curDate.getMonth() + "-" + curDate.getDay();
            }
            row.find("#datetime").text(strDate);
            row.attr("id", "tr_" + n.A);
            row.appendTo("#datas");
        });
        $("#datas tr:first").css("background-color", "#ccc");
        $("#datas tr:not(:first)").css("background-color", "#fff");
        $("#datas tr:not(:first) td").css("border-top", "1px solid black");
    }
</script>
</asp:Content>

