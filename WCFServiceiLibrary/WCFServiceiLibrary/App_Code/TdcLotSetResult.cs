using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TdcLotSetResult
/// </summary>
public class TdcLotSetResult
{
    public bool IsPass { get; set; }
    public string Cause { get; set; }
    public string ErrorNo { get; set; }

    public TdcLotSetResult()
        :this(true,"","")
    {

    }
    public TdcLotSetResult(string errorNo, string cause)
        :this(false,errorNo,cause)
    {

    }
    private TdcLotSetResult(bool isPass,string errorNo,string cause)
    {
        this.IsPass = isPass;
        this.Cause = cause;
        this.ErrorNo = errorNo;
    }
}