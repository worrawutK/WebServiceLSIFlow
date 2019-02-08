using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TdcLotEndResult
/// </summary>
public class TdcLotEndResult
{
    public bool IsPass { get; set; }
    public string Cause { get; set; }
    public string ErrorNo { get; set; }

    public TdcLotEndResult()
        :this(true,"","")
    {

    }
    public TdcLotEndResult(string cause,string errorNo)
        :this(false,cause,errorNo)
    {

    }
    private TdcLotEndResult(bool isPass,string cause,string errorNo)
    {
        this.IsPass = isPass;
        this.Cause = cause;
        this.ErrorNo = errorNo;
    }
}