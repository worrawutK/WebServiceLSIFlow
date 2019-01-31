using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using Rohm.Common.Logging;

namespace WCFServiceRistiLibrary
{
    [ServiceContract(CallbackContract = typeof(IServer))]
    public interface IServiceiLibrary
    {
        [OperationContract]
        MachineOnlineStateResult OnMachineOnlineState(string mcNo, MachineOnline online);

        [OperationContract]
        UpdateMachineStateResult OnUpdateMachineState(string mcNo, MachineProcessingState state);

        /// <summary>
        /// จำเป็นต้องเก็บ recipe จาก SetupLotResult เพื่อใช้ต่อ StartLot
        /// </summary>
        /// <param name="lotNo"></param>
        /// <param name="mcNo"></param>
        /// <param name="opNo"></param>
        /// <param name="processName">เพื่อเช็คสิทธิ์</param>
        /// <returns></returns>
        [OperationContract]
        SetupLotResult OnSetupLot(string lotNo, string mcNo, string opNo, string processName);
        //CheckPermissionApcsPro(mcNo, userInf, "PL-SetupLot", c_Log)
        //LicenseWarning(userInf)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lotNo"></param>
        /// <param name="mcNo"></param>
        /// <param name="opNo"></param>
        /// <param name="recipe">recipe เอามาจาก Setup Result</param>
        /// <returns></returns>
        [OperationContract]
        StartLotResult OnStartLot(string lotNo, string mcNo, string opNo, string recipe);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lotNo"></param>
        /// <param name="opNo"></param>
        /// <param name="judge"></param>
        /// <param name="mcNo">เพื่อสร้าง log</param>
        /// <returns></returns>
        [OperationContract]
        UpdateFirstinspectionResult OnUpdateFirstinspection(string lotNo, string opNo, Judge judge, string mcNo);

        [OperationContract]
        EndLotResult OnEndLot(string lotNo, string mcNo, string opNo, int good, int ng);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lotNo"></param>
        /// <param name="opNo"></param>
        /// <param name="judge"></param>
        /// <param name="mcNo">เพื่อสร้าง log</param>
        /// <returns></returns>
        [OperationContract]
        UpdateFinalinspectionResult OnUpdateFinalinspection(string lotNo, string opNo, Judge judge, string mcNo);

        [OperationContract]
        MachineAlarmResult OnMachineAlarm(string lotNo, string mcNo, string opNo, string AlarmNo, Alarm alarm);
    }
   

    #region ClassResult
    [DataContract()]
    public class MachineAlarmResult
    {
        [DataMember()]
        public bool IsPass { get; internal set; }
        [DataMember()]
        public string Cause { get; internal set; }

        public MachineAlarmResult()
            : this(true, "")
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public MachineAlarmResult(string cause, string functionName, Logger log)
            : this(false, cause)
        {
            log.ConnectionLogger.Write(0, functionName, "", "WCFService", "iLibrary", 0, "", cause, "");
        }
        private MachineAlarmResult(bool isPass, string cause)
        {
            this.IsPass = isPass;
            this.Cause = cause;
        }
    }

    [DataContract()]
    public class UpdateFinalinspectionResult
    {
        public bool IsPass { get; internal set; }
        public string Cause { get; internal set; }

        public UpdateFinalinspectionResult()
            : this(true, "")
        {

        }
        public UpdateFinalinspectionResult(string cause, string functionName, Logger log)
            : this(false, cause)
        {
            log.ConnectionLogger.Write(0, functionName, "", "WCFService", "iLibrary", 0, "", cause, "");
        }
        private UpdateFinalinspectionResult(bool isPass, string cause)
        {
            this.IsPass = isPass;
            this.Cause = cause;
        }
    }

    [DataContract()]
    public class EndLotResult
    {
        [DataMember()]
        public bool IsPass { get; internal set; }
        [DataMember()]
        public string Cause { get; internal set; }

