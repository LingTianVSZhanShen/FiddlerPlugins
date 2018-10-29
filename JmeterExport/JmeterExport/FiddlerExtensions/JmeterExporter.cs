using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiddler;

namespace JmeterExport.FiddlerExtensions
{
    // ProfferFormat方法第一个参数是导出下拉列表中的菜单栏名称
    // ProfferFormat方法第二个参数是选中下拉菜单后的文本描述
    [ProfferFormat("Jmeter Script", "Export JMeter Script.")]
    public class JmeterExporter : ISessionExporter, IDisposable
    {
        public JmeterExporter()
        {

        }

        public void Dispose()
        {
            
        }

        /// <summary>
        /// 导出会话
        /// </summary>
        /// <param name="sExportFormat">下拉列表中的菜单名称</param>
        /// <param name="oSessions"></param>
        /// <param name="dictOptions"></param>
        /// <param name="evtProgressNotifications"></param>
        /// <returns></returns>
        public bool ExportSessions(string sExportFormat, Session[] oSessions, Dictionary<string, object> dictOptions, EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
        {
            string filePath = null;
            bool flag = true;
            // 获取文件路径
            filePath = Utilities.ObtainSaveFilename("Export As" + sExportFormat, "Jmeter Script (*.jmx)|*.jmx");
            if (filePath == null)
            {
                return false;
            }

            try
            {
                TestPlan testPlan = new TestPlan(oSessions, filePath);
                testPlan.saveAsJmeterScript();
            }
            catch(Exception e)
            {
                FiddlerApplication.Log.LogString("导出脚本出错，错误信息如下：");
                FiddlerApplication.Log.LogString(e.Message);
                FiddlerApplication.Log.LogString(e.StackTrace);
                flag = false;
            }
            return flag;
        }
    }
}
