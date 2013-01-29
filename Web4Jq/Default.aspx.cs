using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using Common;
using CommLayer;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //CommLayer.Cache.Remove("");
        Response.Cookies["ctest"].Value = "12345";
        if ("gettbinfo" == Request.QueryString["method"])
        {
            int pageIndex =Convert.ToInt32(Request.QueryString["pageindex"]??"0");
            int pageSize = Convert.ToInt32(Request.QueryString["pagesize"]??"10");
            getDataByPage(pageIndex, pageSize);
        }
    }

    protected void getDataByPage(int _pageIndex,int _pageSize)
    {
        DataSet ds = new DataSet();
        DALBase dal = new DALBase();
        ds = dal.GetPagingDataSet("t_Test", " 1=1", "order by d desc", " * ", _pageIndex, _pageSize);
        DataTable dt = ds.Tables[0];
        DataRow[] drs = dt.Select("B='b10'");
        foreach(DataRow dr in drs)
        {
            dr["B"] = "X"+dr["B"].ToString();
        }
        Response.Write(Common.ConvertU.Ds2Json(ds));
        Response.End();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Label1.Text= HttpUtility.UrlEncode(TextBox1.Text.Trim());
    }
}