        public EndLotResult()
            : this(true, "")
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public EndLotResult(string cause, string functionName, Logger log)
            : this(false, cause)
        {
            log.ConnectionLogger.Write(0, functionName, "", "WCFService", "iLibrary", 0, "", cause, "");
        }
        private EndLotResult(bool isPass, string cause)
        {
            this.IsPass = isPass;
            this.Cause = cause;
        }
    }

    [DataContract()]
    public class UpdateFirstinspectionResult
    {
        [DataMember()]
        public bool IsPass { get; internal set; }

        [DataMember()]
        public string Cause { get; internal set; }

        public UpdateFirstinspectionResult()
            : this(true, "")
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public UpdateFirstinspectionResult(string cause, string functionName, Logger log)
            : this(false, cause)
        {
            log.ConnectionLogger.Write(0, functionName, "", "WCFService", "iLibrary", 0, "", cause, "");
        }
        private UpdateFirstinspectionResult(bool isPass, string cause)
        {
            this.IsPass = isPass;
            this.Cause = cause;
        }
    }

    [DataContract()]
    public class StartLotResult
    {
        [DataMember()]
        public bool IsPass { get; internal set; }

        [DataMember()]
        public string Cause { get; internal set; }

        public StartLotResult()
            : this(true, "")
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public StartLotResult(string cause, string functionName, Logger log)
            : this(false, cause)
        {
            log.ConnectionLogger.Write(0, functionName, "", "WCFService", "iLibrary", 0, "", cause, "");
        }
        private StartLotResult(bool isPass, string cause)
        {
            this.IsPass = isPass;
            this.Cause = cause;
        }
    }

    [DataContract()]
    public class SetupLotResult
    {
        [DataMember()]
        public bool IsPass { get; internal set; }

        [DataMember()]
        public string Cause { get; internal set; }
        /// <summary>
        /// จำเป็นต้องเก็บ recipe เพื่อส่งค่าตอน Start
        /// </summary>
        [DataMember()]
        public string Recipe { get; internal set; }

        /// <summary>
        /// Is Pass
        /// </summary>
        public SetupLotResult(string recipe)
            : this(true, "", recipe)
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Not Pass
        /// </summary>
        /// <param name="cause"></param>
        public SetupLotResult(string cause, string functionName, Logger log)
            : this(false, cause, "")
        {
            log.ConnectionLogger.Write(0, functionName, "", "WCFService", "iLibrary", 0, "", cause, "");

        }
        private SetupLotResult(bool isPass, string cause, string recipe)
        {
            this.IsPass = isPass;
            this.Cause = cause;
            this.Recipe = recipe;
        }
    }

    [DataContract()]
    public class UpdateMachineStateResult
    {
        [DataMember()]
        public bool IsPass { get; internal set; }
        [DataMember()]
        public string Cause { get; internal set; }

        public UpdateMachineStateResult()
            : this(true, "")
        {

        }
        public UpdateMachineStateResult(string cause, string functionName, Logger log)
            : this(false, cause)
        {
            log.ConnectionLogger.Write(0, functionName, "", "WCFService", "iLibrary", 0, "", cause, "");
        }
        private UpdateMachineStateResult(bool isPass, string cause)
        {
            this.IsPass = IsPass;
            this.Cause = cause;
        }
    }

    [DataContract()]
    public class MachineOnlineStateResult
    {
        [DataMember()]
        public bool IsPass { get; internal set; }

        [DataMember()]
        public string Cause { get; internal set; }

        /// <summary>
        /// IsPass
        /// </summary>
        public MachineOnlineStateResult()
            : this(true, "")
        {

        }
        /// <summary>
        /// Not Pass
        /// </summary>
        /// <param name="cause"></param>
        public MachineOnlineStateResult(string cause, string functionName, Logger log)
            : this(false, cause)
        {
            log.ConnectionLogger.Write(0, functionName, "", "WCFService", "iLibrary", 0, "", cause, "");
        }
        private MachineOnlineStateResult(bool isPass, string cause)
        {
            this.IsPass = isPass;
            this.Cause = cause;
        }
    }

