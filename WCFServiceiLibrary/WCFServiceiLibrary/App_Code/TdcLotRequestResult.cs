using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TdcLotRequestResult
/// </summary>
public class TdcLotRequestResult
{
    public bool IsPass { get;internal set; }
    public string ErrorNo { get;internal set; }
    public string Cause { get;internal set; }
    public int GoodQty { get;internal set; }
    public int NgQty { get;internal set; }

    public TdcLotRequestResult(int goodQty,int ngQty)
        :this(true,"","",goodQty,ngQty)
    {

    }
    public TdcLotRequestResult(string errorNo,string cause)
        :this(false,errorNo,cause,0,0)
    {

    }
    private TdcLotRequestResult(bool isPass,string errorNo,string cause,int goodQty,int ngQty)
    {
        this.IsPass = isPass;
        this.ErrorNo = errorNo;
        this.Cause = cause;
        this.GoodQty = goodQty;
        this.NgQty = ngQty;
    }
}