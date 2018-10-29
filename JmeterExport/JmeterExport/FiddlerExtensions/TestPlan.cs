using Fiddler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace JmeterExport.FiddlerExtensions
{
    public class TestPlan
    {
        private string filePath;
        private Session[] oSessions;

        public TestPlan()
        {

        }

        public TestPlan(Session[] oSessions)
        {

        }

        public TestPlan(Session[] oSessions, string filePath)
        {
            this.filePath = filePath;
            this.oSessions = oSessions;
        }

        private string getRequestPath(Session session)
        {
            return WebUtility.HtmlEncode(session.PathAndQuery);
        }

        private string getRequestMethod(Session session)
        {
            return session.oRequest.headers.HTTPMethod.ToUpper();
        }

        private string getRequestBody(Session session)
        {
            return WebUtility.HtmlEncode(session.GetRequestBodyAsString());
        }

        private string getProtocol(Session session)
        {
            return session.oRequest.headers.UriScheme.ToLower();
        }

        private bool isExistContentType(Session session)
        {
            return session.oRequest.headers.Exists("Content-Type");
        }

        private string getContentType(Session session)
        {
            return session.oRequest["Content-Type"];
        }

        private string getIpAddress(Session session)
        {
            return session.hostname;
        }

        public string generateContent()
        {   if (oSessions.Length == 0)
            {
                FiddlerApplication.Log.LogString("Error, There has no sessions, please check.");
                throw new Exception();
            }

            Session session0 = oSessions[0];
            string ip = getIpAddress(session0);
            string port = session0.port.ToString();
            string encoding = "utf-8";
            string protocol = getProtocol(session0);
            Element element = new Element();

            // 添加Http请求默认值
            string scriptContent = element.addConfigTestElement(null, ip, port, encoding, protocol);
            // 添加Cookie管理器
            scriptContent = element.addCookieManager(scriptContent);
            // 添加用户定义的变量
            scriptContent = element.addArguments(scriptContent);

            string requestPath = null;
            StringBuilder httpSamplerProxys = new StringBuilder();
            foreach (Session session in this.oSessions)
            {
                string content = null;
                content = element.addJSONPathAssertion(content);
                requestPath = this.getRequestPath(session);
                if (ip.Equals(getIpAddress(session)))
                {
                    ip = "";
                }
                else
                {
                    ip = getIpAddress(session);
                }

                if (port.Equals(session.port.ToString()))
                {
                    port = "";
                }
                else
                {
                    port = session.port.ToString();
                }

                if (protocol.Equals(this.getProtocol(session)))
                {
                    protocol = "";
                }
                else
                {
                    protocol = this.getProtocol(session);
                }

                string body = null;

                if (this.isExistContentType(session))
                {
                    string contentType = this.getContentType(session);
                    if ((((!contentType.Contains("boundary") && !contentType.Contains("octet - stream")) && (!contentType.Contains("image") && !contentType.Contains("video"))) && ((!contentType.Contains("audio") && !contentType.Contains("tar")) && (!contentType.Contains("zip") && !contentType.Contains("rtf")))) && ((!contentType.Contains("pdf") && !contentType.Contains("powerpoint")) && (!contentType.Contains("x-compress") && !contentType.Contains("msword"))))
                    {
                       body = getRequestBody(session);
                        
                    }
                    else
                    {
                        body = "";
                    }
                }
                else
                {
                    body = getRequestBody(session);
                }
                if (requestPath.Contains("/unode/stor/uploadPart"))
                {
                    body = "";
                }
                content = element.surroundByHTTPSamplerProxy(content, ip, port, protocol, this.getRequestMethod(session), requestPath, body, requestPath);
                httpSamplerProxys.Append(content);
            }
            // 组装线程组
            string threadGroup = element.surroundByThreadGroup(httpSamplerProxys.ToString());

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(scriptContent);
            stringBuilder.Append(threadGroup);

            // 添加查看结果树
            scriptContent = element.addViewResultTree(stringBuilder.ToString());
            // 添加断言结果树
            scriptContent = element.addAssertionResult(scriptContent);
            // 组装测试计划
            scriptContent = element.surroundByTestPlan(scriptContent);
            // 组装xml头
            FiddlerApplication.Log.LogString(scriptContent);
            scriptContent = element.addXmlHead(XDocument.Parse(scriptContent).ToString());

            return scriptContent;
        }

        public void saveAsJmeterScript()
        {
            Encoding encoding = new UTF8Encoding(true);
            string scriptContent = generateContent();
            StreamWriter streamWriter = null;
            try
            {
                streamWriter = new StreamWriter(this.filePath, false, encoding);
                streamWriter.Write(scriptContent);
                MessageBox.Show("Jmeter脚本导出成功 !", "提示", MessageBoxButtons.OK);
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                    streamWriter.Dispose();
                }
            }
                     
        }
    }
}
