using GameFramework;
using GameFramework.Event;
using GameFramework.Resource;
using UnityGameFramework.Runtime;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.DataTable;

namespace Penny
{
    public class ProcedureInit : ProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public static bool s_IsNeedInitResource = true;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            IsEnterNextProduce = false;
            GlobalData.GameStateType = GameStateType.Unknown;
            //初始化为0.5f
            GameEntry.Sound.SetVolume("Music", 1);
            GameEntry.Sound.SetVolume("Sound", 1);
            GameEntry.Sound.SetVolume("UISound", 1);
            GameEntry.VideoPlayer.Volume = 1;

            InitializationWithConfig(GameEntry.WindowsConfig.Config);

        }

        /// <summary>
        /// 根据配置文件初始化
        /// </summary>
        /// <param name="config"></param>
        private void InitializationWithConfig(Config config)
        {
            if (!s_IsNeedInitResource) return;
            //if (GameEntry.Socket)
            //    GameEntry.Socket.CreateNetworkChannel(config.Socket_IP, config.Socket_Port);
            if (GameEntry.Urg)
                GameEntry.Urg.StartUrgEthernet(config.URG_Address, config.URG_Port, config.URG_Offset_Width, config.URG_Offset_Height, (float)config.URG_Scale);
            if (GameEntry.SerialPort)
                GameEntry.SerialPort.StartSerialPort(config.Serial_BaudRate, config.Serial_Port, config.Screen_Land_Width, config.Screen_Land_Height, config.Serial_Offset_Width, config.Serial_Offset_Height, (float)config.Serial_Scale);
            //if (GameEntry.Ethernet)
            //    GameEntry.Ethernet.StartEthernet(config.Screen_Land_Width, config.Screen_Land_Height, config.Ethernet_Address, config.Ethernet_Port, config.Serial_Offset_Width, config.Serial_Offset_Height, (float)config.Serial_Scale);
            s_IsNeedInitResource = false;
            NextProduce();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (IsEnterNextProduce)
                ChangeState<ProcedureMindMapping>(procedureOwner);

        }

    }
}
