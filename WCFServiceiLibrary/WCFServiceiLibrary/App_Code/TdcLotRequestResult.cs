using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TdcLotRequestResult
/// </summary>
public class TdcLotRequestResult
{
    public bool IsPass { get; set; }
    public string ErrorNo { get; set; }
    public string Cause { get; set; }

    public TdcLotRequestResult()
        :this(true,"","")
    {

    }
    public TdcLotRequestResult(string errorNo,string cause)
        :this(false,errorNo,cause)
    {

    }
    private TdcLotRequestResult(bool isPass,string errorNo,string cause)
    {
        this.IsPass = isPass;
        this.ErrorNo = errorNo;
        this.Cause = cause;
    }
}