    #endregion

    #region EnumValiable
    [DataContract()]
    public enum MachineProcessingState
    {
        [EnumMember()]
        Initial = 0,
        [EnumMember()]
        Idle = 1,
        [EnumMember()]
        Setup = 2,
        [EnumMember()]
        Ready = 3,
        [EnumMember()]
        Execute = 4,
        [EnumMember()]
        Pause = 5
    }

    [DataContract()]
    public enum MachineOnline
    {
        [EnumMember()]
        Offline = 0,
        [EnumMember()]
        Online = 1,

    }
    [DataContract()]
    public enum Judge
    {
        [EnumMember()]
        NG = 0,
        [EnumMember()]
        OK = 1
    }
    [DataContract()]
    public enum Alarm
    {
        RESET = 0,
        SET = 1
    }

    #endregion

    [ServiceContract()]
    public interface IServer
    {
        void OnMessage(MessageInfo messageInfo);
    }

    [DataContract()]
    public class MessageInfo
    {
        [DataMember()]
        public string Message { get; internal set; }

        private MessageInfo(string message)
        {
            this.Message = message;
        }
    }

    public class ServiceiLibrary
        : IServiceiLibrary
    {
        private Dictionary<string,IServiceiLibrary> c_CallbackList;
        private ServiceHost c_Host;

        public event LotEventHandler EndLot;

        public ServiceiLibrary()
        {
            c_CallbackList = new Dictionary<string, IServiceiLibrary>();
            c_Host = new ServiceHost(this, new Uri("net.Tcp://loaclhost:12345/ServiceiLibrary"));
            c_Host.AddServiceEndpoint(typeof(IServiceiLibrary), new NetTcpBinding(), "");

        }

      

        public class LotEndEventArgs
        {
            public string MCNo { get; set; }
            public string LotNo { get; set; }
            public string OperatorCode { get; set; }
            public int Good { get; set; }
            public int Ng { get; set; }

            public LotEndEventArgs()
            {
                this.Result = new EndLotResult();
            }
            public EndLotResult Result { get; }
        }
        public delegate void LotEventHandler(object sender, LotEndEventArgs e);



        public MachineOnlineStateResult OnMachineOnlineState(string mcNo, MachineOnline online)
        {
            throw new NotImplementedException();
        }

        public UpdateMachineStateResult OnUpdateMachineState(string mcNo, MachineProcessingState state)
        {
            throw new NotImplementedException();
        }

        public SetupLotResult OnSetupLot(string lotNo, string mcNo, string opNo, string processName)
        {
            throw new NotImplementedException();
        }

        public StartLotResult OnStartLot(string lotNo, string mcNo, string opNo, string recipe)
        {
            throw new NotImplementedException();
        }

        public UpdateFirstinspectionResult OnUpdateFirstinspection(string lotNo, string opNo, Judge judge, string mcNo)
        {
            throw new NotImplementedException();
        }

        public EndLotResult OnEndLot(string lotNo, string mcNo, string opNo, int good, int ng)
        {
            LotEndEventArgs args = new LotEndEventArgs()
            {
                LotNo = lotNo,
                MCNo = mcNo,
                Good = good,
                Ng = ng,
                OperatorCode = opNo
            };
            EndLot?.Invoke(this, args);

            return args.Result;
        }

        public UpdateFinalinspectionResult OnUpdateFinalinspection(string lotNo, string opNo, Judge judge, string mcNo)
        {
            throw new NotImplementedException();
        }

        public MachineAlarmResult OnMachineAlarm(string lotNo, string mcNo, string opNo, string AlarmNo, Alarm alarm)
        {
            throw new NotImplementedException();
        }
    }

}
